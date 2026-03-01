using RoR2;
using RoR2.Skills;
using RoR2.Projectile;

using EntityStates.Croco;
using UnityEngine;
namespace SkillsPlusPlus.Modifiers
{

    [SkillLevelModifier("CrocoSlash", typeof(Slash))]
    internal class CrocoSlashSkillModifier : SimpleSkillModifier<Slash>
    {

        public override void OnSkillEnter(Slash slash, int level)
        {
            base.OnSkillEnter(slash, level);
            Logger.Debug("hitPauseDuration: {0}, damageCoefficient: {1}", slash.hitPauseDuration, slash.damageCoefficient);
            slash.hitPauseDuration = MultScaling(slash.hitPauseDuration, -0.1f, level);
            slash.baseDuration = MultScaling(slash.baseDuration, -0.15f, level);
            slash.damageCoefficient = MultScaling(slash.damageCoefficient, 0.20f, level);
            Logger.Debug("hitPauseDuration: {0}, damageCoefficient: {1}", slash.hitPauseDuration, slash.damageCoefficient);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("baseDurationBeforeInterruptable: {0}", Slash.baseDurationBeforeInterruptable);
            Logger.Debug("comboFinisherDamageCoefficient: {0}, comboFinisherBaseDurationBeforeInterruptable: {1}", Slash.comboFinisherDamageCoefficient, Slash.comboFinisherBaseDurationBeforeInterruptable);
            Slash.comboFinisherDamageCoefficient = MultScaling(4, 0.25f, level); // combined with +25% of damage bonus this is effectively 50% for the final attack
        }

    }

    [SkillLevelModifier("CrocoSpit", typeof(FireSpit))]
    internal class CrocoSpitSkillModifier : SimpleSkillModifier<FireSpit>
    {

        public override void OnSkillEnter(FireSpit fireSpit, int level)
        {
            base.OnSkillEnter(fireSpit, level);
            fireSpit.damageCoefficient = MultScaling(fireSpit.damageCoefficient, 0.20f, level);
            fireSpit.force = MultScaling(fireSpit.force, 0.3f, level);
            if (fireSpit.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion))
            {
                projectileImpactExplosion.blastRadius = MultScaling(3, 0.3f, level);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            skillDef.baseMaxStock = (int)AdditiveScaling(1, 0.5f, level);
        }

    }

    [SkillLevelModifier("CrocoBite", typeof(Bite))]
    internal class CrocoBiteSkillModifier : SimpleSkillModifier<Bite>
    {

        public override void OnSkillEnter(Bite bite, int level)
        {
            base.OnSkillEnter(bite, level);

            bite.damageCoefficient = MultScaling(bite.damageCoefficient, 0.20f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            skillDef.baseMaxStock = AdditiveScaling(1, 1, level);
        }
    }

    [SkillLevelModifier("CrocoLeap", typeof(Leap))]
    internal class CrocoLeapSkillModifier : SimpleSkillModifier<Leap>
    {

        public override void OnSkillEnter(Leap leap, int level)
        {
            base.OnSkillEnter(leap, level);
            leap.blastDamageCoefficient = MultScaling(leap.blastDamageCoefficient, 0.20f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            skillDef.baseMaxStock = (int)AdditiveScaling(2, 0.5f, level);
            if (Leap.projectilePrefab.TryGetComponent(out ProjectileDotZone dotZone))
            {
                dotZone.damageCoefficient = MultScaling(0.25f, 0.30f, level);
            }

            // the FX game object contains the hitbox so the effective area can be changed with the following
            Transform fxTransform = Leap.projectilePrefab.transform.Find("FX");
            if (fxTransform)
            {
                fxTransform.localScale = Vector3.one * MultScaling(8.0f, 0.20f, level);
            }
        }

    }

    [SkillLevelModifier("CrocoChainableLeap", typeof(ChainableLeap))]
    internal class CrocoChainableLeapSkillModifier : SimpleSkillModifier<ChainableLeap>
    {

        public override void OnSkillEnter(ChainableLeap skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            skillState.blastDamageCoefficient = MultScaling(skillState.blastDamageCoefficient, 0.25f, level);
            skillState.blastBonusForce = skillState.blastBonusForce * MultScaling(1, 0.25f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            // 0.5s extra cooldown per level
            ChainableLeap.refundPerHit = MultScaling(2, 0.15f, level);
        }

    }

    [SkillLevelModifier(new string[] { "CrocoDisease", "Plague" }, typeof(FireDiseaseProjectile))]
    internal class CrocoDiseaseSkillModifier : SimpleSkillModifier<FireDiseaseProjectile>
    {

        public override void OnSkillEnter(FireDiseaseProjectile fireDisease, int level)
        {
            base.OnSkillEnter(fireDisease, level);
            fireDisease.damageCoefficient = MultScaling(fireDisease.damageCoefficient, 0.20f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Disease.maxBounces = AdditiveScaling(20, 5, level);
            Disease.bounceRange = MultScaling(25, 0.20f, level);
        }

    }
}
