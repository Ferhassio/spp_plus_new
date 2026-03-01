using EntityStates;
using EntityStates.Railgunner.Backpack;
using EntityStates.Railgunner.Reload;
using EntityStates.Railgunner.Scope;
using EntityStates.Railgunner.Weapon;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static R2API.RecalculateStatsAPI;

namespace SkillsPlusPlus.Modifiers
{
    [SkillLevelModifier("RailgunnerBodyFirePistol", typeof(FirePistol))]
    internal class RailgunnerXQRSkillModifier : SimpleSkillModifier<FirePistol>
    {
        private float originalDuration = 0;
        private float originalLifetime = 0;

        public override void OnSkillEnter(FirePistol skillState, int level)
        {
            if (Mathf.Abs(originalDuration) < 0.01f)
            {
                originalDuration = skillState.baseDuration;

                ProjectileSimple prefabSimple = skillState.projectilePrefab.GetComponent<ProjectileSimple>();
                if (prefabSimple)
                {
                    originalLifetime = prefabSimple.lifetime;
                }
                else
                {
                    SkillsPlusPlus.Logger.Error("Failed to find the Railgunner ProjectilePrefab somehow");
                }
            }

            skillState.baseDuration = MultScaling(originalDuration, -0.10f, level);

            {
                ProjectileSimple prefabSimple = skillState.projectilePrefab.GetComponent<ProjectileSimple>();
                if (prefabSimple)
                {
                    prefabSimple.lifetime = MultScaling(originalLifetime, 0.20f, level);
                }
            }

            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }
    }

    [SkillLevelModifier("RailgunnerBodyScopeHeavy", typeof(FireSnipeHeavy))]
    internal class RailgunnerM99SkillModifier : SimpleSkillModifier<FireSnipeHeavy>
    {
        private static int EmpoweredRoundsLeft = 0;
        private static int MaxEmpoweredRounds = 1;
        private static SkillUpgrade M99skill;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            MaxEmpoweredRounds = level + 1;
            skillDef.baseMaxStock = 1 + level / 2;

            if (!M99skill)
            {
                M99skill = registeredSkill;
            }
        }

        public override void SetupSkill()
        {
            On.EntityStates.Railgunner.Reload.Boosted.ConsumeBoost += new On.EntityStates.Railgunner.Reload.Boosted.hook_ConsumeBoost(ConsumeBoost);
            On.EntityStates.Railgunner.Reload.BoostConfirm.OnEnter += new On.EntityStates.Railgunner.Reload.BoostConfirm.hook_OnEnter(OnEnterBoostConfirm);

            base.SetupSkill();
        }

        public static void ConsumeBoost(On.EntityStates.Railgunner.Reload.Boosted.orig_ConsumeBoost orig, Boosted self, bool queueReload)
        {
            //Do not waste the empowered round if we have more left to shoot.
            if (--EmpoweredRoundsLeft > 0)
            {
                RoR2.Util.PlaySound(self.boostConsumeSoundString, self.gameObject);
            }
            else
            {
                orig(self, queueReload);
            }
        }

        public static void OnEnterBoostConfirm(On.EntityStates.Railgunner.Reload.BoostConfirm.orig_OnEnter orig, BoostConfirm self)
        {
            if (M99skill)
            {
                EmpoweredRoundsLeft = Math.Min(M99skill.targetGenericSkill.maxStock, MaxEmpoweredRounds);
            }

            orig(self);
        }
    }

    [SkillLevelModifier("RailgunnerBodyScopeLight", typeof(FireSnipeLight), typeof(WindUpScopeLight), typeof(WindDownScopeLight))]
    internal class RailgunnerHH44SkillModifier : BaseSkillModifier
    {
        private static BuffDef KillingSpreeBuff;
        private static BuffDef StrategicRetreatBuff;
        private static SkillUpgrade HH44Skill;
        private static bool bScopeActive;
        private static float KillingSpreeTimer = 0;
        private static float SpreeTimerGrace = 2f;

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            if (skillState is FireSnipeLight)
            {
                this.OnFireSnipeLightEnter((FireSnipeLight)skillState, level);
            }
            else if (skillState is WindUpScopeLight)
            {
                this.OnWindUpScopeLightEnter((WindUpScopeLight)skillState, level);
            }
            else if (skillState is WindDownScopeLight)
            {
                this.OnWindDownScopeLightEnter((WindDownScopeLight)skillState, level);
            }

            base.OnSkillEnter(skillState, level);
        }

        public void OnFireSnipeLightEnter(FireSnipeLight skillState, int level)
        {
        }

        public void OnWindUpScopeLightEnter(WindUpScopeLight skillState, int level)
        {
            bScopeActive = true;
        }

        public void OnWindDownScopeLightEnter(WindDownScopeLight skillState, int level)
        {
            bScopeActive = false;

            if (skillState.outer.commonComponents.characterBody.HasBuff(KillingSpreeBuff))
            {
                int KSBuffCount = skillState.outer.commonComponents.characterBody.GetBuffCount(KillingSpreeBuff);
                skillState.outer.commonComponents.characterBody.SetBuffCount(KillingSpreeBuff.buffIndex, 0);

                float SRDuration = 0.25f * level * KSBuffCount;

                foreach (CharacterBody.TimedBuff buff in skillState.outer.commonComponents.characterBody.timedBuffs)
                {
                    if (buff.buffIndex == StrategicRetreatBuff.buffIndex)
                    {
                        buff.timer += SRDuration;
                        return;
                    }
                }
                skillState.outer.commonComponents.characterBody.AddTimedBuff(StrategicRetreatBuff, SRDuration);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (!HH44Skill)
            {
                HH44Skill = registeredSkill;
            }
        }

        public void RegisterKillingBuffs()
        {
            {
                BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();

                buffDef.buffColor = new Color(0.78f, 0.20f, 0.78f);
                buffDef.canStack = true;
                buffDef.eliteDef = null;
                buffDef.iconSprite = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/CritOnUse/bdFullCrit.asset").WaitForCompletion().iconSprite;
                buffDef.isDebuff = true;
                buffDef.name = "RailgunnerKillingSpreeBuff";

                KillingSpreeBuff = buffDef;
                ContentAddition.AddBuffDef(buffDef);
            }

            {
                BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();

                buffDef.buffColor = new Color(0.74f, 0.36f, 0.23f);
                buffDef.canStack = false;
                buffDef.eliteDef = null;
                buffDef.iconSprite = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/MoveSpeedOnKill/bdKillMoveSpeed.asset").WaitForCompletion().iconSprite;
                buffDef.isDebuff = true;
                buffDef.name = "RailgunnerStrategicRetreatBuff";

                StrategicRetreatBuff = buffDef;
                ContentAddition.AddBuffDef(buffDef);
            }
        }

        public void GlobalEventManager_OnCharacterDeath(On.RoR2.GlobalEventManager.orig_OnCharacterDeath orig, RoR2.GlobalEventManager self, RoR2.DamageReport damageReport)
        {
            if (bScopeActive && damageReport.attackerBody == HH44Skill.targetGenericSkill.characterBody && HH44Skill.skillLevel > 0)
            {
                damageReport.attackerBody.AddBuff(KillingSpreeBuff);
                KillingSpreeTimer = SpreeTimerGrace;
            }

            orig(self, damageReport);
        }

        public void CharacterBody_Update(On.RoR2.CharacterBody.orig_Update orig, RoR2.CharacterBody self)
        {
            if (HH44Skill && self == HH44Skill.targetGenericSkill.characterBody && self.HasBuff(KillingSpreeBuff))
            {
                KillingSpreeTimer -= Time.deltaTime;

                if (KillingSpreeTimer < 0f)
                {
                    KillingSpreeTimer += SpreeTimerGrace;
                    if (self.GetBuffCount(KillingSpreeBuff) > 1)
                    {
                        self.SetBuffCount(KillingSpreeBuff.buffIndex, self.GetBuffCount(KillingSpreeBuff) - 1);
                    }
                    else
                    {
                        self.RemoveBuff(KillingSpreeBuff);
                    }
                }
            }

            orig(self);
        }

        public static void RecalculateStats(CharacterBody sender, StatHookEventArgs args)
        {
            if (!sender)
            {
                return;
            }
            if (sender.HasBuff(StrategicRetreatBuff))
            {
                args.baseMoveSpeedAdd += 3f;
            }
            if (sender.HasBuff(KillingSpreeBuff))
            {
                args.armorAdd += (sender.GetBuffCount(KillingSpreeBuff) * HH44Skill.skillLevel);
            }
        }

        public override void SetupSkill()
        {
            RegisterKillingBuffs();

            On.RoR2.GlobalEventManager.OnCharacterDeath += GlobalEventManager_OnCharacterDeath;
            On.RoR2.CharacterBody.Update += CharacterBody_Update;
            GetStatCoefficients += RecalculateStats;

            base.SetupSkill();
        }
    }

    [SkillLevelModifier("RailgunnerBodyFireMineConcussive", typeof(FireMineConcussive))]
    internal class RailgunnerConcussiveMineSkillModifier : SimpleSkillModifier<FireMineConcussive>
    {
        private static BuffDef FeatherFallBuff;
        private static int ConcussiveProjectileCatalogIndex = -1337;
        private static float baseThrowForce = 0;
        private static SkillUpgrade ConcussiveMineSkill;

        public override void OnSkillEnter(FireMineConcussive skillState, int level)
        {
            if (ConcussiveProjectileCatalogIndex == -1337)
            {
                if (skillState.projectilePrefab.TryGetComponent(out ProjectileController projectileController))
                {
                    ConcussiveProjectileCatalogIndex = projectileController.catalogIndex;
                }

                if (skillState.projectilePrefab.TryGetComponent(out ProjectileSimple projectileSimple))
                {
                    baseThrowForce = projectileSimple.desiredForwardSpeed;
                }
            }

            if (skillState.projectilePrefab.TryGetComponent(out ProjectileSimple projectile))
            {
                projectile.desiredForwardSpeed = MultScaling(baseThrowForce, 0.15f, level);
            }

            base.OnSkillEnter(skillState, level);
        }
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (!ConcussiveMineSkill)
            {
                ConcussiveMineSkill = registeredSkill;
            }
        }

        public override void SetupSkill()
        {
            RegisterFeatherFallBuff();

            On.RoR2.HealthComponent.TakeDamage += new On.RoR2.HealthComponent.hook_TakeDamage(RailgunnerConcussiveMineSkillModifier.HealthComponent_TakeDamage);
            On.RoR2.CharacterBody.FixedUpdate += new On.RoR2.CharacterBody.hook_FixedUpdate(RailgunnerConcussiveMineSkillModifier.CharacterBody_FixedUpdate);

            base.SetupSkill();
        }

        public void RegisterFeatherFallBuff()
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();

            buffDef.buffColor = new Color(1f, 0.8f, 0.55f);
            buffDef.canStack = false;
            buffDef.eliteDef = null;
            buffDef.iconSprite = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Bandit2/bdCloakSpeed.asset").WaitForCompletion().iconSprite;

            buffDef.isDebuff = false;
            buffDef.name = "RailgunnerFeatherFallBuff";

            FeatherFallBuff = buffDef;
            ContentAddition.AddBuffDef(buffDef);
        }

        public static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
        {
            if (di.inflictor && di.inflictor.TryGetComponent(out ProjectileController controller) && controller.catalogIndex == ConcussiveProjectileCatalogIndex)
            {
                if (ConcussiveMineSkill.skillLevel != 0)
                {
                    self.body.AddTimedBuff(FeatherFallBuff, 1.0f + (0.5f * ConcussiveMineSkill.skillLevel));
                    if (self.body != ConcussiveMineSkill.targetGenericSkill.characterBody)
                    {
                        self?.body?.characterMotor?.AddDisplacement(new Vector3(0f, 5f, 0f));
                    }
                }
            }

            orig.Invoke(self, di);
        }

        public static void CharacterBody_FixedUpdate(On.RoR2.CharacterBody.orig_FixedUpdate orig, CharacterBody self)
        {
            if (self && self.HasBuff(FeatherFallBuff) && !self.characterMotor.isGrounded)
            {
                float upVelocity = self.characterMotor.velocity.y;

                upVelocity = Mathf.Max(upVelocity, -30f * (3.5f / (ConcussiveMineSkill.skillLevel + 3.5f)));
                self.characterMotor.velocity = new Vector3(self.characterMotor.velocity.x, upVelocity, self.characterMotor.velocity.z);
            }

            orig(self);
        }
    }

    [SkillLevelModifier("RailgunnerBodyFireMineBlinding", typeof(FireMineBlinding))]
    internal class RailgunnerPolarFieldMineSkillModifier : SimpleSkillModifier<FireMineBlinding>
    {
        private static BuffDef TimePressureDebuff;
        private static int PolarProjectileCatalogIndex = -1337;
        private static float baseThrowForce = 0;
        private static SkillUpgrade PolarMineSkill;
        private static float baseRadius = 0;
        private static List<BuffWard> Wards = new List<BuffWard>();

        public override void OnSkillEnter(FireMineBlinding skillState, int level)
        {
            FirstTimeSetup(skillState);

            if (skillState.projectilePrefab.TryGetComponent(out ProjectileSimple projectile))
            {
                projectile.desiredForwardSpeed = MultScaling(baseThrowForce, 0.15f, level);
            }

            base.OnSkillEnter(skillState, level);
        }

        private static void FirstTimeSetup(FireMineBlinding skillState)
        {
            if (PolarProjectileCatalogIndex == -1337)
            {
                if (skillState.projectilePrefab.TryGetComponent(out ProjectileExplosion projectileExplosion))
                {
                    if (projectileExplosion.childrenProjectilePrefab.TryGetComponent(out BuffWard buffWard))
                    {
                        baseRadius = buffWard.radius;

                        BuffWard copy = (BuffWard)(projectileExplosion.childrenProjectilePrefab.AddComponent(buffWard.GetType()));

                        Wards.Add(buffWard);
                        Wards.Add(copy);

                        System.Reflection.FieldInfo[] fields = buffWard.GetType().GetFields();
                        foreach (System.Reflection.FieldInfo field in fields)
                        {
                            field.SetValue(copy, field.GetValue(buffWard));
                        }
                    }
                }

                if (skillState.projectilePrefab.TryGetComponent(out ProjectileController projectileController))
                {
                    PolarProjectileCatalogIndex = projectileController.catalogIndex;
                }
                if (skillState.projectilePrefab.TryGetComponent(out ProjectileSimple projectileSimple))
                {
                    baseThrowForce = projectileSimple.desiredForwardSpeed;
                }
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (!PolarMineSkill)
            {
                PolarMineSkill = registeredSkill;
            }

            bool bCheckedForOriginal = false;
            foreach (BuffWard ward in Wards)
            {
                ward.radius = MultScaling(baseRadius, 0.15f, level);
                if (PolarMineSkill.skillLevel > 0 && bCheckedForOriginal)
                {
                    ward.buffDef = TimePressureDebuff;
                }
                else
                {
                    bCheckedForOriginal = true;
                }
            }
        }

        public override void SetupSkill()
        {
            RegisterTimePressureDebuff();

            On.RoR2.HealthComponent.TakeDamage += new On.RoR2.HealthComponent.hook_TakeDamage(HealthComponent_TakeDamage);

            base.SetupSkill();
        }

        public static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
        {
            if (self.body.HasBuff(TimePressureDebuff))
            {
                di.damage = MultScaling(di.damage, 0.1f, PolarMineSkill.skillLevel);
            }
            orig(self, di);
        }

        public void RegisterTimePressureDebuff()
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();

            buffDef.buffColor = new Color(0.5f, 0.6f, 1f);
            buffDef.canStack = false;
            buffDef.eliteDef = null;
            buffDef.iconSprite = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Grandparent/bdOverheat.asset").WaitForCompletion().iconSprite;
            buffDef.isDebuff = true;
            buffDef.name = "RailgunnerTimePressureBuff";

            TimePressureDebuff = buffDef;
            ContentAddition.AddBuffDef(buffDef);
        }
    }

    [SkillLevelModifier("RailgunnerBodyChargeSnipeSuper", typeof(FireSnipeSuper))]
    internal class RailgunnerSuperchargeSkillModifier : SimpleSkillModifier<FireSnipeSuper>
    {
        private static float originalProcRate = 0f;
        private static float originalCritMult = 0f;
        private static SkillUpgrade superchargeSkill;

        private static float originalRechargeRate;

        public override void OnSkillEnter(FireSnipeSuper skillState, int level)
        {
            if (Mathf.Abs(originalProcRate) < 0.1f)
            {
                originalProcRate = skillState.procCoefficient;
                originalCritMult = skillState.critDamageMultiplier;
            }

            if (level > 0)
            {
                skillState.procCoefficient = MultScaling(originalProcRate, 0.1f, level);
                skillState.critDamageMultiplier = MultScaling(originalCritMult, 0.2f, level);
            }

            base.OnSkillEnter(skillState, level);
        }
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (!superchargeSkill)
            {
                superchargeSkill = registeredSkill;
            }

            originalRechargeRate = 1 - (5f / (level + 5f));
            Logger.Debug(originalRechargeRate);
        }

        public override void SetupSkill()
        {
            GetStatCoefficients += RecalculateStats;

            base.SetupSkill();
        }

        private void RecalculateStats(CharacterBody sender, StatHookEventArgs args)
        {
            if (!sender || !superchargeSkill)
            {
                return;
            }

            if (sender == PlayerCharacterMasterController.instances[0].master.GetBody())
            {
                args.utilityCooldownMultAdd -= 1 - (20f / (superchargeSkill.skillLevel + 20f));
                args.specialCooldownMultAdd -= originalRechargeRate;
            }
        }
    }

    [SkillLevelModifier("RailgunnerBodyChargeSnipeCryo", typeof(FireSnipeCryo), typeof(ExpiredCryo), typeof(ChargedCryo))]
    internal class RailgunnerCryochargeSkillModifier : BaseSkillModifier
    {
        private static BuffDef FrostfireBuff;
        private static DotController.DotDef FrostfireDot;
        private static DotController.DotIndex FrostfireIndex;
        private static SkillUpgrade CryochargeSkill;
        private static bool bCryoActive = false;
        private static float originalRechargeRate = 0;

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is ChargedCryo)
            {
                this.OnChargedCryo((ChargedCryo)skillState, level);
            }
        }

        public override void OnSkillExit(BaseState skillState, int level)
        {
            base.OnSkillExit(skillState, level);
            if (skillState is FireSnipeCryo)
            {
                this.OnFireSnipeCryo((FireSnipeCryo)skillState, level);
            }
            else if (skillState is ExpiredCryo)
            {
                this.OnExpiredCryo((ExpiredCryo)skillState, level);
            }
        }

        public void OnFireSnipeCryo(FireSnipeCryo skillstate, int level)
        {
            bCryoActive = false;
        }

        public void OnExpiredCryo(ExpiredCryo skillstate, int level)
        {
            bCryoActive = false;
        }

        public void OnChargedCryo(ChargedCryo skillstate, int level)
        {
            bCryoActive = true;
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (!CryochargeSkill)
            {
                CryochargeSkill = registeredSkill;
            }
            originalRechargeRate = 1 - (10f / (level + 10f));
            Logger.Debug(originalRechargeRate);
        }

        public void RegisterFrostfireBuff()
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();

            buffDef.buffColor = new Color(0.5f, 0.5f, 1f);
            buffDef.canStack = false;
            buffDef.eliteDef = null;
            buffDef.iconSprite = Addressables.LoadAssetAsync<BuffDef>("RoR2/DLC1/StrengthenBurn/bdStrongerBurn.asset").WaitForCompletion().iconSprite;
            buffDef.isDebuff = true;
            buffDef.name = "RailgunnerFrostfireBuff";

            FrostfireBuff = buffDef;
            ContentAddition.AddBuffDef(buffDef);

            DotController.DotDef dotDef = new DotController.DotDef();

            dotDef.associatedBuff = FrostfireBuff;
            dotDef.damageCoefficient = 2f;
            dotDef.damageColorIndex = DamageColorIndex.Item;
            dotDef.terminalTimedBuff = FrostfireBuff;
            dotDef.interval = 0.5f;

            FrostfireDot = dotDef;
            FrostfireIndex = DotAPI.RegisterDotDef(dotDef);
        }

        public static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
        {
            if (bCryoActive && di.inflictor && di.inflictor.TryGetComponent(out CharacterBody body) && body == CryochargeSkill.targetGenericSkill.characterBody)
            {
                if (CryochargeSkill.skillLevel > 0)
                {
                    self.body.AddTimedBuff(FrostfireBuff, 2f * CryochargeSkill.skillLevel);
                    int ignitionTanks = body.inventory.GetItemCount(DLC1Content.Items.StrengthenBurn);
                    DotController.InflictDot(self.gameObject, di.attacker, FrostfireIndex, 2f * CryochargeSkill.skillLevel, (1 + 3 * ignitionTanks), 1);
                }
            }

            orig.Invoke(self, di);
        }

        public override void SetupSkill()
        {
            RegisterFrostfireBuff();

            On.RoR2.HealthComponent.TakeDamage += new On.RoR2.HealthComponent.hook_TakeDamage(HealthComponent_TakeDamage);

            base.SetupSkill();

            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPIOnGetStatCoefficients;
        }

        private void RecalculateStatsAPIOnGetStatCoefficients(CharacterBody sender, StatHookEventArgs args)
        {
            if (sender == PlayerCharacterMasterController.instances[0].master.GetBody())
            {
                args.specialCooldownMultAdd -= originalRechargeRate;
            }
        }
    }
}
