using RoR2;
using RoR2.Skills;
using EntityStates.Treebot;
using EntityStates.Treebot.Weapon;
using EntityStates.Treebot.TreebotFlower;
using RoR2.Projectile;
using UnityEngine;

namespace SkillsPlusPlus.Modifiers
{

    [SkillLevelModifier("TreebotBodyFireSyringe", typeof(FireSyringe))]
    internal class TreebotSyringeSkillModifier : SimpleSkillModifier<FireSyringe>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            FireSyringe.projectileCount = AdditiveScaling(3, 1, level);
            FireSyringe.totalYawSpread = AdditiveScaling(1, 0.1f, level);
            FireSyringe.damageCoefficient = MultScaling(0.8f, 0.1f, level);
        }

    }

    [SkillLevelModifier("TreebotBodyAimMortarRain", typeof(AimMortarRain))]
    internal class TreebotMortarRainSkillModifier : SimpleSkillModifier<AimMortarRain>
    {

        public override void OnSkillEnter(AimMortarRain skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.1f, level);
            var radius = MultScaling(7, 0.2f, level);
            if (skillState.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion impactExplosion))
            {
                impactExplosion.blastRadius = radius;
            }
            if (skillState.projectilePrefab.TryGetComponent(out ProjectileDotZone damageOverTimeZone))
            {
                damageOverTimeZone.lifetime = MultScaling(3, 0.1f, level);
            }
            if (skillState.projectilePrefab.TryGetComponent(out HitBoxGroup hitBoxGroup))
            {
                foreach (var hitbox in hitBoxGroup.hitBoxes)
                {
                    hitbox.transform.localScale = Vector3.one * 1.4f * radius;
                }
            }
            var expanderTransform = skillState.projectilePrefab.transform.Find("Expander");
            if (expanderTransform != null)
            {
                expanderTransform.transform.localScale = Vector3.one * radius;
            }
            var activeFXTransform = skillState.projectilePrefab.transform.Find("ActiveFX");
            if (activeFXTransform != null)
            {
                var bulletCenterTransform = activeFXTransform.Find("BulletCenter");
                if (bulletCenterTransform != null)
                {
                    bulletCenterTransform.transform.localScale = Vector3.one * radius;
                }
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }


    }

    [SkillLevelModifier("TreebotBodyAimMortar2", typeof(FireMortar2))]
    internal class TreebotMortar2SkillModifier : SimpleSkillModifier<AimMortar2>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            var radius = MultScaling(5, 0.2f, level);
            if (FireMortar2.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion))
            {
                projectileImpactExplosion.blastRadius = radius;
                projectileImpactExplosion.blastDamageCoefficient = MultScaling(1, 0.2f, level);
            }
            var expanderTransform = FireMortar2.projectilePrefab.transform.Find("Expander");
            if (expanderTransform != null)
            {
                expanderTransform.transform.localScale = Vector3.one * radius;
            }
        }

    }

    [SkillLevelModifier("TreebotBodySonicBoom", typeof(FireSonicBoom))]
    internal class TreebotSonicBoomSkillModifier : SimpleSkillModifier<FireSonicBoom>
    {

        public override void OnSkillEnter(FireSonicBoom skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            skillState.maxDistance = MultScaling(skillState.maxDistance, 0.30f, level);
            skillState.idealDistanceToPlaceTargets = MultScaling(skillState.idealDistanceToPlaceTargets, 0.30f, level);
            skillState.fieldOfView = MultScaling(skillState.fieldOfView, 0.20f, level);
            skillState.slowDuration = MultScaling(skillState.slowDuration, 0.20f, level);
        }

    }

    [SkillLevelModifier("TreebotBodyPlantSonicBoom", typeof(FirePlantSonicBoom))]
    internal class TreebotPlantSonicBoomSkillModifier : SimpleSkillModifier<FirePlantSonicBoom>
    {
        public override void OnSkillEnter(FirePlantSonicBoom skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            skillState.maxDistance = MultScaling(skillState.maxDistance, 0.20f, level);
            skillState.idealDistanceToPlaceTargets = MultScaling(skillState.idealDistanceToPlaceTargets, 0.20f, level);
            skillState.fieldOfView = MultScaling(skillState.fieldOfView, 0.20f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            FirePlantSonicBoom.damageCoefficient = MultScaling(5.5f, 0.20f, level);
        }
    }

    [SkillLevelModifier(new string[] { "TreebotBodyFireFlower2", "Chaotic Growth" }, typeof(TreebotFlower2Projectile))]
    internal class TreebotFlowerSkillModifier : SimpleSkillModifier<TreebotFlower2Projectile>
    {

        public override void OnSkillEnter(TreebotFlower2Projectile skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            var areaIndicatorTransform = skillState.outer.commonComponents.modelLocator?.modelBaseTransform?.Find("AreaIndicator");
            if (areaIndicatorTransform != null)
            {
                areaIndicatorTransform.localScale = Vector3.one * TreebotFlower2Projectile.radius;
            }
            if (skillState.outer.TryGetComponent(out ProjectileDamage projectileDamage))
            {
                projectileDamage.damage = MultScaling(2, 0.2f, level);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            TreebotFlower2Projectile.radius = MultScaling(15, 0.20f, level);
            TreebotFlower2Projectile.healPulseCount = (int)MultScaling(16, 0.1f, level);
        }

    }

    [SkillLevelModifier("TreebotBodyFireFruitSeed", typeof(TreebotFireFruitSeed)/*, typeof(TreebotPrepFruitSeed)*/)]
    internal class TreebotHarvestSkillModifier : SimpleSkillModifier<TreebotFireFruitSeed>
    {
        private static float baseBlastRadius = 0f;
        private static float baseDamageCoefficient = 0f;
        private static SkillUpgrade seedSkill;
        public override void OnSkillEnter(TreebotFireFruitSeed skillState, int level)
        {
            if (skillState is TreebotFireFruitSeed)
            {
                TreebotFireFruitSeed skill = (TreebotFireFruitSeed)skillState;

                if (Mathf.Abs(baseDamageCoefficient) < 0.01f)
                {
                    baseDamageCoefficient = skill.damageCoefficient;

                    if (skill.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion explosionBase))
                    {
                        baseBlastRadius = explosionBase.blastRadius;
                    }
                }

                skill.damageCoefficient = MultScaling(baseDamageCoefficient, 0.20f, level);

                if (((TreebotFireFruitSeed)skillState).projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion))
                {
                    projectileImpactExplosion.blastRadius = MultScaling(baseBlastRadius, 0.10f, level);
                }
            }

            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (!seedSkill)
            {
                seedSkill = registeredSkill;
            }
        }

        public static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
        {
            if (seedSkill != null)
            {
                if (di.attacker != null && self != null)
                {
                    CharacterBody body = self.GetComponent<CharacterBody>();
                    if (body != null)
                    {
                        if (body.HasBuff(RoR2Content.Buffs.Fruiting))
                        {
                            di.attacker.GetComponent<CharacterBody>().healthComponent.Heal(seedSkill.skillLevel * 0.01f * di.damage, default(ProcChainMask), true);
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

