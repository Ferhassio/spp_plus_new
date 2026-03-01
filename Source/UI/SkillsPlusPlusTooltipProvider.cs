using RoR2.UI;
using UnityEngine;

namespace SkillsPlusPlus.UI
{
    internal class SkillUpgradeTooltipProvider : MonoBehaviour
    {

        public string skillName;
        public SkillIcon skillIcon;

        private void Awake()
        {
            this.skillIcon = GetComponent<SkillIcon>();
        }

        internal static string SkillNameToToken(string skillName)
        {
            if (skillName == null)
            {
                return null;
            }
            return (skillName + "_UPGRADE_DESCRIPTION").ToUpper();
        }

        internal string GetToken()
        {
            if (skillName != null)
            {
                return SkillNameToToken(skillName);
            }
            if (skillIcon)
            {
                var skillName = ((ScriptableObject)skillIcon.targetSkill?.skillDef)?.name;
                if (skillName != null)
                {
                    return SkillNameToToken(skillName);
                }
            }
            return null;
        }

    }
}