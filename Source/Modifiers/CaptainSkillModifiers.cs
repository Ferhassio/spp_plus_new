using UnityEngine;

using EntityStates;
using EntityStates.Captain.Weapon;
using EntityStates.CaptainSupplyDrop;

using RoR2;
using RoR2.Projectile;
using RoR2.Skills;

using static R2API.RecalculateStatsAPI;

namespace SkillsPlusPlus.Modifiers
{

    [SkillLevelModifier("CaptainShotgun", typeof(FireCaptainShotgun), typeof(ChargeCaptainShotgun))]
    internal class CaptainShotgunSkillModifier : BaseSkillModifier
    {

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is FireCaptainShotgun fireshotgun)
            {
                fireshotgun.bulletCount = (int)MultScaling(fireshotgun.bulletCount, 0.2f, level);
                fireshotgun.damageCoefficient = MultScaling(fireshotgun.damageCoefficient, 0.1f, level);
            }
        }
    }

    [SkillLevelModifier("CaptainTazer", typeof(FireTazer))]
    internal class CaptainTaserSkillModifier : SimpleSkillModifier<FireTazer>
    {

        public override void OnSkillEnter(FireTazer skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            if (FireTazer.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion))
            {
                projectileImpactExplosion.blastRadius = MultScaling(2, 0.4f, level);
                projectileImpactExplosion.blastDamageCoefficient = MultScaling(1, 0.2f, level);
            }
        }

    }

    [SkillLevelModifier(new string[] { "PrepAirstrike", "21-Probe Salute" }, typeof(CallAirstrike1), typeof(CallAirstrike2), typeof(CallAirstrike3))]
    internal class CaptainAirstrikeSkillModifier : SimpleSkillModifier<CallAirstrikeBase>
    {

        public override void OnSkillEnter(CallAirstrikeBase skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            var radius = MultScaling(8, 0.2f, level);
            if (skillState.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion))
            {
                projectileImpactExplosion.blastRadius = radius;
                projectileImpactExplosion.blastDamageCoefficient = MultScaling(1, 0.2f, level);
            }
            if (skillState.projectilePrefab.TryGetComponent(out ProjectileController projectileController))
            {
                var expanderTransform = projectileController.ghostPrefab.transform.Find("Expander");
                if (expanderTransform != null)
                {
                    expanderTransform.localScale = Vector3.one * radius;
                }
            }
        }
    }

    [SkillLevelModifier(new string[] { "PrepAirstrikeAlt" }, typeof(CallAirstrikeAlt), typeof(SetupAirstrikeAlt))]
    internal class CaptainDiabloStrikeSkillModifier : BaseSkillModifier
    {
        private static int diabloStrikeProjectileCatalogIndex = -1337;
        private static float fuseDuration;
        private static SkillUpgrade diabloSkill;

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (!diabloSkill)
            {
                diabloSkill = registeredSkill;
            }

            fuseDuration = Mathf.Clamp(20f - (level), 0f, 20f);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            //Try and update the speed of the Indicator
            if (skillState is CallAirstrikeAlt callAirstrikeAlt && callAirstrikeAlt.projectilePrefab.TryGetComponent(out ProjectileController projectileController) && registeredSkill != null)
            {

                var CenterTransform = projectileController.ghostPrefab.transform.Find("AreaIndicatorCenter");
                if (CenterTransform != null)
                {
                    for (int i = 0; i < CenterTransform.childCount; i++)
                    {
                        var child = CenterTransform.GetChild(i);
                        if (child != null && child.gameObject != null)
                        {
                            //Update the Ring Animation
                            if (child.TryGetComponent(out ObjectScaleCurve scaleCurve))
                            {
                                scaleCurve.timeMax = fuseDuration;
                            }
                            else
                            {
                                //Dodge Rings and update the Lasers
                                var vertical = child.Find("LaserVerticalOffset");
                                if (vertical)
                                {
                                    var laser = vertical.Find("Laser");
                                    if (laser)
                                    {
                                        if (laser != null && laser.gameObject.TryGetComponent(out ObjectTransformCurve laserCurve))
                                        {
                                            laserCurve.timeMax = fuseDuration;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public override void SetupSkill()
        {
            GetStatCoefficients += RecalculateStats;

            On.EntityStates.AimThrowableBase.ModifyProjectile += AimThrowableBase_ModifyProjectile;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;

            base.SetupSkill();
        }

        private void RecalculateStats(CharacterBody sender, StatHookEventArgs args)
        {
            if (!sender || !diabloSkill)
            {
                return;
            }

            if (sender == PlayerCharacterMasterController.instances[0].master.GetBody())
            {
                args.utilityCooldownMultAdd -= 1 - (5f / (diabloSkill.skillLevel + 5f));
            }
        }

        public static void AimThrowableBase_ModifyProjectile(On.EntityStates.AimThrowableBase.orig_ModifyProjectile orig, EntityStates.AimThrowableBase self, ref RoR2.Projectile.FireProjectileInfo fireProjectileInfo)
        {
            orig(self, ref fireProjectileInfo);

            if (self is CallAirstrikeAlt && diabloSkill != null)
            {
                fireProjectileInfo.useFuseOverride = true;
                fireProjectileInfo.fuseOverride = fuseDuration;
            }
        }

        public static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo di)
        {
            if (diabloSkill != null)
            {
                if (di != null && self != null)
                {
                    if (diabloStrikeProjectileCatalogIndex == -1337)
                    {
                        diabloStrikeProjectileCatalogIndex = ProjectileCatalog.FindProjectileIndex("CaptainAirstrikeAltProjectile");
                    }

                    if (di.inflictor && di.inflictor.TryGetComponent(out ProjectileController controller) && controller.catalogIndex == diabloStrikeProjectileCatalogIndex)
                    {
                        if (di.attacker && di.attacker.TryGetComponent(out CharacterBody attackerBody))
                        {
                            if (self?.body?.teamComponent?.teamIndex == attackerBody?.teamComponent?.teamIndex && diabloSkill.skillLevel > 0)
                            {
                                var friendlyFireProtection = (25 / ((float)diabloSkill.skillLevel + 25));
                                if (self.combinedHealthFraction > friendlyFireProtection)
                                {
                                    di.damage = (self.combinedHealth - (diabloSkill.skillLevel * 0.04f * self.fullCombinedHealth)) * 2;    //Friendly fire deals half damage
                                    di.damageType |= DamageType.BypassArmor;
                                    di.damageType &= ~DamageType.AOE;
                                }
                            }
                        }
                    }
                }
            }
            orig.Invoke(self, di);
        }
    }

    [SkillLevelModifier(new string[] {
        "PrepSupplyDrop", 
        //"CaptainSkillUsedUp", 
        //"CaptainSupplyDropDepleted", 
        //"CaptainSupplyDropHealing", 
        //"CaptainSupplyDropPlating",
        //"CaptainSupplyDropEquipmentRestock",
        //"CaptainSupplyDropHacking"
    }, typeof(SetupSupplyDrop), typeof(DeployState), typeof(HealZoneMainState), typeof(ShockZoneMainState), typeof(HackingMainState), typeof(HackingInProgressState), typeof(EquipmentRestockMainState))]
    internal class CaptainSupplyDropHealingSkillModifier : BaseSkillModifier
    {

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is DeployState deploying)
            {
                var modelLocator = deploying.outer?.commonComponents.modelLocator;
                if (modelLocator != null)
                {
                    var indicatorTransform = modelLocator.modelTransform?.Find("Indicator");
                    if (indicatorTransform != null)
                    {
                        indicatorTransform.localScale = Vector3.one * HackingMainState.baseRadius / 2;
                        if (indicatorTransform.TryGetComponent(out ObjectScaleCurve objectScaleCurve))
                        {
                            objectScaleCurve.baseScale = indicatorTransform.localScale;
                        }
                    }
                }
            }

            if (skillState is EquipmentRestockMainState equipmentRestock)
            {
                equipmentRestock.activationCost = 100 / AdditiveScaling(3, 1, level);
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            if (HealZoneMainState.healZonePrefab.TryGetComponent(out HealingWard healingWard))
            {
                var healRadius = MultScaling(10, 0.2f, level);
                healingWard.radius = healRadius;
            }
            ShockZoneMainState.shockRadius = MultScaling(10, 0.3f, level);

            HackingMainState.baseRadius = MultScaling(10, 0.2f, level);
            HackingInProgressState.baseDuration = MultScaling(15, -0.2f, level);

            var warningZone = SetupSupplyDrop.blueprintPrefab.transform.Find("Base")?.Find("Warning");
            if (warningZone)
            {
                warningZone.localScale = new Vector3(HackingMainState.baseRadius / 2, HackingMainState.baseRadius / 2, warningZone.localScale.z);
            }
        }

    }
}
