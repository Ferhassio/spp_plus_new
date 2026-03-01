using System.Collections.Generic;
using UnityEngine;

using RoR2;
using RoR2.CharacterAI;

using EntityStates;
using EntityStates.Engi.EngiWeapon;
using EntityStates.Engi.EngiBubbleShield;
using EntityStates.Engi.EngiMissilePainter;
using EntityStates.Engi.Mine;
using EntityStates.EngiTurret.EngiTurretWeapon;

using RoR2.Projectile;
using RoR2.Skills;

using System.Linq;

namespace SkillsPlusPlus.Modifiers
{
    internal sealed class EngiSkillModifier
    {

        public static Dictionary<DeployableSlot, int> deployableSlotCountBonus = new Dictionary<DeployableSlot, int>();

        public static int GetDeployableSameSlotBonus(DeployableSlot slot)
        {
            if (deployableSlotCountBonus.TryGetValue(slot, out int bonusCount))
            {
                return bonusCount;
            }
            return 0;
        }

    }

    [SkillLevelModifier("EngiBodyFireGrenade", typeof(ChargeGrenades))]
    internal class EngiGrenadesSkillModifier : SimpleSkillModifier<ChargeGrenades>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            ChargeGrenades.minGrenadeCount = AdditiveScaling(2, 1, level);
            ChargeGrenades.maxGrenadeCount = AdditiveScaling(8, 4, level);
        }

    }

    [SkillLevelModifier("EngiBodyPlaceMine", typeof(FireMines), typeof(MineArmingFull), typeof(MineArmingWeak))]
    internal class EngiMineSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            MineArmingWeak.duration = MultScaling(3, -0.10f, level);
            Logger.Debug("MineArmingWeak.duration: {0}", MineArmingWeak.duration);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            if (skillState is FireMines)
            {
                this.OnFireMinesEnter((FireMines)skillState, level);
            }
            else if (skillState is BaseMineArmingState)
            {
                this.OnMinesArmedEnter((BaseMineArmingState)skillState, level);
            }
        }

        public void OnFireMinesEnter(FireMines skillState, int level)
        {

        }

        public void OnMinesArmedEnter(BaseMineArmingState skillState, int level)
        {
            Logger.Debug("triggerRadius: {0}, blastRadiusScale: {1}, force: {2}, damageScale: {3}", skillState.triggerRadius, skillState.blastRadiusScale, skillState.forceScale, skillState.damageScale);

            if (skillState is MineArmingFull)
            {
                skillState.forceScale = MultScaling(3, 0.20f, level);
                skillState.damageScale = MultScaling(3, 0.20f, level);
                skillState.blastRadiusScale = MultScaling(2, 0.20f, level);
                skillState.triggerRadius = MultScaling(8, 0.20f, level);

                Transform strongIndicator = skillState.outer.commonComponents.transform.Find(skillState.pathToChildToEnable);
                if (strongIndicator.gameObject.TryGetComponentInChildren(out ObjectScaleCurve objectScaleCurve))
                {
                    Vector3 scale = Vector3.one * 2 * skillState.triggerRadius;
                    objectScaleCurve.gameObject.transform.localScale = scale;
                    objectScaleCurve.baseScale = scale;
                    Logger.Debug("objectScaleCurve.baseScale: {0}", objectScaleCurve.baseScale);
                }
            }
            else if (skillState is MineArmingWeak)
            {
                skillState.forceScale = MultScaling(1, 0.20f, level);
                skillState.damageScale = MultScaling(1, 0.20f, level);
                skillState.blastRadiusScale = MultScaling(0.2f, 0.20f, level);
                skillState.triggerRadius = MultScaling(4, 0.20f, level);
            }
            Logger.Debug("triggerRadius: {0}, blastRadiusScale: {1}, force: {2}, damageScale: {3}", skillState.triggerRadius, skillState.blastRadiusScale, skillState.forceScale, skillState.damageScale);
        }
    }

    [SkillLevelModifier("EngiBodyPlaceSpiderMine", typeof(FireSpiderMine), typeof(EntityStates.Engi.SpiderMine.WaitForTarget))]
    internal class EngiSpiderMineSkillModifier : BaseSkillModifier
    {

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is EntityStates.Engi.SpiderMine.WaitForTarget)
            {
                OnBurrowEnter((EntityStates.Engi.SpiderMine.WaitForTarget)skillState, level);
            }
            else if (skillState is FireSpiderMine)
            {
                OnFireSpiderMineEnter((FireSpiderMine)skillState, level);
            }
        }

        private void OnBurrowEnter(EntityStates.Engi.SpiderMine.WaitForTarget waitForTarget, int level)
        {
            if (waitForTarget.outer.TryGetComponent(out ProjectileSphereTargetFinder targetFinder))
            {
                targetFinder.lookRange = MultScaling(25, 0.30f, level);
            }
            else
            {
                ReportBroken("ProjectileSphereTargetFinder component in EntityStates.Engi.SpiderMine.WaitForTarget");
            }
        }

        private void OnFireSpiderMineEnter(FireSpiderMine fireSpiderMine, int level)
        {
            Logger.Debug("damageCoefficient: {0}", fireSpiderMine.damageCoefficient);
            fireSpiderMine.damageCoefficient = MultScaling(6, 0.20f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("blastRadius: {0}", EntityStates.Engi.SpiderMine.Detonate.blastRadius);
            skillDef.baseMaxStock = AdditiveScaling(4, 1, level);
            EntityStates.Engi.SpiderMine.Detonate.blastRadius = MultScaling(14, 0.25f, level);
        }
    }

    [SkillLevelModifier("EngiBodyPlaceBubbleShield", typeof(FireBubbleShield))]
    internal class EngiBubbleShieldSkillModifier : SimpleSkillModifier<FireBubbleShield>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Deployed.lifetime = MultScaling(20, 0.15f, level);
        }

        public override void OnSkillEnter(FireBubbleShield skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            float bubbleSize = MultScaling(20, 0.15f, level);
            // float shieldDuration = MultScaling();

            if (skillState.projectilePrefab.TryGetComponent(out ChildLocator childLocator))
            {
                GameObject bubbleGameObject = childLocator.FindChild(Deployed.childLocatorString).gameObject;
                if (bubbleGameObject)
                {
                    bubbleGameObject.transform.localScale = Vector3.one * bubbleSize;
                }
            }
        }
    }

    [SkillLevelModifier("EngiHarpoons", typeof(Paint))]
    internal class EngiHarpoonsSkillModifier : SimpleSkillModifier<Paint>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            skillDef.baseMaxStock = AdditiveScaling(4, 1, level);
            Logger.Debug("paintInterval: {0}", Paint.stackInterval);
            Logger.Debug("baseDurationPerMissile: {0}, damageCoefficient: {1}", Fire.baseDurationPerMissile, Fire.damageCoefficient);
            Fire.damageCoefficient = MultScaling(5, 0.20f, level);
            Paint.maxDistance = MultScaling(150, 0.30f, level);
        }
    }

    [SkillLevelModifier(new string[] { "EngiBodyPlaceTurret", "TR12-C Gauss Compact" }, typeof(PlaceTurret), typeof(FireGauss))]
    internal class EngiTurretSkillModifier : BaseSkillModifier
    {

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is PlaceTurret)
            {

            }
            else if (skillState is FireGauss)
            {
                OnFireGaussEnter((FireGauss)skillState, level);
            }
        }

        private void OnFireGaussEnter(FireGauss skillState, int level)
        {
            // skillState.bullea
            Logger.Debug("damageCoefficient: {0}, bulletCount: {1}", FireGauss.damageCoefficient, FireGauss.bulletCount);
            if (skillState.outer.commonComponents.characterBody.master.TryGetComponent(out BaseAI baseAI))
            {
                AISkillDriver skillDriver = baseAI.skillDrivers.FirstOrDefault(it => { return it.customName == "FireAtEnemy"; });
                if (skillDriver != null)
                {
                    skillDriver.maxDistance = MultScaling(60, 0.25f, level);
                }
            }
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            // an extra turret every two levels
            skillDef.baseMaxStock = (int)AdditiveScaling(2, 0.5f, level);
            FireGauss.damageCoefficient = MultScaling(0.7f, 0.1f, level);
            EngiSkillModifier.deployableSlotCountBonus[DeployableSlot.EngiTurret] = (int)AdditiveScaling(0, 0.5f, level);
        }

    }

    [SkillLevelModifier(new string[] { "EngiBodyPlaceWalkerTurret", "TR58-C Carbonizer Mini" }, typeof(PlaceWalkerTurret), typeof(FireBeam))]
    internal class EngiWalkerTurretSkillModifier : BaseSkillModifier
    {

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is PlaceWalkerTurret)
            {

            }
            else if (skillState is FireBeam)
            {
                OnFireBeamEnter((FireBeam)skillState, level);
            }
        }

        private void OnFireBeamEnter(FireBeam fireBeam, int level)
        {
            Logger.Debug("damageCoefficient: {0}, fireFrequency: {1}, procCoefficient: {2}", fireBeam.damageCoefficient, fireBeam.fireFrequency, fireBeam.procCoefficient);
            fireBeam.damageCoefficient = MultScaling(2, 0.15f, level);
            fireBeam.procCoefficient = MultScaling(3, 0.25f, level);
            fireBeam.fireFrequency = MultScaling(5, 0.10f, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            skillDef.baseMaxStock = (int)AdditiveScaling(2, 0.5f, level);
            EngiSkillModifier.deployableSlotCountBonus[DeployableSlot.EngiTurret] = (int)AdditiveScaling(0, 0.5f, level);
        }
    }
}
