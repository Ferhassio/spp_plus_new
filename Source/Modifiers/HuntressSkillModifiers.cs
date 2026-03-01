using System;
using System.Collections.Generic;
using EntityStates;
using EntityStates.Huntress;
using EntityStates.Huntress.HuntressWeapon;
using EntityStates.Huntress.Weapon;
using RoR2;
using RoR2.Skills;
using RoR2.UI;
using UnityEngine;
using static RoR2.RoR2Content;

namespace SkillsPlusPlus.Modifiers
{

    [SkillLevelModifier("HuntressBodyFireSeekingArrow", typeof(FireSeekingArrow))]
    internal class HuntressSeekingArrowSkillModifier : SimpleSkillModifier<FireSeekingArrow>
    {
        public override void OnSkillEnter(FireSeekingArrow skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            skillState.orbProcCoefficient = AdditiveScaling(1f, 0.2f, level);
            //skillState.orbProcCoefficient;
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            if (characterBody.TryGetComponent(out HuntressTracker huntressTracker))
            {
                huntressTracker.maxTrackingDistance = MultScaling(60, 0.2f, level);
                huntressTracker.maxTrackingAngle = Math.Min(70, AdditiveScaling(30, 5, level)); // 16%
            }
            else
            {
                Logger.Warn("Could not locate the HuntressTracker component on {0}", characterBody);
            }
        }
    }

    [SkillLevelModifier("FireFlurrySeekingArrow", typeof(FireFlurrySeekingArrow))]
    internal class HuntressFlurrySkillModifier : SimpleSkillModifier<FireFlurrySeekingArrow>
    {
        public override void OnSkillEnter(FireFlurrySeekingArrow skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            skillState.maxArrowCount = AdditiveScaling(3, 1, level);
            skillState.baseArrowReloadDuration = 0.3f / skillState.maxArrowCount;
            // FireFlurrySeekingArrow.baseArrowReloadDuration = AdditiveScaling(6, 2, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            // when the flurry crits it uses the following number of arrows
            Logger.Debug("critMaxArrowCount: {0}, critBaseArrowReloadDuration: {1}", FireFlurrySeekingArrow.critMaxArrowCount, FireFlurrySeekingArrow.critBaseArrowReloadDuration);

            FireFlurrySeekingArrow.critMaxArrowCount = AdditiveScaling(6, 2, level);
            if (characterBody.TryGetComponent(out HuntressTracker huntressTracker))
            {
                huntressTracker.maxTrackingDistance = MultScaling(60, 0.10f, level);
                huntressTracker.maxTrackingAngle = Math.Min(70, AdditiveScaling(30, 5, level)); // 16%
            }
            else
            {
                Logger.Warn("Could not locate the HuntressTracker component on {0}", characterBody);
            }
        }
    }

    [SkillLevelModifier("HuntressBodyGlaive", typeof(ThrowGlaive))]
    internal class HuntressGlaiveSkillModifier : SimpleSkillModifier<ThrowGlaive>
    {
        static HuntressGlaiveSkillModifier()
        {
            // R2API.LanguageAPI.Add("HUNTRESS_SECONDARY_DESCRIPTION", "Throw a seeking glaive that bounces up to <style=cIsDamage>6 (+2)</style> times for <style=cIsDamage>250% damage</style>. Damage increases by <style=cIsDamage>10% (+2.5%)</style> per bounce.");
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("OnSkillLeveledUp(level: {0})", level);
            Logger.Debug("Glaive stats - max bounces: {0}, damage coefficient: {1}, glaiveBounceRange: {2}", ThrowGlaive.maxBounceCount, ThrowGlaive.damageCoefficientPerBounce, ThrowGlaive.glaiveBounceRange);
            ThrowGlaive.maxBounceCount = AdditiveScaling(6, 1, level);
            ThrowGlaive.damageCoefficient = MultScaling(2.5f, 0.1f, level);
            ThrowGlaive.glaiveBounceRange = AdditiveScaling(35, 10, level);
            Logger.Debug("Glaive stats - max bounces: {0}, damage coefficient: {1}, glaiveBounceRange: {2}", ThrowGlaive.maxBounceCount, ThrowGlaive.damageCoefficientPerBounce, ThrowGlaive.glaiveBounceRange);
        }

    }

    [SkillLevelModifier(new[] { "HuntressBodyBlink", "HuntressBodyMiniBlink" }, typeof(BlinkState), typeof(MiniBlinkState))]
    internal class HuntressBlinkSkillModifier : BaseSkillModifier
    {
        public override void OnSkillExit(BaseState skillState, int level)
        {
            base.OnSkillExit(skillState, level);
            float duration = AdditiveScaling(0.0f, 1f, level);
            if (skillState is MiniBlinkState)
            {
                duration /= 2f;
            }
            if (duration > 0)
            {
                skillState.outer.commonComponents.characterBody?.AddTimedBuff(Buffs.FullCrit, duration);
            }
        }
    }

    [SkillLevelModifier(new[] { "HuntressBodyArrowRain", "Burning Rain" }, typeof(ArrowRain))]
    internal class HuntressArrowRainSkillModifier : SimpleSkillModifier<ArrowRain>
    {
        static HuntressArrowRainSkillModifier()
        {
            // R2API.LanguageAPI.Add("HUNTRESS_SPECIAL_DESCRIPTION", "<style=cIsUtility>Teleport</style> into the sky. Target a <style=cIsDamage>7.5 unit (+2.5)</style> radius area to rain arrows, <style=cIsUtility>slowing</style> all enemies and dealing <style=cIsDamage>225% (+%25) damage per second</style>.");
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("ArrowRain stats - arrowRainRadius: {0}, damageCoefficient: {1}, prefabScale {2}", ArrowRain.arrowRainRadius, ArrowRain.damageCoefficient, ArrowRain.projectilePrefab.transform.localScale);
            ArrowRain.arrowRainRadius = MultScaling(7.5f, 0.25f, level);
            ArrowRain.damageCoefficient = MultScaling(2.2f, 0.2f, level);

            ArrowRain.projectilePrefab.transform.localScale = Vector3.one * ArrowRain.arrowRainRadius * 2;
        }

    }

    [SkillLevelModifier(new[] { "AimArrowSnipe", "Rabauld" }, typeof(BeginArrowSnipe), typeof(FireArrowSnipe), typeof(AimArrowSnipe))]
    internal class HuntressSnipeSkillModifier : BaseSkillModifier
    {
        private static readonly float stockImageInterspacing = 18.0f;

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is AimArrowSnipe aimState)
            {
                aimState.maxDuration = AdditiveScaling(aimState.maxDuration, 0.5f, level);
            }
            if (skillState is FireArrowSnipe snipeState)
            {
                snipeState.damageCoefficient = MultScaling(snipeState.damageCoefficient, 0.2f, level);
                Logger.Debug("damageCoefficient: {0}", snipeState.damageCoefficient);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            int stocks = AdditiveScaling(3, 1, level);
            AimArrowSnipe.primarySkillDef.baseMaxStock = stocks;
            if (AimArrowSnipe.crosshairOverridePrefab.TryGetComponent(out CrosshairController crosshairController))
            {
                GameObject stockCountHolderGameObject = crosshairController.gameObject.transform.Find("StockCountHolder").gameObject;
                RectTransform stockCountHolderRectTransform = stockCountHolderGameObject.GetComponent<RectTransform>();

                List<GameObject> stockGameObjects = new List<GameObject>();
                GameObject stockPrefab = null;
                for (int i = 0; i < stockCountHolderGameObject.transform.childCount; i++)
                {
                    if (stockPrefab == null)
                    {
                        stockPrefab = stockCountHolderGameObject.transform.GetChild(i).gameObject;
                    }
                    stockGameObjects.Add(stockCountHolderGameObject.transform.GetChild(i).gameObject);
                }

                if (stockPrefab)
                {
                    while (stockGameObjects.Count < stocks)
                    {
                        GameObject newStock = GameObject.Instantiate(stockPrefab);
                        newStock.transform.parent = stockCountHolderGameObject.transform;
                        RectTransform stockRectTransform = newStock.GetComponent<RectTransform>();
                        stockRectTransform.localPosition = Vector2.zero;
                        stockRectTransform.anchorMin = Vector2.zero;
                        stockRectTransform.anchorMax = Vector2.zero;
                        stockRectTransform.offsetMin = new Vector2(4f, 0);
                        stockRectTransform.offsetMax = new Vector2(-4f, 0);
                        stockRectTransform.sizeDelta = new Vector2(-8f, 0);
                        stockRectTransform.ForceUpdateRectTransforms();
                        stockGameObjects.Add(newStock);
                        newStock.name = "Stock, " + stockGameObjects.Count;
                    }
                }

                stockCountHolderRectTransform.sizeDelta = new Vector2(stocks * stockImageInterspacing, 10);

                CrosshairController.SkillStockSpriteDisplay[] stockDisplays = new CrosshairController.SkillStockSpriteDisplay[stocks];
                float anchorIncrement = 1f / stocks;
                for (int i = 0; i < Math.Min(stocks, stockGameObjects.Count); i++)
                {
                    stockDisplays[i] = new CrosshairController.SkillStockSpriteDisplay()
                    {
                        target = stockGameObjects[i],
                        skillSlot = SkillSlot.Primary,
                        minimumStockCountToBeValid = i + 1,
                        maximumStockCountToBeValid = 999
                    };
                    RectTransform stockRectTransform = stockGameObjects[i].GetComponent<RectTransform>();
                    stockRectTransform.anchorMin = new Vector2(anchorIncrement * i, 0);
                    stockRectTransform.anchorMax = new Vector2(anchorIncrement * (i + 1), 1);
                    stockRectTransform.ForceUpdateRectTransforms();
                }

                crosshairController.skillStockSpriteDisplays = stockDisplays;

            }
        }

    }
}