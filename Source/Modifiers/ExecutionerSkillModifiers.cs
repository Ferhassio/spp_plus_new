using System;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using UnityEngine;

namespace SkillsPlusPlus.Modifiers
{
    [SkillLevelModifier("sdExe2Pistol", "EntityStates.Executioner.FirePistol")]
    internal class ExecutionerFirePistolSkillModifier : ReflectedSkillModifier
    {
        private float originalDamageCoefficient = 0;
        private float originalBaseDuration = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Type firePistolType = EntityStateTypes.Length > 0 ? EntityStateTypes[0] : null;
            
            if (firePistolType != null)
            {
                if (Mathf.Abs(originalDamageCoefficient) < 0.01f) 
                    originalDamageCoefficient = GetStaticFloat(firePistolType, "damageCoefficient");
                if (Mathf.Abs(originalBaseDuration) < 0.01f) 
                    originalBaseDuration = GetStaticFloat(firePistolType, "baseDuration");

                SetStaticFloat(firePistolType, "damageCoefficient", MultScaling(originalDamageCoefficient, 0.15f, level));
                SetStaticFloat(firePistolType, "baseDuration", MultScaling(originalBaseDuration, -0.20f, level));
            }
        }
    }

    [SkillLevelModifier("sdExe2FireTaser", "EntityStates.Executioner2.FireTaser")]
    internal class ExecutionerFireTaserSkillModifier : ReflectedSkillModifier
    {
        private float originalBaseDuration = 0;
        private float originalDamageCoefficient = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Type fireTaserType = EntityStateTypes.Length > 0 ? EntityStateTypes[0] : null;
            
            if (fireTaserType != null)
            {
                if (Mathf.Abs(originalBaseDuration) < 0.01f) 
                    originalBaseDuration = GetStaticFloat(fireTaserType, "baseDuration");
                if (Mathf.Abs(originalDamageCoefficient) < 0.01f) 
                    originalDamageCoefficient = GetStaticFloat(fireTaserType, "damageCoefficient");

                SetStaticFloat(fireTaserType, "baseDuration", MultScaling(originalBaseDuration, -0.15f, level));
                SetStaticFloat(fireTaserType, "damageCoefficient", MultScaling(originalDamageCoefficient, 0.20f, level));
            }
        }
    }

    [SkillLevelModifier("sdExe2Dash", "EntityStates.Executioner2.Dash")]
    internal class ExecutionerDashSkillModifier : ReflectedSkillModifier
    {
        private float originalBaseDuration = 0;
        private float originalFearDuration = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Type dashType = EntityStateTypes.Length > 0 ? EntityStateTypes[0] : null;
            
            if (dashType != null)
            {
                if (Mathf.Abs(originalBaseDuration) < 0.01f) 
                    originalBaseDuration = GetStaticFloat(dashType, "baseDuration");
                if (Mathf.Abs(originalFearDuration) < 0.01f) 
                    originalFearDuration = GetStaticFloat(dashType, "fearDuration");

                SetStaticFloat(dashType, "baseDuration", MultScaling(originalBaseDuration, -0.15f, level));
                SetStaticFloat(dashType, "fearDuration", AdditiveScaling(originalFearDuration, 0.5f, level));
            }
        }
    }

    [SkillLevelModifier("sdExe2Bloodletting", "EntityStates.Executioner2.ChargeBloodletting", "EntityStates.Executioner2.Bloodletting")]
    internal class ExecutionerBloodlettingSkillModifier : ReflectedSkillModifier
    {
        private float originalChargeDuration = 0;
        private float originalDamageCoefficient = 0;
        private float originalHealFraction = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            if (EntityStateTypes.Length < 2) return;

            Type chargeType = EntityStateTypes[0];
            Type bloodlettingType = EntityStateTypes[1];

            if (chargeType != null)
            {
                if (Mathf.Abs(originalChargeDuration) < 0.01f) 
                    originalChargeDuration = GetStaticFloat(chargeType, "baseDuration");

                SetStaticFloat(chargeType, "baseDuration", MultScaling(originalChargeDuration, -0.20f, level));
            }

            if (bloodlettingType != null)
            {
                if (Mathf.Abs(originalDamageCoefficient) < 0.01f) 
                    originalDamageCoefficient = GetStaticFloat(bloodlettingType, "damageCoefficient");
                if (Mathf.Abs(originalHealFraction) < 0.01f) 
                    originalHealFraction = GetStaticFloat(bloodlettingType, "healFraction");

                SetStaticFloat(bloodlettingType, "damageCoefficient", MultScaling(originalDamageCoefficient, 0.20f, level));
                SetStaticFloat(bloodlettingType, "healFraction", AdditiveScaling(originalHealFraction, 0.05f, level));
            }
        }
    }

    [SkillLevelModifier("sdExe2ChargeIons", "EntityStates.Executioner2.ChargeGun")]
    internal class ExecutionerChargeIonsSkillModifier : ReflectedSkillModifier
    {
        private float originalBaseDuration = 0;
        private float originalDamageCoefficient = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Type chargeGunType = EntityStateTypes.Length > 0 ? EntityStateTypes[0] : null;
            
            if (chargeGunType != null)
            {
                if (Mathf.Abs(originalBaseDuration) < 0.01f) 
                    originalBaseDuration = GetStaticFloat(chargeGunType, "baseDuration");
                if (Mathf.Abs(originalDamageCoefficient) < 0.01f) 
                    originalDamageCoefficient = GetStaticFloat(chargeGunType, "damageCoefficient");

                SetStaticFloat(chargeGunType, "baseDuration", MultScaling(originalBaseDuration, -0.15f, level));
                SetStaticFloat(chargeGunType, "damageCoefficient", MultScaling(originalDamageCoefficient, 0.20f, level));
            }
        }
    }

    [SkillLevelModifier("sdExe2FireChargeBurst", "EntityStates.Executioner2.FireChargeGun")]
    internal class ExecutionerFireIonGunSkillModifier : ReflectedSkillModifier
    {
        private float originalBaseDuration = 0;
        private float originalDamageCoefficient = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Type fireChargeGunType = EntityStateTypes.Length > 0 ? EntityStateTypes[0] : null;
            
            if (fireChargeGunType != null)
            {
                if (Mathf.Abs(originalBaseDuration) < 0.01f) 
                    originalBaseDuration = GetStaticFloat(fireChargeGunType, "baseDuration");
                if (Mathf.Abs(originalDamageCoefficient) < 0.01f) 
                    originalDamageCoefficient = GetStaticFloat(fireChargeGunType, "damageCoefficient");

                SetStaticFloat(fireChargeGunType, "baseDuration", MultScaling(originalBaseDuration, -0.10f, level));
                SetStaticFloat(fireChargeGunType, "damageCoefficient", MultScaling(originalDamageCoefficient, 0.25f, level));
            }
        }
    }

    [SkillLevelModifier("sdExe2Slam", "EntityStates.Executioner2.ExecuteLeap", "EntityStates.Executioner2.ExecuteSlam", "EntityStates.Executioner2.ExecuteImpact")]
    internal class ExecutionerExecuteSkillModifier : ReflectedSkillModifier
    {
        private float originalLeapDamageCoefficient = 0;
        private float originalLeapBlastRadius = 0;
        private float originalSlamDamageCoefficient = 0;
        private float originalSlamBlastRadius = 0;
        private float originalImpactDamageCoefficient = 0;
        private float originalImpactDuration = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            if (EntityStateTypes.Length < 3) return;

            Type leapType = EntityStateTypes[0];
            Type slamType = EntityStateTypes[1];
            Type impactType = EntityStateTypes[2];

            if (leapType != null)
            {
                if (Mathf.Abs(originalLeapDamageCoefficient) < 0.01f) 
                    originalLeapDamageCoefficient = GetStaticFloat(leapType, "damageCoefficient");
                if (Mathf.Abs(originalLeapBlastRadius) < 0.01f) 
                    originalLeapBlastRadius = GetStaticFloat(leapType, "blastRadius");

                SetStaticFloat(leapType, "damageCoefficient", MultScaling(originalLeapDamageCoefficient, 0.20f, level));
                SetStaticFloat(leapType, "blastRadius", MultScaling(originalLeapBlastRadius, 0.25f, level));
            }

            if (slamType != null)
            {
                if (Mathf.Abs(originalSlamDamageCoefficient) < 0.01f) 
                    originalSlamDamageCoefficient = GetStaticFloat(slamType, "damageCoefficient");
                if (Mathf.Abs(originalSlamBlastRadius) < 0.01f) 
                    originalSlamBlastRadius = GetStaticFloat(slamType, "blastRadius");

                SetStaticFloat(slamType, "damageCoefficient", MultScaling(originalSlamDamageCoefficient, 0.20f, level));
                SetStaticFloat(slamType, "blastRadius", MultScaling(originalSlamBlastRadius, 0.25f, level));
            }

            if (impactType != null)
            {
                if (Mathf.Abs(originalImpactDamageCoefficient) < 0.01f) 
                    originalImpactDamageCoefficient = GetStaticFloat(impactType, "damageCoefficient");
                if (Mathf.Abs(originalImpactDuration) < 0.01f) 
                    originalImpactDuration = GetStaticFloat(impactType, "duration");

                SetStaticFloat(impactType, "damageCoefficient", MultScaling(originalImpactDamageCoefficient, 0.20f, level));
                SetStaticFloat(impactType, "duration", MultScaling(originalImpactDuration, -0.10f, level));
            }
        }
    }

    [SkillLevelModifier("sdExe2ChargeConsecration", "EntityStates.Executioner2.ChargeConsecration", "EntityStates.Executioner2.Consecration")]
    internal class ExecutionerConsecrationSkillModifier : ReflectedSkillModifier
    {
        private float originalChargeDuration = 0;
        private float originalDamageCoefficient = 0;
        private float originalBaseDuration = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            if (EntityStateTypes.Length < 2) return;

            Type chargeType = EntityStateTypes[0];
            Type consecrationType = EntityStateTypes[1];

            if (chargeType != null)
            {
                if (Mathf.Abs(originalChargeDuration) < 0.01f) 
                    originalChargeDuration = GetStaticFloat(chargeType, "baseDuration");

                SetStaticFloat(chargeType, "baseDuration", MultScaling(originalChargeDuration, -0.15f, level));
            }

            if (consecrationType != null)
            {
                if (Mathf.Abs(originalDamageCoefficient) < 0.01f) 
                    originalDamageCoefficient = GetStaticFloat(consecrationType, "damageCoefficient");
                if (Mathf.Abs(originalBaseDuration) < 0.01f) 
                    originalBaseDuration = GetStaticFloat(consecrationType, "baseDuration");

                SetStaticFloat(consecrationType, "damageCoefficient", MultScaling(originalDamageCoefficient, 0.25f, level));
                SetStaticFloat(consecrationType, "baseDuration", MultScaling(originalBaseDuration, 0.20f, level));
            }
        }
    }
}
