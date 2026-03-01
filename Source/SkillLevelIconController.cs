using Rebindables;
using Rewired;
using RiskOfOptions;
using RoR2;
using RoR2.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SkillsPlusPlus
{
    internal sealed class SkillLevelIconController : MonoBehaviour
    {

        private static string BUY_TOKEN = "SKILLS_SLOT_BUY_BTN";

        private SkillIcon skillIcon;
        private GameObject CanBuyPanel;
        private GameObject LevelBackgroundPanel;
        private GameObject LevelText;
        private GameObject UpgradeButton;

        private HGTextMeshProUGUI levelTextMesh;
        private HGButton buyButton;

        private CanvasRenderer CanBuyBorderRenderer;
        public SkillUpgrade skillUpgrade;

        private void Awake()
        {
            this.skillIcon = GetComponent<SkillIcon>();
            
            this.CanBuyPanel = Instantiate(skillIcon.isReadyPanelObject, skillIcon.transform);
            this.CanBuyPanel.name = "CanBuyBorderPanel";
            CanBuyPanel.transform.SetSiblingIndex(1);
            CanBuyBorderRenderer = this.CanBuyPanel.GetComponent<CanvasRenderer>();

            // create the upgrade level label
            {
                LevelBackgroundPanel = new GameObject("Skills++ Level Background Panel");
                LevelBackgroundPanel.transform.parent = skillIcon.transform;
                LevelBackgroundPanel.transform.localPosition = Vector3.zero;
                LevelBackgroundPanel.transform.localScale = Vector3.one;

                RectTransform backgroundTransform = LevelBackgroundPanel.AddComponent<RectTransform>();
                backgroundTransform.offsetMin = Vector2.zero;
                backgroundTransform.offsetMax = Vector2.zero;
                backgroundTransform.anchorMin = new Vector2(0.5f, 0.25f);
                backgroundTransform.anchorMax = new Vector2(0.5f, 0.25f);
                backgroundTransform.ForceUpdateRectTransforms();

                RawImage backgroundImage = LevelBackgroundPanel.AddComponent<RawImage>();
                Texture2D texture = new Texture2D(1, 1);
                texture.wrapMode = TextureWrapMode.Repeat;
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
                backgroundImage.texture = texture;
                backgroundImage.color = new Color(0, 0, 0, 0.7f);
                backgroundImage.SetNativeSize();

                ContentSizeFitter contentFitter = LevelBackgroundPanel.AddComponent<ContentSizeFitter>();
                contentFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                contentFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                HorizontalLayoutGroup layoutGroup = LevelBackgroundPanel.AddComponent<HorizontalLayoutGroup>();
                layoutGroup.padding = new RectOffset(4, 4, 0, 0);
                layoutGroup.childAlignment = TextAnchor.MiddleCenter;

                LevelText = new GameObject("LevelIndicator");
                LevelText.transform.parent = LevelBackgroundPanel.transform;
                LevelText.AddComponent<CanvasRenderer>();
                RectTransform textTransform = LevelText.AddComponent<RectTransform>();

                this.levelTextMesh = LevelText.AddComponent<HGTextMeshProUGUI>();
                levelTextMesh.text = "";
                levelTextMesh.fontSize = 16;
                levelTextMesh.color = Color.yellow;
                levelTextMesh.alignment = TMPro.TextAlignmentOptions.Center;
                levelTextMesh.enableWordWrapping = false;

                textTransform.ForceUpdateRectTransforms();
                textTransform.localScale = Vector3.one; // fixes multiplayer bug where joining players have overscaled buttons
                textTransform.localPosition = Vector2.zero;
                textTransform.anchorMin = Vector2.zero;
                textTransform.anchorMax = Vector2.one;
                // textTransform.anchoredPosition = new Vector2(0.5f, 0); // bottom center corner
                textTransform.sizeDelta = Vector2.zero;
                textTransform.offsetMin = Vector2.zero;
                textTransform.offsetMax = Vector2.zero;
                textTransform.ForceUpdateRectTransforms();
            }

            // create the clickable upgrade button
            {
                UpgradeButton = new GameObject("BuySkillButton");
                UpgradeButton.transform.parent = skillIcon.transform;
                UpgradeButton.AddComponent<CanvasRenderer>();
                RectTransform buttonTransform = UpgradeButton.AddComponent<RectTransform>();

                MPEventSystemLocator eventSystemLocation = UpgradeButton.AddComponent<MPEventSystemLocator>();

                HGTextMeshProUGUI buttonTextMesh = UpgradeButton.AddComponent<HGTextMeshProUGUI>();
                buttonTextMesh.text = Language.GetString(BUY_TOKEN);
                buttonTextMesh.fontSize = 18;
                buttonTextMesh.color = Color.yellow;
                buttonTextMesh.alignment = TMPro.TextAlignmentOptions.Center;
                buttonTextMesh.enableWordWrapping = false;

                buyButton = UpgradeButton.AddComponent<HGButton>();
                buyButton.onClick.AddListener(this.OnBuySkill);

                buttonTransform.ForceUpdateRectTransforms();
                buttonTransform.localScale = Vector3.one; // fixes multiplayer bug where joining players have overscaled buttons
                buttonTransform.localPosition = Vector2.zero;
                buttonTransform.anchorMin = Vector2.zero;
                buttonTransform.anchorMax = Vector2.one;
                buttonTransform.anchoredPosition = new Vector2(0, 0); // bottom right corner
                buttonTransform.sizeDelta = Vector2.zero;
                buttonTransform.offsetMin = new Vector2(0, 0);
                buttonTransform.offsetMax = new Vector2(0, 0);
                buttonTransform.ForceUpdateRectTransforms();

                // ButtonSkinController skinController = UpgradeLevelButton.AddComponent<ButtonSkinController>();
                // skinController.skinData =
            }
        }

        private void Update()
        {
            if (skillIcon)
            {
                if (skillIcon.targetSkill)  //Prevents errors when morphing to Heretic
                {
                    var skillUpgrades = skillIcon.targetSkill?.characterBody?.GetComponents<SkillUpgrade>();
                    if (skillUpgrades != null)
                    {
                        foreach (var skillUpgrade in skillUpgrades)
                        {
                            if (skillUpgrade.targetGenericSkill && ((ScriptableObject)skillIcon?.targetSkill.skillDef)?.name == ((ScriptableObject)skillUpgrade.targetGenericSkill.skillDef)?.name)
                            {
                                this.skillUpgrade = skillUpgrade;
                            }
                        }
                    }
                }
            }

            if (skillUpgrade)
            {
                var canBuySkill = skillUpgrade.CanUpgradeSkill();
                if (levelTextMesh != null)
                {
                    levelTextMesh.text = skillUpgrade.skillLevel > 0 ? skillUpgrade.skillLevel.ToString() : null;
                }
                CanBuyBorderRenderer.gameObject.SetActive(canBuySkill);
                CanBuyBorderRenderer.SetColor(Color.yellow);

                var masterController = skillIcon?.playerCharacterMasterController;
                if (masterController)
                {

                    LocalUser localUser = masterController?.networkUser?.localUser;
                    Player inputPlayer = localUser?.inputPlayer;
                    InputBankTest inputBank = masterController.body.inputBank;

                    if (inputPlayer != null)
                    {
                        //if (localUser.eventSystem.currentInputSource == MPEventSystem.InputSource.Gamepad) {
                        if (skillIcon != null)
                        {
                            SkillSlot skillSlot = skillIcon.targetSkillSlot;
                            int skillAction = 0;
                            switch (skillSlot)
                            {
                                case SkillSlot.None:
                                    skillAction = 0;
                                    break;
                                case SkillSlot.Primary:
                                    skillAction = RewiredConsts.Action.PrimarySkill;
                                    break;
                                case SkillSlot.Secondary:
                                    skillAction = RewiredConsts.Action.SecondarySkill;
                                    break;
                                case SkillSlot.Utility:
                                    skillAction = RewiredConsts.Action.UtilitySkill;
                                    break;
                                case SkillSlot.Special:
                                    skillAction = RewiredConsts.Action.SpecialSkill;
                                    break;
                            }
                            UpgradeButton.SetActive(canBuySkill && (ConVars.ConVars.buySkillsKeybind.IsPressedInclusive() || inputBank.GetButtonState(SkillOptions.hotkey).down) || inputPlayer.GetButton(RewiredConsts.Action.Info));
                            if (skillAction != 0 && inputPlayer.GetButtonDown(skillAction) && (ConVars.ConVars.buySkillsKeybind.IsPressedInclusive() || inputBank.GetButtonState(SkillOptions.hotkey).down))
                            {
                                this.OnBuySkill();
                            }
                        }
                        //} else {
                        //    UpgradeButton.SetActive(canBuySkill && inputPlayer.GetButton(RewiredConsts.Action.Info));
                        //}
                    }
                }
            }
        }

        private void OnBuySkill()
        {
            if (skillUpgrade)
            {
                skillUpgrade.OnBuySkill();
            }
        }
    }
}