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

                float newDamage = MultScaling(originalDamageCoefficient, 0.15f, level);
                float newDuration = MultScaling(originalBaseDuration, -0.20f, level);
                
                SetStaticFloat(firePistolType, "damageCoefficient", newDamage);
                SetStaticFloat(firePistolType, "baseDuration", newDuration);
                
                Logger.Warn($"ExecutionerFirePistolSkillModifier: Level {level} - Damage: {originalDamageCoefficient} -> {newDamage}, Duration: {originalBaseDuration} -> {newDuration}");
            }
        }
    }

    [SkillLevelModifier("sdExe2FireTaser", "EntityStates.Executioner2.FireTaser")]
    internal class ExecutionerFireTaserSkillModifier : ReflectedSkillModifier
    {
        private float originalBaseDuration = 0;
        private float originalDamageCoefficient = 0;
        private int originalBaseBouncesRemaining = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Type fireTaserType = EntityStateTypes.Length > 0 ? EntityStateTypes[0] : null;
            
            if (fireTaserType != null)
            {
                if (Mathf.Abs(originalBaseDuration) < 0.01f) 
                    originalBaseDuration = GetStaticFloat(fireTaserType, "baseDuration");
                if (Mathf.Abs(originalDamageCoefficient) < 0.01f) 
                    originalDamageCoefficient = GetStaticFloat(fireTaserType, "damageCoefficient");
                if (originalBaseBouncesRemaining == 0)
                    originalBaseBouncesRemaining = GetStaticInt(fireTaserType, "baseBouncesRemaining");

                float newDuration = MultScaling(originalBaseDuration, -0.15f, level);
                float newDamage = MultScaling(originalDamageCoefficient, 0.20f, level);
                int newBouncesRemaining = originalBaseBouncesRemaining + level;
                
                SetStaticFloat(fireTaserType, "baseDuration", newDuration);
                SetStaticFloat(fireTaserType, "damageCoefficient", newDamage);
                SetStaticInt(fireTaserType, "baseBouncesRemaining", newBouncesRemaining);
                
                Logger.Warn($"ExecutionerFireTaserSkillModifier: Level {level} - Duration: {originalBaseDuration} -> {newDuration}, Damage: {originalDamageCoefficient} -> {newDamage}, Bounces: {originalBaseBouncesRemaining} -> {newBouncesRemaining}");
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

                float newDuration = MultScaling(originalBaseDuration, -0.15f, level);
                float newFearDuration = AdditiveScaling(originalFearDuration, 0.5f, level);
                
                SetStaticFloat(dashType, "baseDuration", newDuration);
                SetStaticFloat(dashType, "fearDuration", newFearDuration);
                
                Logger.Warn($"ExecutionerDashSkillModifier: Level {level} - Duration: {originalBaseDuration} -> {newDuration}, Fear Duration: {originalFearDuration} -> {newFearDuration}");
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

                float newChargeDuration = MultScaling(originalChargeDuration, -0.20f, level);
                SetStaticFloat(chargeType, "baseDuration", newChargeDuration);
                
                Logger.Warn($"ExecutionerBloodlettingSkillModifier: Level {level} - Charge Duration: {originalChargeDuration} -> {newChargeDuration}");
            }

            if (bloodlettingType != null)
            {
                if (Mathf.Abs(originalDamageCoefficient) < 0.01f) 
                    originalDamageCoefficient = GetStaticFloat(bloodlettingType, "damageCoefficient");
                if (Mathf.Abs(originalHealFraction) < 0.01f) 
                    originalHealFraction = GetStaticFloat(bloodlettingType, "healFraction");

                float newDamage = MultScaling(originalDamageCoefficient, 0.20f, level);
                float newHealFraction = AdditiveScaling(originalHealFraction, 0.05f, level);
                
                SetStaticFloat(bloodlettingType, "damageCoefficient", newDamage);
                SetStaticFloat(bloodlettingType, "healFraction", newHealFraction);
                
                Logger.Warn($"ExecutionerBloodlettingSkillModifier: Level {level} - Damage: {originalDamageCoefficient} -> {newDamage}, Heal Fraction: {originalHealFraction} -> {newHealFraction}");
            }
        }
    }

    [SkillLevelModifier("sdExe2ChargeIons", "EntityStates.Executioner2.ChargeGun")]
    internal class ExecutionerChargeIonsSkillModifier : ReflectedSkillModifier
    {
        private float originalBaseDuration = 0;
        private float originalDamageCoefficient = 0;
        private int originalBaseMaxStock = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Type chargeGunType = EntityStateTypes.Length > 0 ? EntityStateTypes[0] : null;
            
            if (chargeGunType != null)
            {
                if (Mathf.Abs(originalBaseDuration) < 0.01f) 
                    originalBaseDuration = GetStaticFloat(chargeGunType, "baseDuration");
                if (Mathf.Abs(originalDamageCoefficient) < 0.01f) 
                    originalDamageCoefficient = GetStaticFloat(chargeGunType, "damageCoefficient");

                float newDuration = MultScaling(originalBaseDuration, -0.15f, level);
                float newDamage = MultScaling(originalDamageCoefficient, 0.20f, level);
                
                SetStaticFloat(chargeGunType, "baseDuration", newDuration);
                SetStaticFloat(chargeGunType, "damageCoefficient", newDamage);
                
                Logger.Warn($"ExecutionerChargeIonsSkillModifier: Level {level} - Duration: {originalBaseDuration} -> {newDuration}, Damage: {originalDamageCoefficient} -> {newDamage}");
            }
            
            if (skillDef != null)
            {
                if (originalBaseMaxStock == 0)
                    originalBaseMaxStock = skillDef.baseMaxStock;
                
                int newBaseMaxStock = originalBaseMaxStock + (level * 5);
                skillDef.baseMaxStock = newBaseMaxStock;
                
                Logger.Warn($"ExecutionerChargeIonsSkillModifier: Level {level} - Base Max Stock: {originalBaseMaxStock} -> {newBaseMaxStock}");
            }
        }
    }

    [SkillLevelModifier("sdExe2FireChargeBurst", "EntityStates.Executioner2.FireChargeGun")]
    internal class ExecutionerFireIonGunSkillModifier : ReflectedSkillModifier
    {
        private float originalBaseDuration = 0;
        private float originalDamageCoefficient = 0;
        private float originalSelfKnockbackForce = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Type fireChargeGunType = EntityStateTypes.Length > 0 ? EntityStateTypes[0] : null;
            
            if (fireChargeGunType != null)
            {
                if (Mathf.Abs(originalBaseDuration) < 0.01f) 
                    originalBaseDuration = GetStaticFloat(fireChargeGunType, "baseDuration");
                if (Mathf.Abs(originalDamageCoefficient) < 0.01f) 
                    originalDamageCoefficient = GetStaticFloat(fireChargeGunType, "damageCoefficient");
                if (Mathf.Abs(originalSelfKnockbackForce) < 0.01f) 
                    originalSelfKnockbackForce = GetStaticFloat(fireChargeGunType, "selfKnockbackForce");

                float newDuration = MultScaling(originalBaseDuration, -0.15f, level);
                float newDamage = MultScaling(originalDamageCoefficient, 0.30f, level);
                float newKnockbackForce = MultScaling(originalSelfKnockbackForce, -0.10f, level);
                
                SetStaticFloat(fireChargeGunType, "baseDuration", newDuration);
                SetStaticFloat(fireChargeGunType, "damageCoefficient", newDamage);
                SetStaticFloat(fireChargeGunType, "selfKnockbackForce", newKnockbackForce);
                
                Logger.Warn($"ExecutionerFireIonGunSkillModifier: Level {level} - Duration: {originalBaseDuration} -> {newDuration}, Damage: {originalDamageCoefficient} -> {newDamage}, Knockback Force: {originalSelfKnockbackForce} -> {newKnockbackForce}");
            }
        }
    }

    [SkillLevelModifier("sdExe2Slam", "EntityStates.Executioner2.ExecuteLeap", "EntityStates.Executioner2.ExecuteSlam", "EntityStates.Executioner2.ExecuteImpact")]
    internal class ExecutionerExecuteSkillModifier : ReflectedSkillModifier
    {
        private float originalLeapBaseDuration = 0;
        private float originalLeapVerticalSpeed = 0;
        private float originalSlamBaseDamageCoefficient = 0;
        private float originalSlamBlastRadius = 0;
        private float originalSlamDuration = 0;
        private float originalImpactHitPauseDuration = 0;
        private int originalChargesToGrant = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            if (EntityStateTypes.Length < 3) return;

            Type leapType = EntityStateTypes[0];
            Type slamType = EntityStateTypes[1];
            Type impactType = EntityStateTypes[2];

            if (leapType != null)
            {
                if (Mathf.Abs(originalLeapBaseDuration) < 0.01f) 
                    originalLeapBaseDuration = GetStaticFloat(leapType, "baseDuration");
                if (Mathf.Abs(originalLeapVerticalSpeed) < 0.01f) 
                    originalLeapVerticalSpeed = GetStaticFloat(leapType, "baseVerticalSpeed");

                float newLeapDuration = MultScaling(originalLeapBaseDuration, -0.15f, level);
                float newLeapSpeed = MultScaling(originalLeapVerticalSpeed, 0.20f, level);
                
                SetStaticFloat(leapType, "baseDuration", newLeapDuration);
                SetStaticFloat(leapType, "baseVerticalSpeed", newLeapSpeed);
                
                Logger.Warn($"ExecutionerExecuteSkillModifier: Level {level} - Leap Duration: {originalLeapBaseDuration} -> {newLeapDuration}, Leap Speed: {originalLeapVerticalSpeed} -> {newLeapSpeed}");
            }

            if (slamType != null)
            {
                if (Mathf.Abs(originalSlamBaseDamageCoefficient) < 0.01f) 
                    originalSlamBaseDamageCoefficient = GetStaticFloat(slamType, "baseDamageCoefficient");
                if (Mathf.Abs(originalSlamBlastRadius) < 0.01f) 
                    originalSlamBlastRadius = GetStaticFloat(slamType, "slamRadius");
                if (Mathf.Abs(originalSlamDuration) < 0.01f) 
                    originalSlamDuration = GetStaticFloat(slamType, "duration");

                float newSlamDamage = MultScaling(originalSlamBaseDamageCoefficient, 0.25f, level);
                float newSlamBlast = MultScaling(originalSlamBlastRadius, 0.25f, level);
                float newSlamDuration = MultScaling(originalSlamDuration, -0.10f, level);
                
                SetStaticFloat(slamType, "baseDamageCoefficient", newSlamDamage);
                SetStaticFloat(slamType, "slamRadius", newSlamBlast);
                SetStaticFloat(slamType, "duration", newSlamDuration);
                
                Logger.Warn($"ExecutionerExecuteSkillModifier: Level {level} - Slam Damage: {originalSlamBaseDamageCoefficient} -> {newSlamDamage}, Slam Blast: {originalSlamBlastRadius} -> {newSlamBlast}, Slam Duration: {originalSlamDuration} -> {newSlamDuration}");
            }

            if (impactType != null)
            {
                if (Mathf.Abs(originalImpactHitPauseDuration) < 0.01f) 
                    originalImpactHitPauseDuration = GetStaticFloat(impactType, "hitPauseDurationSolo");

                float newImpactHitPause = MultScaling(originalImpactHitPauseDuration, -0.15f, level);
                
                SetStaticFloat(impactType, "hitPauseDurationSolo", newImpactHitPause);
                
                Logger.Warn($"ExecutionerExecuteSkillModifier: Level {level} - Impact Hit Pause: {originalImpactHitPauseDuration} -> {newImpactHitPause}");
            }
        }
    }

    [SkillLevelModifier("sdExe2ChargeConsecration", "EntityStates.Executioner2.ChargeConsecration", "EntityStates.Executioner2.Consecration")]
    internal class ExecutionerConsecrationSkillModifier : ReflectedSkillModifier
    {
        private float originalChargeDuration = 0;
        private float originalHealFraction = 0;
        private float originalBuffDuration = 0;
        private int originalBaseMaxStock = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            if (EntityStateTypes.Length < 2) return;

            Type chargeType = EntityStateTypes[0];
            Type consecrationType = EntityStateTypes[1];

            if (chargeType != null)
            {
                if (Mathf.Abs(originalChargeDuration) < 0.01f) 
                    originalChargeDuration = GetStaticFloat(chargeType, "baseDuration");

                float newChargeDuration = MultScaling(originalChargeDuration, -0.15f, level);
                SetStaticFloat(chargeType, "baseDuration", newChargeDuration);
                
                Logger.Warn($"ExecutionerConsecrationSkillModifier: Level {level} - Charge Duration: {originalChargeDuration} -> {newChargeDuration}");
            }

            if (consecrationType != null)
            {
                if (Mathf.Abs(originalHealFraction) < 0.01f) 
                    originalHealFraction = GetStaticFloat(consecrationType, "healFraction");
                if (Mathf.Abs(originalBuffDuration) < 0.01f) 
                    originalBuffDuration = GetStaticFloat(consecrationType, "buffDuration");

                float newHealFraction = MultScaling(originalHealFraction, 0.25f, level);
                float newBuffDuration = MultScaling(originalBuffDuration, 0.10f, level);
                
                SetStaticFloat(consecrationType, "healFraction", newHealFraction);
                SetStaticFloat(consecrationType, "buffDuration", newBuffDuration);
                
                Logger.Warn($"ExecutionerConsecrationSkillModifier: Level {level} - Heal Fraction: {originalHealFraction} -> {newHealFraction}, Buff Duration: {originalBuffDuration} -> {newBuffDuration}");
            }

            if (skillDef != null)
            {
                if (Mathf.Abs(originalBaseMaxStock) < 0.01f) 
                    originalBaseMaxStock = skillDef.baseMaxStock;

                int newMaxStock = originalBaseMaxStock + (5 * level);
                skillDef.baseMaxStock = newMaxStock;
                
                Logger.Warn($"ExecutionerConsecrationSkillModifier: Level {level} - Max Stock: {originalBaseMaxStock} -> {newMaxStock}");
            }
        }
    }
}
