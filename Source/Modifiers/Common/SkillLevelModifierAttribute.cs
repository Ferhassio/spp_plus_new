using System;

namespace SkillsPlusPlus.Modifiers
{

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SkillLevelModifierAttribute : Attribute
    {

        internal readonly string[] skillNames;
        internal readonly Type[] baseStateTypes;

        public SkillLevelModifierAttribute(string name, params string[] skillnames)
        {
            this.skillNames = new string[0];
            this.baseStateTypes = new Type[0];
            Logger.Warn("Skill modifier for {0} is using a older version of the Skills++ API and cannot be loaded. Please notify the author to update their dependancies", name);
        }

        public SkillLevelModifierAttribute(string skillName, params Type[] stateTypes)
        {
            this.skillNames = new string[] { skillName };
            this.baseStateTypes = stateTypes;
        }

        public SkillLevelModifierAttribute(string[] skillNames, params Type[] stateTypes)
        {
            this.skillNames = skillNames;
            this.baseStateTypes = stateTypes;
        }

    }
}
