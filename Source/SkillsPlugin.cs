using System;
using BepInEx;
using BepInEx.Configuration;
using R2API.Utils;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2;
using RoR2.UI;

using SkillsPlusPlus.Modifiers;
using SkillsPlusPlus.UI;
using UnityEngine;
using UnityEngine.Networking;

using Rebindables;
using Rewired;
using Rewired.Data;
using System.Linq;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Rewired.Data.Mapping;
//using UnityHotReloadNS;

namespace SkillsPlusPlus
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency("com.KingEnderBrine.ExtendedLoadout", BepInDependency.DependencyFlags.SoftDependency)] //Soft-dependency to make Skills++ load after ExtendedLoadout
    [BepInPlugin("com.cwmlolzlz.skills", "Skills", "0.6.4")]
    [BepInDependency("pseudopulse.Rebindables")]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod)]
    public sealed class SkillsPlugin : BaseUnityPlugin
    {
        public static SkillsPlugin Instance = null;

        private void Awake()
        {
            //   _____  _     _  _  _
            //  / ____|| |   (_)| || |        _      _
            // | (___  | | __ _ | || | ___  _| |_  _| |_
            //  \___ \ | |/ /| || || |/ __||_   _||_   _|
            //  ____) ||   < | || || |\__ \  |_|    |_|
            // |_____/ |_|\_\|_||_||_||___/
#if DEBUG
            //enable logger.debug for debug
            SkillsPlusPlus.Logger.LOG_LEVEL = SkillsPlusPlus.Logger.LogLevel.Debug;
            UnityEngine.Networking.LogFilter.currentLogLevel = LogFilter.Debug;

            //unlock all for debug
            On.RoR2.Stats.StatSheet.HasUnlockable += (orig, self, def) =>
            {
                return true;
            };
#endif

            Instance = this;

            //this is required to ensure that you can actually upgrade skills .,.,
            GameObject playerMasterPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/charactermasters/CommandoMaster");
            playerMasterPrefab.EnsureComponent<SkillPointsController>();
            
            //ensure hotkeys are properly setup 
            
            
            SkillModifierManager.LoadSkillModifiers();
            SkillOptions.SetupGameplayOptions();

            R2API.RecalculateStatsAPI.GetStatCoefficients += LunarModifiers.RecalculateStats_GetLunarStats;

            HUD.onHudTargetChangedGlobal += HUD_onHudTargetChangedGlobal;

            On.RoR2.UI.TooltipController.SetTooltipProvider += TooltipController_SetTooltipProvider;
            On.RoR2.UI.LoadoutPanelController.Row.FromSkillSlot += Row_FromSkillSlot;

            InitConfig();
        }

        private static void TooltipController_SetTooltipProvider(On.RoR2.UI.TooltipController.orig_SetTooltipProvider orig, TooltipController self, TooltipProvider provider)
        {
            orig(self, provider);

            if (provider.TryGetComponent(out SkillUpgradeTooltipProvider tooltipProvider))
            {
                var tooltipController = self.EnsureComponent<SkillsPlusPlusTooltipController>();
                tooltipController.skillUpgradeToken = tooltipProvider.GetToken();
            }
        }

        private static LoadoutPanelController.Row Row_FromSkillSlot(On.RoR2.UI.LoadoutPanelController.Row.orig_FromSkillSlot orig, LoadoutPanelController owner, BodyIndex bodyIndex, int skillSlotIndex, GenericSkill skillSlot)
        {
            var row = orig(owner, bodyIndex, skillSlotIndex, skillSlot);

            if (row == null) return null;
                
            var buttons = row.rowData;

            for (int i = 0; i < buttons.Count; i++)
            {
                SkillsPlusPlus.Logger.Debug("Ensuring SkillsPlusPlusTooltipProvider({0})", i);
                var button = buttons[i];
                var skillDef = skillSlot?.skillFamily?.variants[i].skillDef;
                if (skillDef != null)
                {
                    var provider = button.button.gameObject.EnsureComponent<SkillUpgradeTooltipProvider>();
                    provider.skillName = ((ScriptableObject)skillDef)?.name;
                    SkillsPlusPlus.Logger.Debug(((ScriptableObject)skillDef)?.name);
                }
            }

            return row;
        }

        private void InitConfig()
        {
            {
                var levelsPerSkillPoint = Config.Bind("Skills++",
                    "Levels per skill point",
                    5f,
                    "The number of levels to reach to be rewarded with a skillpoint. Changes will not be applied during a run. In multiplayer runs the host's setting is used");

                levelsPerSkillPoint.SettingChanged += (sender, args) => ConVars.ConVars.levelsPerSkillPoint.value = Mathf.RoundToInt(levelsPerSkillPoint.Value);

                ConVars.ConVars.levelsPerSkillPoint.value = Mathf.RoundToInt(levelsPerSkillPoint.Value);

                ModSettingsManager.AddOption(new SliderOption(levelsPerSkillPoint, new SliderConfig
                {
                    max = 50,
                    min = 1,
                    FormatString = "{0:0}"
                }));
            }

            {
                var disableInput = Config.Bind("Skills++",
                    "Disable Skills While Buying",
                    true,
                    "Should skills be disabled while the Buy Skills Input is pressed. (Disable this if you find yourself hitting the key by mistake)");

                disableInput.SettingChanged += (sender, args) => ConVars.ConVars.disableOnBuy.value = disableInput.Value;

                ConVars.ConVars.disableOnBuy.value = disableInput.Value;

                ModSettingsManager.AddOption(new CheckBoxOption(disableInput));
            }

            {
                var multScalingLinear = Config.Bind("Skills++",
                    "Linear Skill Multipliers",
                    false,
                    "Should Multiplicative (+%) skill values use a linear value rather than an exponential one. (Useful for playing with low \"Levels per skill point\" values). In multiplayer runs the host's setting is used");

                multScalingLinear.SettingChanged += (sender, args) => ConVars.ConVars.multScalingLinear.value = multScalingLinear.Value;

                ConVars.ConVars.multScalingLinear.value = multScalingLinear.Value;

                ModSettingsManager.AddOption(new CheckBoxOption(multScalingLinear));
            }
        }

        [SystemInitializer(typeof(SurvivorCatalog))]
        private static void OnCatalogInitializer()
        {
            foreach (var survivorDef in SurvivorCatalog.allSurvivorDefs)
            {
                Instance.PrepareSurvivor(survivorDef);
            }
        }

        private void PrepareSurvivor(SurvivorDef survivorDef)
        {
            if (!survivorDef || !survivorDef.bodyPrefab)
                return;

            var upgrades = survivorDef.bodyPrefab.GetComponents<SkillUpgrade>();
            for (int i = 0; i < upgrades.Length; i++)
            {
                Destroy(upgrades[i]);
            }

            foreach (var genericSkill in survivorDef.bodyPrefab.GetComponents<GenericSkill>())
            {
                if (genericSkill == null)
                {
                    continue;
                }

                var skillUpgrade = survivorDef.bodyPrefab.AddComponent<SkillUpgrade>();
                skillUpgrade.targetGenericSkill = genericSkill;
            }
        }

        private static void HUD_onHudTargetChangedGlobal(HUD hud)
        {
            foreach (var skillIcon in hud.GetComponentsInChildren<SkillIcon>(true))
            {
                skillIcon.EnsureComponent<SkillUpgradeTooltipProvider>();
                skillIcon.EnsureComponent<SkillLevelIconController>();
            }
        }
        
        // void Update()
        // {
        //     if (Input.GetKeyUp(KeyCode.F4))
        //     {
        //         UnityHotReload.LoadNewAssemblyVersion(
        //             typeof(SkillsPlugin).Assembly, // The currently loaded assembly to replace.
        //             System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Info.Location), "Skills.dll")  // The path to the newly compiled DLL.
        //         );
        //     }
        // }
    }
}