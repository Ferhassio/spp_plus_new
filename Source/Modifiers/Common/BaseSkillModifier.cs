using System;
using BepInEx.Configuration;
using EntityStates;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace SkillsPlusPlus.Modifiers
{
    public abstract class BaseSkillModifier
    {

        /// <inheritdoc/>
        internal Type[] EntityStateTypes { get; set; }

        /// <summary>
        /// The list of skillnames associated with this modifier.
        /// </summary>
        internal string[] skillNames { get; set; }

        /// <summary>
        /// The SkillUpgrade associated to this Modifier. Call FindSkillUpgrade to assign.
        /// </summary>
        public SkillUpgrade registeredSkill;
        
        /// <summary>
        /// The current character body skill modifier is being applied to.
        /// </summary>
        internal CharacterBody characterBody { get; set; }


        static bool bMultScalingLinear;

        public BaseSkillModifier()
        {
            this.skillNames = new string[0];
            this.EntityStateTypes = new Type[0];
        }

        public BaseSkillModifier(string[] skillNames, Type[] entityStateTypes)
        {
            this.skillNames = skillNames;
            this.EntityStateTypes = entityStateTypes;
        }

        /// <summary>
        /// Called immediately before the character enters one of the listed entity state types
        /// </summary>
        /// <param name="skillState">The entity state instance</param>
        /// <param name="level">The current level of the associated skill</param>
        public virtual void OnSkillEnter(BaseState skillState, int level)
        {
            Logger.Debug("{0}.OnSkillEnter({1}, {2})", this.GetType().Name, skillState, level);
        }

        /// <summary>
        /// Called when the character exits one of the listed entity state types
        /// </summary>
        /// <param name="skillState">The entity state instance</param>
        /// <param name="level">The current level of the associated skill</param>
        public virtual void OnSkillExit(BaseState skillState, int level)
        {
            Logger.Debug("{0}.OnSkillExit({1}, {2})", this.GetType().Name, skillState, level);
        }

        /// <summary>
        /// Called when the player spends a skill point one of the listed entity state types
        /// </summary>
        /// <param name="level">The new level of the skill</param>
        /// <param name="characterBody">The player's character body</param>
        /// <param name="skillDef">The associated skill definition</param>
        public virtual void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            Logger.Debug("{0}.OnSkillLeveledUp({1}, {2}, {3})", this.GetType().Name, level, characterBody, skillDef);
            FindSkillUpgrade(characterBody, "blank", true);

            bMultScalingLinear = (bool)registeredSkill?.skillPointsController?.multScalingLinear;
        }

        /// <summary>
        /// Provides a string token to replace the associated skill's descriptions.
        /// 
        /// Use null if you do not wish to change the skill's existing description.
        /// </summary>
        /// <remarks>This API is still a work in progress and may be deprecated in later releases</remarks>
        /// 
        /// <returns>The token resources for to replace the associated skills description.</returns>
        public virtual string GetOverrideSkillDescriptionToken()
        {
            return null;
        }

        internal void ReportBroken(params String[] fields)
        {
            var fieldInfo = String.Join(",", fields);
            Logger.Warn("Skill {0} is broken. Cannot access the following items: {1}", this.GetType().FullName, fieldInfo);
        }

        /// <summary>
        /// Called before registering a BaseSkillModifier. Override this function to call things such as adding hooks and Patching Skill Names
        /// </summary>
        public virtual void SetupSkill()
        {
            return;
        }

        public virtual void SetupConfig(ConfigFile config)
        {
            return;
        }

        #region Helpers

        /// <summary>
        /// A helper method that performs the math to acheive a linear scaling.
        /// <para>
        /// More docs: <see href="https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/blob/feature/public-api/Documentation/scaling-operators.md"/>
        /// </para>
        /// </summary>
        /// <param name="baseValue">The base value when the skill has not been upgraded</param>
        /// <param name="constant">The amount to add every level</param>
        /// <param name="level">The current level to scale up to</param>
        /// <returns></returns>
        public static float AdditiveScaling(float baseValue, float constant, int level)
        {
            return baseValue + constant * level;
        }

        /// <summary>
        /// A helper method that performs the math to acheive a linear scaling.
        /// <para>
        /// More docs: <see href="https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/blob/feature/public-api/Documentation/scaling-operators.md"/>
        /// </para>
        /// </summary>
        /// <param name="baseValue">The base value when the skill has not been upgraded</param>
        /// <param name="constant">The amount to add every level</param>
        /// <param name="level">The current level to scale up to</param>
        /// <returns></returns>
        public static int AdditiveScaling(int baseValue, int constant, int level)
        {
            return baseValue + constant * level;
        }

        /// <summary>
        /// A helper method that performs the math to acheive a compounding scaling.
        /// Use this if you want the base value to multiply by a constant amount every level. 
        /// <para>
        /// More docs: <see href="https://gitlab.com/cwmlolzlz/ror2skillsplusplus/-/blob/feature/public-api/Documentation/scaling-operators.md"/>
        /// </para>
        /// </summary>
        /// <param name="baseValue">The Base value when the skill has not been upgraded</param>
        /// <param name="multiplier">The multiplication to apply per level</param>
        /// <param name="level">The current level to scale up to</param>
        /// <returns></returns>
        public static float MultScaling(float baseValue, float multiplier, int level)
        {
            if (multiplier <= -1)
            {
                Logger.Error("Multipliers less than -1 are not allowed as it causes sporadic behaviour with the scaling.");
                return baseValue;
            }

            //Do not use linear scaling if using negative multipliers as it can lead to < 0 values.
            if ((bool)bMultScalingLinear && multiplier > 0)
            {
                return (float)((multiplier * level) + 1) * baseValue;
            }
            else
            {
                return (float)((Math.Pow(multiplier + 1, level) - 1) * baseValue) + baseValue;
            }
        }

        // public static int MultScaling(int baseValue, float multiplier, int level) {
        //     return (int)MultScaling((float)baseValue, multiplier, level);            
        // }

        /// <summary>
        /// A helper method that find the SkillUpgrade of a Skill by name.
        /// </summary>
        /// <param name="characterBody">The characterbody to search.</param>
        /// <param name="baseSkillName">The base skill name to find.</param>
        /// <param name="bConfirmSkill">Whether we need to confirm the registered skill is still valid.</param>
        /// <returns></returns>
        protected void FindSkillUpgrade(CharacterBody characterBody, String baseSkillName, bool bConfirmSkill = false)
        {

            if (bConfirmSkill && registeredSkill)
            {
                SkillUpgrade[] upgrades = characterBody.GetComponents<SkillUpgrade>();

                //If registered skill isn't on this character body, null it.
                if (Array.IndexOf(upgrades, registeredSkill) == -1)
                {
                    registeredSkill = null;
                }
            }

            if (registeredSkill == null)
            {
                SkillUpgrade[] upgrades = characterBody.GetComponents<SkillUpgrade>();

                for (int i = 0; i < upgrades.Length; i++)
                {
                    if (upgrades[i] != null)
                    {
                        foreach (GenericSkill skill in characterBody?.skillLocator?.allSkills)
                        {
                            int pos = Array.IndexOf(skillNames, ((ScriptableObject)skill.skillDef)?.name);
                            if (pos > -1)
                            {
                                if (upgrades[i].targetGenericSkill.skillFamily == skill.skillFamily)
                                {
                                    registeredSkill = upgrades[i];
                                    break;
                                }
                            }
                        }
                    }

                    if (registeredSkill)
                    {
                        break;
                    }
                }
            }

            if (registeredSkill == null)
            {
                Logger.Warn("Could not find {0}'s Skill Upgrade", baseSkillName);
            }
        }

        #endregion
    }
}