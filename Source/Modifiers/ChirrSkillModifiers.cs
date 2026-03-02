using System;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using UnityEngine;
using R2API;

namespace SkillsPlusPlus.Modifiers
{
    [SkillLevelModifier("FireTriLeaf", "EntityStates.Chirr.FireTriLeaf")]
    internal class ChirrFireTriLeafSkillModifier : ReflectedSkillModifier
    {
        private int originalNumShots = 0;
        private float originalDamageCoefficient = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Type fireTriLeafType = EntityStateTypes.Length > 0 ? EntityStateTypes[0] : null;
            
            if (fireTriLeafType != null)
            {
                if (originalNumShots == 0)
                    originalNumShots = GetStaticInt(fireTriLeafType, "numShots");
                if (Mathf.Abs(originalDamageCoefficient) < 0.01f)
                    originalDamageCoefficient = GetStaticFloat(fireTriLeafType, "damageCoefficient");

                int newNumShots = (int)AdditiveScaling(originalNumShots, 1, level);
                float newDamage = MultScaling(originalDamageCoefficient, 0.20f, level);
                
                SetStaticInt(fireTriLeafType, "numShots", newNumShots);
                SetStaticFloat(fireTriLeafType, "damageCoefficient", newDamage);
            }
        }
    }

    [SkillLevelModifier("SpitBomb", "EntityStates.Chirr.SpitBomb")]
    internal class ChirrSpitBombSkillModifier : ReflectedSkillModifier
    {
        private float originalDamageCoefficient = 0;
        private float originalBaseDuration = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Type spitBombType = EntityStateTypes.Length > 0 ? EntityStateTypes[0] : null;
            
            if (spitBombType != null)
            {
                if (Mathf.Abs(originalDamageCoefficient) < 0.01f)
                    originalDamageCoefficient = GetStaticFloat(spitBombType, "damageCoefficient");
                if (Mathf.Abs(originalBaseDuration) < 0.01f)
                    originalBaseDuration = GetStaticFloat(spitBombType, "baseDuration");

                float newDamage = MultScaling(originalDamageCoefficient, 0.20f, level);
                float newDuration = MultScaling(originalBaseDuration, 0.15f, level);
                
                SetStaticFloat(spitBombType, "damageCoefficient", newDamage);
                SetStaticFloat(spitBombType, "baseDuration", newDuration);
            }
        }
    }

    [SkillLevelModifier("GrabDash", "EntityStates.Chirr.GrabDash", "EntityStates.Chirr.AimDrop", "EntityStates.Chirr.DroppedState")]
    internal class ChirrGrabDashSkillModifier : ReflectedSkillModifier
    {
        private float originalDashSpeed = 0;
        private float originalThrowDamage = 0;
        private float originalDropDamage = 0;
        private float originalTurnAngle = 0;
        private float originalFriendAttackBoost = 0;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Type chirrSurvivorType = System.Type.GetType("SS2.Survivors.Chirr, SS2");

            for (int i = 0; i < EntityStateTypes.Length; i++)
            {
                Type stateType = EntityStateTypes[i];
                string stateName = stateType.Name;

                if (stateName == "GrabDash")
                {
                    if (Mathf.Abs(originalDashSpeed) < 0.01f)
                        originalDashSpeed = GetStaticFloat(stateType, "dashSpeed");

                    SetStaticFloat(stateType, "dashSpeed", MultScaling(originalDashSpeed, 0.15f, level));

                    if (Mathf.Abs(originalTurnAngle) < 0.01f)
                        originalTurnAngle = GetStaticFloat(stateType, "maxTurnAnglePerSecond");

                    SetStaticFloat(stateType, "maxTurnAnglePerSecond", MultScaling(originalTurnAngle, 0.20f, level));
                }
                else if (stateName == "AimDrop")
                {
                    if (Mathf.Abs(originalThrowDamage) < 0.01f)
                        originalThrowDamage = GetStaticFloat(stateType, "damageCoefficient");

                    SetStaticFloat(stateType, "damageCoefficient", MultScaling(originalThrowDamage, 0.25f, level));
                }
                else if (stateName == "DroppedState")
                {
                    if (Mathf.Abs(originalDropDamage) < 0.01f)
                        originalDropDamage = GetStaticFloat(stateType, "damageCoefficient");

                    SetStaticFloat(stateType, "damageCoefficient", MultScaling(originalDropDamage, 0.25f, level));
                }
            }

            if (chirrSurvivorType != null)
            {
                if (Mathf.Abs(originalFriendAttackBoost) < 0.01f)
                    originalFriendAttackBoost = GetStaticFloat(chirrSurvivorType, "_grabFriendAttackBoost");

                SetStaticFloat(chirrSurvivorType, "_grabFriendAttackBoost", MultScaling(originalFriendAttackBoost, 0.5f, level));
            }
        }
    }

    [SkillLevelModifier("Befriend", "EntityStates.Chirr.Befriend")]
    internal class ChirrBefriendSkillModifier : ReflectedSkillModifier
    {
        private float originalBaseDuration = 0;
        private float originalFriendHealthFraction = 0;
        private CharacterBody surv;
        private int skilllevel;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            surv = characterBody;
            skilllevel = level;

            Type befriendType = EntityStateTypes.Length > 0 ? EntityStateTypes[0] : null;
            Type friendTrackerType = System.Type.GetType("SS2.Components.ChirrFriendTracker, SS2");
            
            if (befriendType != null)
            {
                if (Mathf.Abs(originalBaseDuration) < 0.01f)
                    originalBaseDuration = GetStaticFloat(befriendType, "baseDuration");

                float newDuration = MultScaling(originalBaseDuration, -0.10f, level);
                SetStaticFloat(befriendType, "baseDuration", newDuration);
            }

            if (friendTrackerType != null)
            {
                if (Mathf.Abs(originalFriendHealthFraction) < 0.01f)
                    originalFriendHealthFraction = GetStaticFloat(friendTrackerType, "maximumFriendHealthFraction");

                float newFriendHealthFraction = MultScaling(originalFriendHealthFraction, 0.10f, level);
                SetStaticFloat(friendTrackerType, "maximumFriendHealthFraction", newFriendHealthFraction);
            }
        }

        public override void SetupSkill()
        {
            base.SetupSkill();
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPIOnGetStatCoefficients;
        }

        private void RecalculateStatsAPIOnGetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            Type friendControllerType = System.Type.GetType("SS2.Components.ChirrFriendController, SS2");
            if (friendControllerType != null && sender.GetComponent(friendControllerType) != null && sender == surv)
            {
                args.armorAdd += AdditiveScaling(0, 20, skilllevel);
                args.moveSpeedMultAdd += MultScaling(0, 0.05f, skilllevel);
            }
        }
    }
}
