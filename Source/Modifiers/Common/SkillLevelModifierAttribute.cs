using System;

namespace SkillsPlusPlus.Modifiers
{

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SkillLevelModifierAttribute : Attribute
    {

        internal readonly string[] skillNames;
        internal readonly Type[] baseStateTypes;
        internal readonly string[] baseStateTypeNames;

        public SkillLevelModifierAttribute(string skillName, params Type[] stateTypes)
        {
            this.skillNames = new string[] { skillName };
            this.baseStateTypes = stateTypes;
            this.baseStateTypeNames = new string[0];
        }

        public SkillLevelModifierAttribute(string[] skillNames, params Type[] stateTypes)
        {
            this.skillNames = skillNames;
            this.baseStateTypes = stateTypes;
            this.baseStateTypeNames = new string[0];
        }

        public SkillLevelModifierAttribute(string skillName, params string[] stateTypeNames)
        {
            this.skillNames = new string[] { skillName };
            this.baseStateTypes = new Type[0];
            this.baseStateTypeNames = stateTypeNames;
        }

        public SkillLevelModifierAttribute(string[] skillNames, params string[] stateTypeNames)
        {
            this.skillNames = skillNames;
            this.baseStateTypes = new Type[0];
            this.baseStateTypeNames = stateTypeNames;
        }

    }
}
