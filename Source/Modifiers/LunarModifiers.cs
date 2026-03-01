using System;
using System.Collections.Generic;
using EntityStates;
using EntityStates.GlobalSkills.LunarDetonator;
using EntityStates.GlobalSkills.LunarNeedle;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using static R2API.RecalculateStatsAPI;

namespace SkillsPlusPlus.Modifiers
{
    internal class LunarModifiers
    {

#pragma warning disable CS0649
        private static List<String> HereticSkillsWarned = new List<string>();
#pragma warning restore CS0649
        public static List<String> HereticSupportedPassiveUpgrades = new List<String> { "LunarPrimaryReplacement", "LunarSecondaryReplacement", "LunarUtilityReplacement", "LunarDetonatorSpecialReplacement" };

        public static void RecalculateStats_GetLunarStats(CharacterBody sender, StatHookEventArgs args)
        {
            if (!sender)
            {
                return;
            }

            if (sender.baseNameToken == "HERETIC_BODY_NAME") /*Heretic*/
            {
                SkillUpgrade[] upgrades = sender.GetComponents<SkillUpgrade>();

                foreach (SkillUpgrade upgrade in upgrades)
                {
                    switch (upgrade.targetBaseSkillName)
                    {
                        case "LunarPrimaryReplacement":
                            args.baseDamageAdd += (upgrade.skillLevel * 0.25f);
                            break;
                        case "LunarSecondaryReplacement":
                            args.armorAdd += (upgrade.skillLevel * 3);
                            break;
                        case "LunarUtilityReplacement":
                            args.healthMultAdd += (upgrade.skillLevel * 0.03f);
                            break;
                        case "LunarDetonatorSpecialReplacement":
                            args.baseAttackSpeedAdd += (upgrade.skillLevel * 0.25f);
                            break;
                        default:
                            if (!HereticSupportedPassiveUpgrades.Contains(upgrade.targetGenericSkill.name) && !HereticSkillsWarned.Contains(upgrade.targetGenericSkill.name))
                            {
                                Logger.Warn("Heretic Skill {0} found but no bonuses may have been applied. If this skill is supported by SkillsPlusPlus, please add it to LunarModifiers.HereticSupportedPassiveUpgrades!", upgrade.targetGenericSkill.name);
                                HereticSkillsWarned.Add(upgrade.targetGenericSkill.name);
                            }
                            break;
                    }
                }
            }
        }

        [SkillLevelModifier(new string[] { "LunarPrimaryReplacement", "HungeringGaze" }, typeof(FireLunarNeedle))]
        internal class VisionsOfHeresySkillModifier : SimpleSkillModifier<FireLunarNeedle>
        {

            public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
            {
                base.OnSkillLeveledUp(level, characterBody, skillDef);

                skillDef.baseMaxStock = 12 + (2 * level);
                FireLunarNeedle.damageCoefficient = MultScaling(0.05f, 0.2f, level);
            }

            public override void SetupSkill()
            {
                base.SetupSkill();
            }
        }

        [SkillLevelModifier(new string[] { "LunarSecondaryReplacement", "SlicingMaelstrom" }, typeof(ThrowLunarSecondary), typeof(ChargeLunarSecondary))]
        internal class HooksOfHeresySkillModifier : BaseSkillModifier
        {
            private static float baseMaelstromScale = 0f;
            private static float baseBlastRadius = 0f;

            public override void OnSkillEnter(BaseState skillState, int level)
            {
                base.OnSkillEnter(skillState, level);

                if (skillState is ThrowLunarSecondary && registeredSkill != null)
                {
                    ThrowLunarSecondary throwSkillState = ((ThrowLunarSecondary)skillState);
                    if (throwSkillState.projectilePrefab.TryGetComponent(out ProjectileDotZone projectileDot))
                    {

                        projectileDot.resetFrequency = 5 + level;
                        projectileDot.fireFrequency = 20 + level;

                        HitBoxGroup group = projectileDot.gameObject.GetComponent<HitBoxGroup>();

                        foreach (HitBox hitbox in group.hitBoxes)
                        {
                            if (Mathf.Abs(baseMaelstromScale) < 0.1f)
                            {
                                baseMaelstromScale = hitbox.transform.localScale.x;
                            }

                            hitbox.transform.localScale = Vector3.one * MultScaling(baseMaelstromScale, 0.15f, level);
                        }
                    }

                    if (throwSkillState.projectilePrefab.TryGetComponent(out ProjectileExplosion projectileExplosion))
                    {
                        if (Mathf.Abs(baseBlastRadius) < 0.1f)
                        {
                            baseBlastRadius = projectileExplosion.blastRadius;
                        }

                        projectileExplosion.blastRadius = MultScaling(baseBlastRadius, 0.10f, level);
                    }
                }
            }

            public override void SetupSkill()
            {
                base.SetupSkill();
            }
        }

        [SkillLevelModifier(new string[] { "LunarUtilityReplacement", "Shadowfade" }, typeof(GhostUtilitySkillState))]
        internal class StridesOfHeresySkillModifier : SimpleSkillModifier<GhostUtilitySkillState>
        {

            public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
            {
                base.OnSkillLeveledUp(level, characterBody, skillDef);

                GhostUtilitySkillState.moveSpeedCoefficient = MultScaling(1.3f, 0.1f, level); // +10% 
                GhostUtilitySkillState.healFrequency = MultScaling(5, 0.15f, level); // +15%
            }

            public override void SetupSkill()
            {
                base.SetupSkill();
            }
        }

        [SkillLevelModifier(new string[] { "LunarDetonatorSpecialReplacement", "Ruin" }, typeof(LunarDetonatorSkill), typeof(Detonate))]
        internal class HeartOfHeresySkillModifier : BaseSkillModifier
        {
            private static float baseDamageCoefficient = 0f;
            private static SkillUpgrade heartSkill;

            public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
            {
                base.OnSkillLeveledUp(level, characterBody, skillDef);

                if (!heartSkill)
                {
                    heartSkill = registeredSkill;
                    baseDamageCoefficient = Detonate.baseDamageCoefficient;
                }

                Detonate.baseDamageCoefficient = MultScaling(baseDamageCoefficient, 0.2f, level);
            }

            public override void OnSkillEnter(BaseState skillState, int level)
            {
                base.OnSkillEnter(skillState, level);
            }

            public static void LunarDetonatorPassiveAttachment_OnDamageDealt(On.RoR2.LunarDetonatorPassiveAttachment.DamageListener.orig_OnDamageDealtServer orig, MonoBehaviour self, DamageReport damageReport)
            {
                orig.Invoke((LunarDetonatorPassiveAttachment.DamageListener)self, damageReport);

                float rollValue = damageReport.damageInfo.procCoefficient * (((heartSkill ? heartSkill.skillLevel : 0) * 0.20f));

                int assuredStacks = (int)Mathf.Floor(rollValue);
                float remainder = rollValue - Mathf.Floor(rollValue);

                if (damageReport.victim.alive)
                {
                    for (int i = 0; i < assuredStacks; i++)
                    {
                        damageReport.victimBody.AddTimedBuff(RoR2Content.Buffs.LunarDetonationCharge, 10f);
                    }

                    if (RoR2.Util.CheckRoll(100f * remainder, damageReport.attackerMaster))  //Don't apply the odds if we are still below the additional stacks numbers.
                    {
                        damageReport.victimBody.AddTimedBuff(RoR2Content.Buffs.LunarDetonationCharge, 10f);
                    }
                }
            }

            public override void SetupSkill()
            {
                base.SetupSkill();

                On.RoR2.LunarDetonatorPassiveAttachment.DamageListener.OnDamageDealtServer += LunarDetonatorPassiveAttachment_OnDamageDealt;
            }
        }
    }
}