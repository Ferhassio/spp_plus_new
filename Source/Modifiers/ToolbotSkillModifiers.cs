using System;
using System.Collections.Generic;

using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using RoR2.UI;

using EntityStates;
using EntityStates.Toolbot;
using R2API;
using R2API.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static RoR2.RoR2Content;

namespace SkillsPlusPlus.Modifiers
{

    [SkillLevelModifier("ToolbotBodyFireNailgun", typeof(FireNailgun))]
    internal class ToolbotSkillModifier : SimpleSkillModifier<FireNailgun>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            NailgunFinalBurst.finalBurstBulletCount = AdditiveScaling(12, 4, level);
            FireNailgun.damageCoefficient = MultScaling(0.7f, 0.20f, level);
            NailgunFinalBurst.damageCoefficient = MultScaling(0.7f, 0.20f, level);
            NailgunFinalBurst.burstTimeCostCoefficient = MultScaling(1.2f, -0.15f, level);
        }
    }

    [SkillLevelModifier("ToolbotBodyFireSpear", typeof(FireSpear), typeof(CooldownSpear))]
    internal class ToolbotSpearSkillModifier : BaseSkillModifier
    {

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("skillState: {0}", skillState);
            if (skillState is FireSpear fireSpear)
            {
                Logger.Debug("baseDuration: {0}", fireSpear.baseDuration);
                fireSpear.baseDuration = MultScaling(fireSpear.baseDuration, -0.15f, level);
                fireSpear.damageCoefficient = MultScaling(fireSpear.damageCoefficient, 0.10f, level);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            CooldownSpear.baseDuration = MultScaling(0.7f, -0.15f, level);
        }
    }

    [SkillLevelModifier("ToolbotBodyFireGrenadeLauncher", typeof(FireGrenadeLauncher))]
    internal class ToolbotGrenadeLauncherSkillModifier : SimpleSkillModifier<FireGrenadeLauncher>
    {

        private static readonly float stockImageInterspacing = 13.5f;

        public override void OnSkillEnter(FireGrenadeLauncher skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.20f, level);
            if (skillState.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion))
            {
                projectileImpactExplosion.blastRadius = MultScaling(7, 0.15f, level);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            int stocks = AdditiveScaling(4, 1, level);

            skillDef.baseMaxStock = stocks;

            if (skillDef is ToolbotWeaponSkillDef)
            {
                // return;
                // instantiate a new version of the prefab because it will cause the crosshair manager to recognize a new
                // prefab and reload the crosshair displayed on screen
                GameObject crosshairPrefab = ((ToolbotWeaponSkillDef)skillDef).crosshairPrefab;
                if (crosshairPrefab.TryGetComponent(out CrosshairController crosshairController))
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
                            stockRectTransform.rotation = Quaternion.Euler(0, 0, 270);
                            stockRectTransform.localPosition = Vector2.zero;
                            stockRectTransform.anchorMin = Vector2.zero;
                            stockRectTransform.anchorMax = Vector2.zero;
                            stockRectTransform.offsetMin = new Vector2(1.75f, 0);
                            stockRectTransform.offsetMax = new Vector2(-1.75f, 0);
                            stockRectTransform.sizeDelta = new Vector2(-3.5f, 0);
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

    [SkillLevelModifier("ToolbotBodyFireBuzzsaw", typeof(FireBuzzsaw))]
    internal class ToolbotBuzzsawSkillModifier : SimpleSkillModifier<FireBuzzsaw>
    {

        public override void OnSkillEnter(FireBuzzsaw fireBuzzsaw, int level)
        {
            base.OnSkillEnter(fireBuzzsaw, level);
            Transform modelTransform = fireBuzzsaw.outer?.commonComponents.modelLocator?.modelTransform;
            if (modelTransform)
            {
                HitBoxGroup buzzsawHitBoxGroup = Array.Find(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == "Buzzsaw");
                foreach (HitBox hitBox in buzzsawHitBoxGroup.hitBoxes)
                {
                    hitBox.transform.localScale = new Vector3(21.04f, 21.04f, 12.04f) * MultScaling(1, 0.3f, level);
                }
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("baseFireFrequency: {0}", FireBuzzsaw.baseFireFrequency);
            FireBuzzsaw.damageCoefficientPerSecond = MultScaling(10, 0.20f, level);
            FireBuzzsaw.baseFireFrequency = MultScaling(10, 0.20f, level);
        }
    }

    [SkillLevelModifier("ToolbotBodyStunDrone", typeof(AimStunDrone))]
    internal class ToolbotStunDroneSkillModifier : SimpleSkillModifier<AimStunDrone>
    {

        public override void OnSkillEnter(AimStunDrone aimStunDrone, int level)
        {
            base.OnSkillEnter(aimStunDrone, level);
            Logger.Debug("damageCoefficient: {0}, baseMinimumDuration: {1}, maxDistance: {2}", aimStunDrone.damageCoefficient, aimStunDrone.baseMinimumDuration, aimStunDrone.maxDistance);
            aimStunDrone.maxDistance = MultScaling(aimStunDrone.maxDistance, 0.4f, level); // 2 * 20%
            if (aimStunDrone.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion))
            {
                projectileImpactExplosion.blastRadius = MultScaling(12f, 0.20f, level);
                projectileImpactExplosion.childrenCount = AdditiveScaling(5, 3, level);
                if (projectileImpactExplosion.childrenProjectilePrefab.TryGetComponent(out ProjectileSimple projectileSimple))
                {
                    projectileSimple.desiredForwardSpeed = MultScaling(10, 0.4f, level); // 2 * 20%
                }
                if (projectileImpactExplosion.childrenProjectilePrefab.TryGetComponent(out ProjectileImpactExplosion clusterProjectileExplosion))
                {
                    clusterProjectileExplosion.blastRadius = MultScaling(6, 0.20f, level);
                    clusterProjectileExplosion.blastDamageCoefficient = MultScaling(1, 0.20f, level);
                }
            }
        }
    }

    [SkillLevelModifier(new string[] { "ToolbotBodyToolbotDash", "Breach Mode" }, typeof(ToolbotDash))]
    internal class ToolbotDashSkillModifier : SimpleSkillModifier<ToolbotDash>
    {

        public override void OnSkillEnter(ToolbotDash toolbotDash, int level)
        {
            base.OnSkillEnter(toolbotDash, level);
            Logger.Debug("baseDuration: {0}, speedMultiplier: {2}, chargeDamageCoefficient: {5}, knockbackForce: {1}, knockbackDamageCoefficient: {3}, massThresholdForKnockback: {4}", toolbotDash.baseDuration, ToolbotDash.knockbackForce, toolbotDash.speedMultiplier, ToolbotDash.knockbackDamageCoefficient, ToolbotDash.massThresholdForKnockback, ToolbotDash.chargeDamageCoefficient);
            // baseDuration: 2, speedMultiplier: 2.2, knockbackForce: 8000, knockbackDamageCoefficient: 10, massThresholdForKnockback: 250
            toolbotDash.baseDuration = MultScaling(toolbotDash.baseDuration, 0.10f, level);
            toolbotDash.speedMultiplier = MultScaling(2.2f, 0.10f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            ToolbotDash.chargeDamageCoefficient = MultScaling(2.5f, 0.30f, level);
            ToolbotDash.knockbackDamageCoefficient = MultScaling(10, 0.30f, level);
            ToolbotDash.massThresholdForKnockback = MultScaling(250, 0.3f, level);
            ToolbotDash.knockbackForce = MultScaling(8000, 0.15f, level);
        }
    }

    [SkillLevelModifier("ToolbotBodySwap", typeof(ToolbotStanceSwap))]
    internal class ToolbotEquipmentSwapSkillModifier : SimpleSkillModifier<ToolbotStanceSwap>
    {

        public override void OnSkillEnter(ToolbotStanceSwap toolbotStanceSwap, int level)
        {
            base.OnSkillEnter(toolbotStanceSwap, level);
            Logger.Debug("baseDuration: {0}", toolbotStanceSwap.GetFieldValue<float>("baseDuration"));
            // toolbotStanceSwap.SetFieldValue("baseDuration", MultScaling(0.4f, -0.25f, level));
        }

        public override void OnSkillExit(ToolbotStanceSwap skillState, int level)
        {
            base.OnSkillExit(skillState, level);
            float duration = AdditiveScaling(0, 1, level);
            if (duration > 0)
            {
                skillState.outer.commonComponents.characterBody.AddTimedBuff(Buffs.Energized, AdditiveScaling(0, 1, level));
            }
            skillState.outer.commonComponents.characterBody.inventory.DeductActiveEquipmentCooldown(level);
        }
    }

    [SkillLevelModifier("ToolbotDualWield", typeof(ToolbotDualWield))]
    internal class ToolbotPowerModeSkillModifier : SimpleSkillModifier<ToolbotDualWield>
    {

        public override void OnSkillEnter(ToolbotDualWield toolbotStanceSwap, int level)
        {
            base.OnSkillEnter(toolbotStanceSwap, level);
            if (armorBonus > 0)
            {
                Logger.Debug("enter dualwield");
                toolbotStanceSwap.characterBody.SetBuffCount(dualWieldArmorBuff.buffIndex, 1);
            }
        }
        public override void OnSkillExit(ToolbotDualWield skillState, int level)
        {
            base.OnSkillExit(skillState, level);
            Logger.Debug("exit dualwield");
            skillState.characterBody.SetBuffCount(dualWieldArmorBuff.buffIndex, 0);
        }

        private static float armorBonus = 0f;
        private static float damageBonus = 0f;
        private static float moveSpeedBonus = 0f;
        private static BuffDef dualWieldArmorBuff;



        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            armorBonus = AdditiveScaling(0, 25, level);
            damageBonus = AdditiveScaling(0, 0.15f, level);
            moveSpeedBonus = AdditiveScaling(0, 1f, level);
            Logger.Debug("leveled up dualwield");
        }

        public override void SetupSkill()
        {
            base.SetupSkill();
            registerDualWieldBuff();

            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPIOnGetStatCoefficients;
        }

        private void registerDualWieldBuff()
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();

            buffDef.buffColor = new Color(0.74f, 0.36f, 0.23f);
            buffDef.canStack = false;
            buffDef.eliteDef = null;
            buffDef.iconSprite = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Common/bdArmorBoost.asset").WaitForCompletion().iconSprite;
            buffDef.isDebuff = true;
            buffDef.name = "DualWieldArmorBuff";

            dualWieldArmorBuff = buffDef;
            ContentAddition.AddBuffDef(buffDef);
        }

        private void RecalculateStatsAPIOnGetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (sender.HasBuff(dualWieldArmorBuff))
            {
                args.damageMultAdd += damageBonus;
                args.armorAdd += armorBonus;
                args.moveSpeedMultAdd += 0.2f + (0.7f - 0.3f) * (1f - 1f / (1f + 0.6f * (moveSpeedBonus - 1f))); //i hate hyperbolic </3 
            }
        }
    }
}
