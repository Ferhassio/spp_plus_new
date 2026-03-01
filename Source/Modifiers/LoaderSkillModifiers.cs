using RoR2;
using RoR2.Projectile;
using RoR2.Skills;

using EntityStates;
using EntityStates.Loader;

using UnityEngine;
using RoR2.UI;

namespace SkillsPlusPlus.Modifiers
{

    [SkillLevelModifier("SwingFist", typeof(SwingComboFist))]
    internal class LoaderKnucklesSkillModifier : SimpleSkillModifier<SwingComboFist>
    {

        public override void OnSkillEnter(SwingComboFist skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.20f, level);
            SwingComboFist.barrierPercentagePerHit = AdditiveScaling(0.05f, 0.01f, level);
        }
    }

    [SkillLevelModifier("GroundSlam", typeof(GroundSlam), typeof(PreGroundSlam))]
    internal class LoaderThunderSlamSkillModifier : BaseSkillModifier
    {
        private static float baseUpwardVelocity = 0f;
        private static float baseVerticalAcceleration = 0f;
        private static float baseBlastRadius = 0f;
        private static float maxVerticalCap = 0f;
        private static Vector3 initialPosition;
        private static SkillUpgrade slamSkill;
        public override void OnSkillEnter(BaseState skillState, int level)
        {

            if (skillState is GroundSlam)
            {
                initialPosition = skillState.outer.commonComponents.characterBody.transform.position;

                GroundSlam.blastDamageCoefficient = MultScaling(20f, 0.05f, level);
            }

            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            if (Mathf.Abs(baseUpwardVelocity) < 0.01f)
            {
                baseUpwardVelocity = PreGroundSlam.upwardVelocity;
                baseVerticalAcceleration = GroundSlam.verticalAcceleration;
                baseBlastRadius = GroundSlam.blastRadius;
            }

            PreGroundSlam.upwardVelocity = AdditiveScaling(baseUpwardVelocity, baseUpwardVelocity * 0.25f, level);
            GroundSlam.verticalAcceleration = AdditiveScaling(baseVerticalAcceleration, baseVerticalAcceleration * 0.25f, level);
            maxVerticalCap = AdditiveScaling(0f, 10f, level);

            if (!slamSkill)
            {
                slamSkill = registeredSkill;
            }
        }

        public static void GroundSlamFixedUpdate(On.EntityStates.Loader.GroundSlam.orig_FixedUpdate orig, EntityStates.Loader.GroundSlam self)
        {
            if (self.detonateNextFrame)
            {
                float distanceTravelled = Vector3.Distance(self.outer.commonComponents.characterBody.transform.position, initialPosition);
                float distanceRatio = Mathf.Clamp(distanceTravelled / maxVerticalCap, 0f, 1f);

                GroundSlam.blastRadius = baseBlastRadius * (slamSkill ? (1 + Mathf.Lerp(0.1f, 1f, distanceRatio) * slamSkill.skillLevel) : 1);
            }
            orig(self);
        }

        public override void SetupSkill()
        {
            base.SetupSkill();

            On.EntityStates.Loader.GroundSlam.FixedUpdate += GroundSlamFixedUpdate;

        }
    }

    [SkillLevelModifier("FireHook", typeof(FireHook))]
    internal class LoaderHookSkillModifier : SimpleSkillModifier<FireHook>
    {

        public override void OnSkillEnter(FireHook skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState.projectilePrefab.TryGetComponent(out ProjectileGrappleController grappleController))
            {
                grappleController.maxTravelDistance = MultScaling(80, 0.30f, level);
            }
            CharacterBody body = skillState.outer.commonComponents.characterBody;
            GenericSkill utilitySkill = skillState.outer?.commonComponents.skillLocator?.utility;
            if (utilitySkill != null && utilitySkill.stock <= utilitySkill.maxStock)
            {
                // utilitySkill.AddOneStock();
            }
        }

        public override void OnSkillExit(FireHook skillState, int level)
        {
            base.OnSkillExit(skillState, level);

        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            if (characterBody.defaultCrosshairPrefab.TryGetComponent(out LoaderHookCrosshairController hookCrosshairController))
            {
                hookCrosshairController.range = MultScaling(80, 0.30f, level);
            }
        }

    }

    [SkillLevelModifier("FireYankHook", typeof(FireYankHook))]
    internal class LoaderYankHookSkillModifier : SimpleSkillModifier<FireYankHook>
    {
        private static float baseDamageCoeff = 0f;

        public override void OnSkillEnter(FireYankHook skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState.projectilePrefab.TryGetComponent(out ProjectileGrappleController grappleController))
            {
                grappleController.maxTravelDistance = MultScaling(80, 0.15f, level);
            }
        }
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (Mathf.Abs(baseDamageCoeff) < 0.1f)
            {
                baseDamageCoeff = FireYankHook.damageCoefficient;
            }
            if (characterBody.defaultCrosshairPrefab.TryGetComponent(out LoaderHookCrosshairController hookCrosshairController))
            {
                hookCrosshairController.range = MultScaling(80, 0.15f, level);
            }

            FireYankHook.damageCoefficient = MultScaling(baseDamageCoeff, 0.20f, level);
        }

    }

    [SkillLevelModifier(new[] { "ChargeFist", "Megaton Punch" }, typeof(ChargeFist), typeof(SwingChargedFist))]
    internal class LoaderChargeFistSkillModifier : BaseSkillModifier
    {

        public override void OnSkillEnter(BaseState baseState, int level)
        {
            base.OnSkillEnter(baseState, level);
            if (baseState is ChargeFist)
            {
                ChargeFist chargeState = (ChargeFist)baseState;
                chargeState.baseChargeDuration = MultScaling(chargeState.baseChargeDuration, 0.10f, level); // +10% max charge duration
            }
            else if (baseState is SwingChargedFist)
            {
                SwingChargedFist swingState = (SwingChargedFist)baseState;
                swingState.maxLungeSpeed = MultScaling(swingState.maxLungeSpeed, 0.15f, level); // +10% max lunge speed
                swingState.maxPunchForce = MultScaling(swingState.maxPunchForce, 0.15f, level); // +10% max pucnh force
                // swingState.maxDuration = MultScaling(swingState.maxDuration, 0.10f, level); // +10% max launge speed
            }

        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            BaseSwingChargedFist.velocityDamageCoefficient = MultScaling(0.3f, 0.20f, level);
        }

    }

    [SkillLevelModifier(new[] { "ChargeZapFist", "Thundercrash" }, typeof(ChargeZapFist), typeof(SwingZapFist))]
    internal class LoaderChargeZapFistSkillModifier : BaseSkillModifier
    {

        public override void OnSkillEnter(BaseState baseState, int level)
        {
            base.OnSkillEnter(baseState, level);
            if (baseState is ChargeZapFist)
            {
                ChargeZapFist chargeState = (ChargeZapFist)baseState;
                chargeState.baseChargeDuration = MultScaling(chargeState.baseChargeDuration, -0.15f, level); // +15% charge speed
            }
            else if (baseState is SwingZapFist)
            {
                SwingZapFist swingState = (SwingZapFist)baseState;
                swingState.damageCoefficient = MultScaling(swingState.damageCoefficient, 0.15f, level);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug(SwingZapFist.selfKnockback);
            SwingZapFist.selfKnockback = MultScaling(7000, 0.015f, level); // +10% knockback
            if (SwingZapFist.overchargeImpactEffectPrefab.TryGetComponent(out ProjectileProximityBeamController proximityBeamController))
            {
                proximityBeamController.attackRange = MultScaling(40, 0.20f, level);
            }
        }

    }

    // duplicate skill name
    [SkillLevelModifier("ThrowPylon", typeof(ThrowPylon))]
    internal class LoaderThrowPylonSkillModifier : SimpleSkillModifier<ThrowPylon>
    {

        public override void SetupSkill()
        {
            base.SetupSkill();
        }

        public override void OnSkillEnter(ThrowPylon skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("baseDuration: {0}, damageCoefficient: {1}", ThrowPylon.baseDuration, ThrowPylon.damageCoefficient);
            ThrowPylon.damageCoefficient = MultScaling(1.0f, 0.2f, level);
            if (ThrowPylon.projectilePrefab.TryGetComponent(out ProjectileProximityBeamController proximityBeamController))
            {
                //proximityBeamController.bounces = (int)AdditiveScaling(1, 0.5f, level);
                proximityBeamController.attackRange = MultScaling(15f, 0.20f, level);
                //proximityBeamController.attackFireCount = AdditiveScaling(6, 1, level);
                proximityBeamController.attackInterval = MultScaling(1.0f, -0.20f, level);
            }
        }

    }
}
