using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using HarmonyLib;
using RoR2;
using RoR2.ConVar;
using UnityEngine;

namespace SkillsPlusPlus.ConVars
{
    internal static class ConVars
    {

        internal static StringListConVar disabledSurvivors = new StringListConVar("spp_disabled_survivors", ConVarFlags.Archive, new List<string>(), "The list of survivors excluded from Skills++ behaviour in a string form");
        internal static KeyboardShortcut buySkillsKeybind = new KeyboardShortcut();
        internal static BoundedIntConVar levelsPerSkillPoint = new BoundedIntConVar("spp_levels_per_skillpoint", ConVarFlags.Archive, "5", 1, 99, Language.GetString("LEVELS_PER_SKILLPOINT_DESCRIPTION"));
        internal static BoolConVar multScalingLinear = new BoolConVar("spp_mult_scaling_linear", ConVarFlags.Archive, "0", Language.GetString("MULT_SCALING_LINEAR_DESCRIPTION"));
        internal static BoolConVar disableOnBuy = new BoolConVar("spp_disable_skills_buy_input", ConVarFlags.Archive, "1", Language.GetString("DISABLE_SKILL_BUY_INPUT_DESCRIPTION"));

        [ConCommand(commandName = "spp_disable_survivor", flags = ConVarFlags.None, helpText = "spp_disable_survivor <survivor name>\n  Disables Skills++ for the named survivor.")]
        public static void CCDisableSurvivor(ConCommandArgs args)
        {
            string survivorName = args.TryGetArgString(0);
            if (survivorName == null)
            {
                Debug.Log("Could not parse a survivor name. Did you specify a survivor name?");
                return;
            }
            bool didFindSurvivor = false;
            foreach (SurvivorDef survivorDef in SurvivorCatalog.allSurvivorDefs)
            {
                string resolvedSurvivorName = Language.GetString(survivorDef.displayNameToken);
                if (survivorName.Equals(resolvedSurvivorName, StringComparison.OrdinalIgnoreCase))
                {
                    if (disabledSurvivors.value.Contains(resolvedSurvivorName))
                    {
                        Debug.LogFormat("{0} has already been disabled", resolvedSurvivorName);
                        return;
                    }
                    else
                    {
                        didFindSurvivor = true;
                        disabledSurvivors.value.Add(resolvedSurvivorName);
                    }
                }
            }
            if (didFindSurvivor)
            {
                Debug.Log(String.Format("Disabled survirors: '{0}'", disabledSurvivors.GetString()));
                disabledSurvivors.AttemptSetString(disabledSurvivors.GetString());
            }
            else
            {
                Debug.LogFormat("Could not find any survivor named '{0}'", survivorName);
            }
        }

        [ConCommand(commandName = "spp_enable_survivor", flags = ConVarFlags.None, helpText = "spp_enable_survivor <survivor name>\n  Re-enables Skills++ for the named survivor.")]
        public static void CCEnableSurvivor(ConCommandArgs args)
        {
            string survivorName = args.TryGetArgString(0);
            if (survivorName == null)
            {
                Debug.Log("Could not parse a survivor name. Did you specify a survivor name?");
                return;
            }
            foreach (SurvivorDef survivorDef in SurvivorCatalog.allSurvivorDefs)
            {
                string resolvedSurvivorName = Language.GetString(survivorDef.displayNameToken);
                if (survivorName.Equals(resolvedSurvivorName, StringComparison.OrdinalIgnoreCase))
                {
                    disabledSurvivors.value.Remove(resolvedSurvivorName);
                }
            }
            Debug.Log(String.Format("Disabled survirors: '{0}'", disabledSurvivors.GetString()));
        }
    }

    internal class StringListConVar : BaseConVar
    {

        public List<string> value { get; protected set; }

        public StringListConVar(string name, ConVarFlags flags, List<string> defaultValue, string helpText) : base(name, flags, "[]", helpText)
        {
            this.value = defaultValue;
        }

        public override string GetString()
        {
            return "[" + value.Join() + "]";
        }

        public override void SetString(string newValue)
        {
            Logger.Debug("newValue: " + newValue + ";");
            newValue = newValue.Trim('[', ']');
            this.value = newValue.Split(',').Select((item) =>
            {
                return item.Trim();
            }).ToList();
            foreach (var suvirvorName in this.value)
            {
                Logger.Debug("newValue result: " + suvirvorName + ";");
            }
        }
    }

    internal class BoundedIntConVar : IntConVar
    {
        internal int minValue { get; private set; }
        internal int maxValue { get; private set; }

        public BoundedIntConVar(string name, ConVarFlags flags, string defaultValue, int minValue, int maxValue, string helpText) : base(name, flags, defaultValue, helpText)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
        public override void SetString(string newValue)
        {
            int value;
            if (TextSerialization.TryParseInvariant(newValue, out value))
            {
                this.value = Mathf.Clamp(value, minValue, maxValue);
            }
        }

    }
}