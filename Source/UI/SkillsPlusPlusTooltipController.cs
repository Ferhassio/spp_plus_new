using System;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace SkillsPlusPlus.UI
{

    [RequireComponent(typeof(TooltipController))]
    internal class SkillsPlusPlusTooltipController : MonoBehaviour
    {
        private TooltipController tooltipController;

        public string skillUpgradeToken;

        private void Awake()
        {
            this.tooltipController = GetComponent<TooltipController>();
        }

        private void Start()
        {
            var panelTransform = this.tooltipController.tooltipFlipTransform;
            GameObject bodyRectGameObject = this.tooltipController.bodyLabel.transform.parent.gameObject;
            GameObject skillUpgradeRect = Instantiate(bodyRectGameObject, panelTransform);
            skillUpgradeRect.transform.SetSiblingIndex(Math.Max(0, panelTransform.childCount - 2));

            skillUpgradeRect.SetActive(skillUpgradeToken != null && Language.IsTokenInvalid(skillUpgradeToken) == false);
            if (skillUpgradeToken != null && Language.IsTokenInvalid(skillUpgradeToken) == false)
            {
                var description = Language.GetString(skillUpgradeToken);
                var label = skillUpgradeRect.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                label.text = Language.GetStringFormatted("TOOLTIP_UPGRADE_DESCRIPTION", description);
            }
        }

    }

}