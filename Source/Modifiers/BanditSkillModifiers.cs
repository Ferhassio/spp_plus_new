using EntityStates.Bandit2;
using EntityStates.Bandit2.Weapon;
using R2API;
using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static R2API.RecalculateStatsAPI;

namespace SkillsPlusPlus.Modifiers
{
    [SkillLevelModifier("FireShotgun2", typeof(FireShotgun2))]
    internal class BanditFireShotgunSkillModifier : SimpleSkillModifier<FireShotgun2>
    {
        public override void OnSkillEnter(FireShotgun2 skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            skillState.bulletCount += level;
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.05f, level);

            skillState.minFixedSpreadYaw += level * 0.5f;
            skillState.maxFixedSpreadYaw += level;
        }
    }

    [SkillLevelModifier("Bandit2Blast", typeof(Bandit2FireRifle))]
    internal class BanditFireRifleSkillModifier : SimpleSkillModifier<Bandit2FireRifle>
    {
        private float baseBloom = 0;
        public override void OnSkillEnter(Bandit2FireRifle skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (Mathf.Abs(baseBloom) < 0.01f)
            {
                baseBloom = skillState.spreadBloomValue;
            }

            if (level > 0)
            {
                skillState.procCoefficient = AdditiveScaling(skillState.procCoefficient, 0.1f, level);
                skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.2f, level);

                skillState.spreadBloomValue = baseBloom * (4 / ((float)level + 4));
            }
        }
    }

    [SkillLevelModifier("SlashBlade", typeof(SlashBlade))]
    internal class BanditBladeSkillModifier : SimpleSkillModifier<SlashBlade>
    {
        private Vector3 originalHitboxScale = Vector3.zero;
        public override void OnSkillEnter(SlashBlade slash, int level)
        {
            //Visual Scaling
            slash.swingEffectPrefab.transform.localScale = new Vector3(1 + level, 1 + level, 1 + level);

            base.OnSkillEnter(slash, level);

            slash.damageCoefficient = MultScaling(slash.damageCoefficient, 0.2f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            //Hitbox Scaling
            HitBoxGroup hitboxGroup = characterBody.modelLocator.modelTransform.GetComponent<HitBoxGroup>();

            if (hitboxGroup == null || hitboxGroup.groupName != "SlashBlade")
            {
                Debug.LogWarning("didn't get Bandit's slashHitbox. probably got changed?. aborting");

                return;
            }

            Transform hitboxTransform = hitboxGroup.hitBoxes[0].transform;

            if (originalHitboxScale == Vector3.zero)
            {
                originalHitboxScale = hitboxTransform.localScale;
            }
            hitboxTransform.localScale = new Vector3(MultScaling(originalHitboxScale.x, 0.2f, level), MultScaling(originalHitboxScale.y, 0.2f, level), MultScaling(originalHitboxScale.z, 0.3f, level));
        }
    }

    [SkillLevelModifier("Bandit2SerratedShivs", typeof(Bandit2FireShiv))]
    internal class BanditFireShivSkillModifier : SimpleSkillModifier<Bandit2FireShiv>
    {
        public override void OnSkillEnter(Bandit2FireShiv skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            skillState.maxShivCount = 1 + (level / 2);
            skillState.baseDuration = skillState.baseDuration + (skillState.baseDuration * (level / 2) * 0.5f);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.1f, level);
        }
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }
    }

    [SkillLevelModifier("ThrowSmokebomb", typeof(ThrowSmokebomb))]
    internal class BanditSkillThrowSmokebombModifier : SimpleSkillModifier<ThrowSmokebomb>
    {
        private static float baseRadius = 0;
        private static float baseDamage = 0;
        private static BuffDef BanditSpeedBuff;

        public static void RegisterBanditSpeedBuff()
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();

            buffDef.buffColor = new Color(0.6f, 0.11f, 0.59f);
            buffDef.buffIndex = (BuffIndex)63;
            buffDef.canStack = true;
            buffDef.eliteDef = null;
            buffDef.iconSprite = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Bandit2/bdCloakSpeed.asset").WaitForCompletion().iconSprite;
            buffDef.isDebuff = false;
            buffDef.name = "BanditSpeedBuff";

            BanditSpeedBuff = buffDef;
            ContentAddition.AddBuffDef(buffDef);
        }

        public override void OnSkillEnter(ThrowSmokebomb skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            for (int i = 0; i < level; i++)
            {
                skillState.outer.commonComponents.characterBody.AddTimedBuff(BanditSpeedBuff, StealthMode.duration);
            }

        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            if (Mathf.Abs(baseRadius - 0) < 0.1f)
            {
                baseRadius = StealthMode.blastAttackRadius;
                baseDamage = StealthMode.blastAttackDamageCoefficient;
            }

            StealthMode.blastAttackRadius = MultScaling(baseRadius, 0.15f, level);
            StealthMode.blastAttackDamageCoefficient = MultScaling(baseDamage, 0.2f, level);
        }

        public static void RecalculateStats(CharacterBody sender, StatHookEventArgs args)
        {
            if (!sender)
            {
                return;
            }

            if (sender.HasBuff(BanditSpeedBuff))
            {
                args.baseMoveSpeedAdd += sender.GetBuffCount(BanditSpeedBuff);
            }
        }

        public override void SetupSkill()
        {
            base.SetupSkill();

            RegisterBanditSpeedBuff();
            GetStatCoefficients += RecalculateStats;

        }
    }


    [SkillLevelModifier("ResetRevolver", typeof(FireSidearmResetRevolver))]
    internal class BanditSkillResetRevolverModifier : SimpleSkillModifier<FireSidearmResetRevolver>
    {
        private static SkillUpgrade resetSkill;

        public override void OnSkillEnter(FireSidearmResetRevolver skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
        }
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (!resetSkill)
            {
                resetSkill = registeredSkill;
            }
        }

        public static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
        {
            if (resetSkill != null)
            {
                if (di.attacker != null && self != null)
                {
                    CharacterBody body = self.GetComponent<CharacterBody>();
                    if (body != null)
                    {

                        if ((di.damageType & RoR2.DamageType.BonusToLowHealth) == RoR2.DamageType.BonusToLowHealth && (di.damageType & RoR2.DamageType.ResetCooldownsOnKill) == RoR2.DamageType.ResetCooldownsOnKill)
                        {
                            di.damage *= Mathf.Lerp(1.0f + resetSkill.skillLevel * 0.3f, 1.0f + resetSkill.skillLevel * 0.1f, self.combinedHealthFraction);
                        }
                    }
                }
            }

            orig.Invoke(self, di);
        }

        public override void SetupSkill()
        {
            base.SetupSkill();

            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;

        }
    }


    [SkillLevelModifier("SkullRevolver", typeof(FireSidearmSkullRevolver))]
    internal class BanditSkillSkullRevolverModifier : SimpleSkillModifier<FireSidearmSkullRevolver>
    {
        private static SkillUpgrade revolverSkill;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (!revolverSkill)
            {
                revolverSkill = registeredSkill;
            }
        }

        public override void OnSkillEnter(FireSidearmSkullRevolver skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            //Attempt to get the skill if it's still invalid.
            //FindSkillUpgrade(skillState.outer.commonComponents.characterBody, "Bandit2.ResetRevolver");
        }

        public static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
        {
            if (revolverSkill != null)
            {
                if (di != null && self != null)
                {
                    if ((di.damageType & RoR2.DamageType.GiveSkullOnKill) == RoR2.DamageType.GiveSkullOnKill)
                    {
                        float remainingHealth = (self.combinedHealth - di.damage);
                        if (remainingHealth / self.fullCombinedHealth < revolverSkill.skillLevel * 0.01f && remainingHealth > 0)
                        {
                            di.damage += remainingHealth;
                            di.damageType |= DamageType.BypassArmor;
                        }
                    }
                }
            }
            orig.Invoke(self, di);
        }

        public override void SetupSkill()
        {
            base.SetupSkill();

            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;

        }
    }


}
