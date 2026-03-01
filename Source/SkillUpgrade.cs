using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using EntityStates;
using R2API.Utils;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using UnityEngine;
using UnityEngine.Networking;

namespace SkillsPlusPlus {

    [RequireComponent(typeof(CharacterBody))]
    public class SkillUpgrade : NetworkBehaviour {

        [SyncVar(hook = "OnSkillLevelChanged")]
        public int skillLevel;

        public SkillPointsController skillPointsController { get; private set; }
        public GenericSkill targetGenericSkill;

        public string targetBaseSkillName;

        CharacterBody characterBody;

        private bool isSurvivorEnabled {
            get { return skillPointsController?.isSurvivorEnabled == true; }
        }

        void Awake() {
            this.characterBody = this.GetComponent<CharacterBody>();
            this.targetBaseSkillName = ((ScriptableObject)targetGenericSkill.skillDef)?.name;
            Logger.Debug("Awake() targetBaseSkillName: {0}", targetBaseSkillName);
        }

        void OnEnable() {
            Logger.Debug("OnEnable() targetBaseSkillName: {0}", targetBaseSkillName);
            this.targetGenericSkill.onSkillChanged += this.OnSkillChanged;
            On.EntityStates.BaseState.OnEnter += this.OnBaseStateEnter;
            On.EntityStates.EntityState.OnExit += this.OnBaseStateExit;

            RefreshUpgrades();
        }

        void OnDisable() {
            Logger.Debug("OnDisable() targetBaseSkillName: {0}", targetBaseSkillName);
            On.EntityStates.BaseState.OnEnter -= this.OnBaseStateEnter;
            On.EntityStates.EntityState.OnExit -= this.OnBaseStateExit;
            if (targetGenericSkill) {
                targetGenericSkill.onSkillChanged -= this.OnSkillChanged;
            }

            if (this.skillPointsController) {
                this.skillPointsController.PersistUpgrade(skillLevel, this.targetBaseSkillName);
            }
        }

        #region Getters and setters

        public void SetSkillPointsController(SkillPointsController skillPointsController) {
            if (this.skillPointsController) {
                Logger.Warn("Setting the skill points controller a second time is irregular behaviour. It should be added just once when the character body enables");
            }
            this.skillPointsController = skillPointsController;
            RefreshUpgrades();
        }

        #endregion

        #region Events

        void OnSkillChanged(GenericSkill genericSkill) {

            this.targetBaseSkillName = ((ScriptableObject)genericSkill.skillDef)?.name;
            Logger.Debug("OnSkillChanged({0})", targetBaseSkillName);
            RefreshUpgrades();
        }

        [Client]
        void OnSkillLevelChanged(int newSkillLevel) {
            Logger.Debug("OnSkillLevelChanged({0})", newSkillLevel);
            this.skillLevel = newSkillLevel;
            RefreshUpgrades();
        }

        #endregion

        void RefreshUpgrades() {
            if (!isSurvivorEnabled) {
                Logger.Debug("Couldn't refresh upgrades because the survivor is disabled. targetBaseSkillName: {0}", targetBaseSkillName);
                return;
            };
            var activeSkillDef = GetActiveSkillDef(targetGenericSkill);
            if (activeSkillDef == null) {
                Logger.Debug("Couldn't refresh upgrades because there is no active skill. targetBaseSkillName: {0}", targetBaseSkillName);
                return;
            }
            Logger.Debug("RefreshUpgrades() activeSkillDef: {0}", ((ScriptableObject)activeSkillDef)?.name);
            var modifier = SkillModifierManager.GetSkillModifier(activeSkillDef);
            if (modifier != null) {
                // TODO: rename OnSkillLeveledUp to OnSkillChanged
                modifier.characterBody = this.characterBody;
                modifier.OnSkillLeveledUp(this.skillLevel, this.characterBody, activeSkillDef);
                if (targetGenericSkill) {
                    targetGenericSkill.RecalculateValues();
                }
            }
        }

        public void OnBuySkill() {
            CmdOnBuySkill(this.targetBaseSkillName);
        }

        [Command]
        [Server]
        private void CmdOnBuySkill(string targetSkillName) {
            if (!isSurvivorEnabled) return;
            var allSkillUpgrades = this.GetComponents<SkillUpgrade>();
            foreach (var skillUpgrade in allSkillUpgrades) {
                if (skillUpgrade.targetBaseSkillName == targetSkillName) {
                    if (skillPointsController && skillUpgrade.CanUpgradeSkill()) {
                        skillPointsController.DeductSkillPoints(1);
                        skillUpgrade.skillLevel += 1;
                        Logger.Debug("CmdOnBuySkill({0}): skillLevel: {1}", skillUpgrade.targetBaseSkillName, skillUpgrade.skillLevel);
                        skillUpgrade.RefreshUpgrades();
                    }
                }
            }
        }

        public bool CanUpgradeSkill() {
            if (!isSurvivorEnabled) return false;
            if (targetGenericSkill == null) return false;
            if (skillPointsController && !skillPointsController.hasUnspentPoints) return false;
            return SkillModifierManager.HasSkillModifier(this.targetBaseSkillName);
        }

        static List<EntityState> pastStates = new List<EntityState>();

        private CharacterBody FindOwningCharacterBody(EntityState state) {
            if (state == null) {
                Logger.Debug("State was null, returning.");
                return null;
            }

            bool bLogging = !pastStates.Contains(state);
            if (bLogging) pastStates.Add(state);

            if (state.outer) {
                if (state.outer.TryGetComponent(out CharacterBody characterBody)) {
                    if (characterBody.master && characterBody.master.gameObject.TryGetComponent(out MinionOwnership minion) && minion.ownerMaster) {
                        // GetBody() isn't guaranteed to not return a null reference
                        var masterBody = minion.ownerMaster.GetBody();
                        if (bLogging) Logger.Debug(state.GetType().Name + ": Found MinionOwner in CharacterBody, returning " + (masterBody?.GetDisplayName() ?? "[INVALID/REMOVED]"));
                        return minion.ownerMaster.GetBody();
                    }

                    if (bLogging) Logger.Debug(state.GetType().Name + ": Found CharacterBody, returning " + characterBody.GetDisplayName());
                    RefreshUpgrades(); // kinda a hacky way to do it ? idk it works and 4 am <3 
                    return characterBody;
                } else if (state.outer.TryGetComponent(out ProjectileController projectileController) && projectileController.owner) {
                    if (bLogging) Logger.Debug(state.GetType().Name + ": Found ProjectileController, returning " + projectileController.owner.GetComponent<CharacterBody>().GetDisplayName());
                    return projectileController.owner.GetComponent<CharacterBody>();
                } else if (state.outer.TryGetComponent(out MinionOwnership minionOwnership) && minionOwnership.ownerMaster) {
                    var masterBody = minionOwnership.ownerMaster.GetBody();
                    if (bLogging) Logger.Debug(state.GetType().Name + ": Found MinionOwnership, returning " + (masterBody?.GetDisplayName() ?? "[INVALID/REMOVED]"));
                    return minionOwnership.ownerMaster.GetBody();
                } else if (state.outer.TryGetComponent(out GenericOwnership genericOwnership) && genericOwnership.ownerObject) {
                    if (bLogging) Logger.Debug(state.GetType().Name + ": Found GenericOwnership, returning " + genericOwnership.ownerObject.GetComponent<CharacterBody>().GetDisplayName());
                    return genericOwnership.ownerObject.GetComponent<CharacterBody>();
                } else if (state.outer.TryGetComponent(out Deployable deployable) && deployable.ownerMaster) {
                    var masterBody = deployable.ownerMaster.GetBody(); // This isn't a gup, but knowing GetBody() can return null we should be checking it anyway
                    if (bLogging) Logger.Debug(state.GetType().Name + ": Found Deployable, returning " + (masterBody?.GetDisplayName() ?? "[INVALID/REMOVED]"));
                    return deployable.ownerMaster.GetBody();
                }
            }
            return null;
        }

        private static SkillDef GetActiveSkillDef(GenericSkill genericSkill) {
            if (genericSkill == null) {
                return null;
            }

            if (genericSkill.skillDef != null) {
                return genericSkill.skillDef;
            }

            return genericSkill.baseSkill;
        }

        private void OnBaseStateEnter(On.EntityStates.BaseState.orig_OnEnter orig, BaseState self) {
            if (self == null) // we can probably ujsthand this to the main func then <3 ,.
            {
                orig(self);
                return;
            }
            if (isSurvivorEnabled && FindOwningCharacterBody(self)?.gameObject == this.gameObject && self is BaseState baseState) {
                var skillModifier = SkillModifierManager.GetSkillModifiersForEntityStateType(baseState.GetType());
                var activeSkillDef = GetActiveSkillDef(this.targetGenericSkill);
                if (skillModifier != null && activeSkillDef != null && skillModifier.skillNames.Contains(((ScriptableObject)activeSkillDef)?.name)) {
                    skillModifier.OnSkillEnter(skillState: baseState, this.skillLevel);
                    Logger.Debug("skill level = " + skillLevel);
                    orig(self);
                    return;
                }
            }
            orig(self);

        }

        private void OnBaseStateExit(On.EntityStates.EntityState.orig_OnExit orig, EntityState self) {
            if (isSurvivorEnabled && self is BaseState baseState && FindOwningCharacterBody(self)?.gameObject == this.gameObject) {
                var skillModifier = SkillModifierManager.GetSkillModifiersForEntityStateType(baseState.GetType());
                var activeSkillDef = GetActiveSkillDef(this.targetGenericSkill);
                if (skillModifier != null && activeSkillDef != null && skillModifier.skillNames.Contains(((ScriptableObject)activeSkillDef)?.name)) {
                    skillModifier.OnSkillExit(skillState: baseState, this.skillLevel);
                    orig(self);
                    return;
                }
            }
            orig(self);
        }

    }
}