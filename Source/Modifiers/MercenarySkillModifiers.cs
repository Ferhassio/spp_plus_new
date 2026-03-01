using RoR2;
using RoR2.Skills;

using EntityStates;
using EntityStates.Merc;
using EntityStates.Merc.Weapon;
using UnityEngine;
using RoR2.Projectile;

namespace SkillsPlusPlus.Modifiers
{

    [SkillLevelModifier("MercGroundLight", typeof(GroundLight))]
    internal class GroundLightSkillModifier : SimpleSkillModifier<GroundLight>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            GroundLight.forceMagnitude = MultScaling(600, 0.20f, level);
            GroundLight.selfForceMagnitude = MultScaling(600, 0.20f, level);
            GroundLight.comboDamageCoefficient = MultScaling(1.3f, 0.2f, level);
            GroundLight.baseComboAttackDuration = MultScaling(0.6f, -0.15f, level);
            GroundLight.finisherDamageCoefficient = MultScaling(1.3f, 0.2f, level);
            GroundLight.baseFinisherAttackDuration = MultScaling(1, -0.15f, level);
        }

    }

    [SkillLevelModifier("MercGroundLight2", typeof(GroundLight2))]
    internal class GroundLight2SkillModifier : SimpleSkillModifier<GroundLight2>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            // all swings in the combo fall under the same coefficients as of RoR2 1.0
            GroundLight2.baseDurationBeforeInterruptable = MultScaling(0.45f, -0.15f, level);
            GroundLight2.comboFinisherBaseDurationBeforeInterruptable = MultScaling(0.85f, -0.15f, level);
            GroundLight2.comboFinisherBaseDuration = MultScaling(1, -0.15f, level);
            GroundLight2.comboFinisherDamageCoefficient = MultScaling(1.3f, 0.2f, level);
        }

        public override void OnSkillEnter(GroundLight2 skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("damageCoefficient: {0}, baseDuration: {1}", skillState.damageCoefficient, skillState.baseDuration);
            skillState.baseDuration = MultScaling(0.6f, -.15f, level);
            skillState.damageCoefficient = MultScaling(1.3f, 0.2f, level);
        }

    }

    [SkillLevelModifier("MercBodyWhirlwind", typeof(WhirlwindEntry), typeof(WhirlwindGround), typeof(WhirlwindAir))]
    internal class WhirlwindSkillModifier : BaseSkillModifier
    {

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is WhirlwindEntry)
            {
                WhirlwindEntry whirlwindEntry = (WhirlwindEntry)skillState;
            }
            else if (skillState is WhirlwindBase)
            {
                WhirlwindBase whirlwindBase = (WhirlwindBase)skillState;
                whirlwindBase.baseDamageCoefficient = MultScaling(whirlwindBase.baseDamageCoefficient, 0.25f, level);
                whirlwindBase.selfForceMagnitude = MultScaling(whirlwindBase.selfForceMagnitude, 0.25f, level);
                Transform modelTransform = whirlwindBase.outer?.commonComponents.modelLocator?.modelTransform;
                if (modelTransform)
                {
                    // Merc's model has a WhirlwindGround and WhirlwindAir gameobjects that contain the hit boxes as children for the whirlwind attacks
                    // finding the parenting gameobjects turns out to be a good way to increase the size of the hitboxes.
                    Transform whirlwindHitboxTransform = modelTransform.Find(whirlwindBase.hitboxString);
                    if (whirlwindHitboxTransform)
                    {
                        whirlwindHitboxTransform.localScale = Vector3.one * MultScaling(1, 0.25f, level);
                    }
                }
            }
        }

    }

    [SkillLevelModifier("MercBodyUppercut", typeof(Uppercut))]
    internal class UppercutSkillModifier : SimpleSkillModifier<Uppercut>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Uppercut.baseDamageCoefficient = MultScaling(5.5f, 0.25f, level);
            skillDef.baseMaxStock = (int)AdditiveScaling(1, 0.5f, level);
        }

    }

    [SkillLevelModifier("MercBodyAssaulter", typeof(Assaulter))]
    internal class AssaultSkillModifier : SimpleSkillModifier<Assaulter>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            Assaulter.damageCoefficient = MultScaling(3, 0.2f, level);

            if (skillDef is MercDashSkillDef)
            {
                MercDashSkillDef mercDashSkillDef = (MercDashSkillDef)skillDef;
                mercDashSkillDef.timeoutDuration = AdditiveScaling(3, 0.5f, level);
            }

        }
    }

    [SkillLevelModifier("MercBodyFocusedAssault", typeof(FocusedAssaultDash))]
    internal class Assault2SkillModifier : SimpleSkillModifier<FocusedAssaultDash>
    {
        private Transform assaultHitbox;
        private Vector3 originalHitboxScale;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (!assaultHitbox)
            {
                HitBoxGroup[] hitboxGroups = characterBody.modelLocator.modelTransform.GetComponents<HitBoxGroup>();

                foreach (HitBoxGroup group in hitboxGroups)
                {
                    if (group != null && group.groupName == "Assaulter")
                    {
                        assaultHitbox = group.hitBoxes[0].transform;

                        if (originalHitboxScale == Vector3.zero)
                        {
                            originalHitboxScale = assaultHitbox.localScale;
                        }
                    }
                }
            }
            if (!assaultHitbox)
            {
                Debug.LogWarning("didn't get Merc's AssaulterHitbox. probably got changed?. aborting");
                return;
            }
            else
            {
                assaultHitbox.localScale = new Vector3(MultScaling(originalHitboxScale.x, 0.2f, level), MultScaling(originalHitboxScale.y, 0.2f, level), MultScaling(originalHitboxScale.z, 0.2f, level));
            }


        }
        public override void OnSkillEnter(FocusedAssaultDash skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            skillState.delayedProcCoefficient = AdditiveScaling(1, 0.15f, level);
        }
    }

    // both Mercenary special skills have the same skill name
    [SkillLevelModifier(new string[] { "MercBodyEvis", "MercBodyEvisProjectile", "Gale-Force" }, typeof(Evis), typeof(EvisDash), typeof(ThrowEvisProjectile))]
    internal class EviscerateSkillModifier : BaseSkillModifier
    {

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is Evis)
            {
                this.OnEvisEnter((Evis)skillState, level);
            }
            else if (skillState is ThrowEvisProjectile)
            {
                this.OnThrowEvisProjectileEnter((ThrowEvisProjectile)skillState, level);
            }
        }

        private void OnEvisEnter(Evis evis, int level)
        {
            // evis.
        }

        private void OnThrowEvisProjectileEnter(ThrowEvisProjectile throwEvisProjectile, int level)
        {
            if (throwEvisProjectile.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion))
            {
                if (projectileImpactExplosion.childrenProjectilePrefab.TryGetComponent(out ProjectileOverlapAttack projectileOverlapAttack))
                {
                    float fireFrequency = MultScaling(8f, 0.20f, level);
                    projectileOverlapAttack.damageCoefficient = MultScaling(1, 0.20f, level);
                    projectileOverlapAttack.fireFrequency = fireFrequency;
                    projectileOverlapAttack.resetInterval = 1f / fireFrequency;
                }
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            if (skillDef.activationState.stateType == typeof(EvisDash))
            {
                Evis.maxRadius = MultScaling(16, 0.3f, level);
                Evis.damageFrequency = MultScaling(7, 0.15f, level);
                Evis.procCoefficient = MultScaling(1, 0.1f, level);
            }
            else if (skillDef.activationState.stateType == typeof(ThrowEvisProjectile))
            {

            }
        }

    }
}
