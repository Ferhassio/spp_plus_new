using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using UnityEngine;

namespace SkillsPlusPlus
{
    public sealed class SkillModifierManager
    {

        private static readonly Dictionary<string, BaseSkillModifier> skillNameToModifierMap = new Dictionary<string, BaseSkillModifier>();
        private static readonly Dictionary<Type, BaseSkillModifier> typeToModifierMap = new Dictionary<Type, BaseSkillModifier>();

        /// <summary>
        /// Finds and loads all skill modifiers in the current assembly.
        /// 
        /// Calling this is essential to have your skill modifiers available to Skills++
        /// </summary>
        public static void LoadSkillModifiers()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            if (assembly == null)
            {
                return;
            }
            foreach (Type type in assembly.GetTypes())
            {
                var attributes = type.GetCustomAttributes<SkillLevelModifierAttribute>();
                if (attributes == null || attributes.Count() == 0)
                {
                    continue;
                }
                try
                {
                    ConstructorInfo constructorInfo = type.GetConstructor(new Type[0]);
                    if (constructorInfo == null)
                    {
                        Logger.Debug("Failed to find constructor info for {0}", type.FullName);
                        Logger.Debug("Other constructors included");
                        foreach (ConstructorInfo info in type.GetConstructors())
                        {
                            Logger.Debug(info);
                        }
                        continue;
                    }
                    foreach (SkillLevelModifierAttribute attribute in attributes)
                    {
                        object someSkillModifier = constructorInfo.Invoke(new object[0]);

                        if (someSkillModifier is BaseSkillModifier skillModifier)
                        {
                            skillModifier.SetupSkill();
                            skillModifier.SetupConfig(SkillsPlugin.Instance.Config);

                            skillModifier.skillNames = attribute.skillNames;
                            skillModifier.EntityStateTypes = attribute.baseStateTypes;
                            foreach (string skillName in attribute.skillNames)
                            {
                                if (skillNameToModifierMap.TryGetValue(skillName, out BaseSkillModifier existingModifier))
                                {
                                    Logger.Warn("Skill modifier conflict!!!");
                                    Logger.Warn("Cannot add {0} since {1} already exists for skill named {2}", someSkillModifier.GetType().FullName, existingModifier.GetType().FullName, skillName);
                                    continue;
                                }
                                skillNameToModifierMap[skillName] = skillModifier;
                            }
                            foreach (Type stateType in attribute.baseStateTypes)
                            {
                                if (typeToModifierMap.TryGetValue(stateType, out BaseSkillModifier existingModifier))
                                {
                                    Logger.Warn("Skill modifier conflict!!!");
                                    Logger.Warn("Cannot add {0} since {1} already exists for the entity state {2}", someSkillModifier.GetType().FullName, existingModifier.GetType().FullName, stateType.FullName);
                                    continue;
                                }
                                typeToModifierMap[stateType] = skillModifier;
                            }
                        }
                    }
                }
                catch (Exception error)
                {
                    Logger.Error(error);
                    continue;
                }

            }
        }

        internal static BaseSkillModifier GetSkillModifier(SkillDef skillDef)
        {
            if (skillDef == null)
            {
                return null;
            }
            return GetSkillModifierByName(((ScriptableObject)skillDef)?.name);
        }

        internal static BaseSkillModifier GetSkillModifierByName(string skillName)
        {
            if (skillName == null)
            {
                return null;
            }
            if (skillNameToModifierMap.TryGetValue(skillName, out BaseSkillModifier modifier))
            {
                return modifier;
            }
            return null;
        }

        internal static bool HasSkillModifier(SkillDef skillDef)
        {
            if (skillDef == null)
            {
                return false;
            }
            return HasSkillModifier(((ScriptableObject)skillDef)?.name);
        }

        internal static bool HasSkillModifier(string baseSkillName)
        {
            if (baseSkillName == null)
            {
                return false;
            }
            return skillNameToModifierMap.ContainsKey(baseSkillName);
        }

        // internal static BaseSkillModifier GetSkillModifier(string skillName) {

        // }

        internal static BaseSkillModifier GetSkillModifiersForEntityStateType(Type entityStateType)
        {
            if (entityStateType == null)
            {
                return null;
            }
            if (typeToModifierMap.TryGetValue(entityStateType, out BaseSkillModifier modifiers))
            {
                return modifiers;
            }
            // if (entityStateType != typeof(GenericCharacterPod)) {
            //     Logger.Debug("Could not find any ISkillModifiers for entity state {0}", entityStateType.FullName);
            // }
            return null;
        }

    }
}