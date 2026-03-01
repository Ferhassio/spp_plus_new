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
        private static readonly Dictionary<string, Type> typeNameCache = new Dictionary<string, Type>();
        private static HashSet<string> loadedAssemblyNames = new HashSet<string>();

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

            foreach (var loadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                loadedAssemblyNames.Add(loadedAssembly.FullName);
            }
            Logger.Debug("Initialized with {0} loaded assemblies", loadedAssemblyNames.Count);

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
                            skillModifier.EntityStateTypeNames = attribute.baseStateTypeNames ?? new string[0];
                            
                            List<Type> resolvedTypes = new List<Type>();
                            
                            if (attribute.baseStateTypes != null && attribute.baseStateTypes.Length > 0)
                            {
                                resolvedTypes.AddRange(attribute.baseStateTypes);
                            }
                            
                            skillModifier.EntityStateTypes = resolvedTypes.ToArray();
                             
                             Logger.Warn("Loaded skill modifier: {0}", someSkillModifier.GetType().FullName);
                             Logger.Warn("  Skill names: {0}", string.Join(", ", attribute.skillNames));
                             Logger.Warn("  Entity state type names: {0}", string.Join(", ", skillModifier.EntityStateTypeNames));
                             
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
                            foreach (Type stateType in skillModifier.EntityStateTypes)
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

        public static Type ResolveType(string typeName)
        {
            if (typeNameCache.TryGetValue(typeName, out Type cachedType))
            {
                return cachedType;
            }

            Type type = Type.GetType(typeName);
            if (type != null)
            {
                typeNameCache[typeName] = type;
                return type;
            }

            foreach (Assembly loadedAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    type = loadedAssembly.GetType(typeName);
                    if (type != null)
                    {
                        typeNameCache[typeName] = type;
                        return type;
                    }
                }
                catch
                {
                    continue;
                }
            }

            Logger.Debug("Could not resolve type {0}. Available assemblies:", typeName);
            foreach (Assembly loadedAssembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Contains("Executioner") || a.FullName.Contains("Skills")).OrderBy(a => a.FullName))
            {
                Logger.Debug("  - {0}", loadedAssembly.FullName);
            }

            return null;
        }

        internal static void RegisterTypeToModifier(Type type, BaseSkillModifier modifier)
        {
            if (type != null && modifier != null)
            {
                if (!typeToModifierMap.ContainsKey(type))
                {
                    typeToModifierMap[type] = modifier;
                    Logger.Debug("Registered type {0} to modifier {1}", type.FullName, modifier.GetType().FullName);
                }
            }
        }

        internal static void CheckForNewAssembliesAndRetryResolution()
        {
            var currentAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            bool newAssemblyFound = false;

            foreach (var assembly in currentAssemblies)
            {
                if (!loadedAssemblyNames.Contains(assembly.FullName))
                {
                    loadedAssemblyNames.Add(assembly.FullName);
                    newAssemblyFound = true;
                    Logger.Debug("New assembly loaded: {0}", assembly.FullName);
                }
            }

            if (newAssemblyFound)
            {
                Logger.Debug("New assemblies detected, retrying type resolution for all modifiers");
                foreach (var modifier in skillNameToModifierMap.Values)
                {
                    modifier.ResolveEntityStateTypes();
                }
            }
        }

        internal static BaseSkillModifier GetSkillModifier(SkillDef skillDef)
        {
            if (skillDef == null)
            {
                return null;
            }
            string skillName = ((ScriptableObject)skillDef)?.name;
            Logger.Warn("GetSkillModifier called for skill: {0}", skillName);
            CheckForNewAssembliesAndRetryResolution();
            BaseSkillModifier modifier = GetSkillModifierByName(skillName);
            if (modifier != null)
            {
                Logger.Warn("Found modifier: {0}", modifier.GetType().FullName);
                modifier.ResolveEntityStateTypes();
            }
            else
            {
                Logger.Warn("No modifier found for skill: {0}", skillName);
                Logger.Warn("Available modifiers: {0}", string.Join(", ", skillNameToModifierMap.Keys));
            }
            return modifier;
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
            
            foreach (var modifier in skillNameToModifierMap.Values)
            {
                modifier.ResolveEntityStateTypes();
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