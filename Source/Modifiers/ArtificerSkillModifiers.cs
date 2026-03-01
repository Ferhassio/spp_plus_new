using EntityStates;
using EntityStates.Mage;
using EntityStates.Mage.Weapon;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;

namespace SkillsPlusPlus.Modifiers
{

    [SkillLevelModifier("MageBodyFireFirebolt", typeof(FireFireBolt))]
    internal class ArtificerFireBoltSkillModifier : SimpleSkillModifier<FireFireBolt>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("FireFirebolt before - baseMaxStock: {0}, baseRechargeInterval: {1}", skillDef.baseMaxStock, skillDef.baseRechargeInterval);
            skillDef.baseMaxStock = AdditiveScaling(4, 2, level);
            skillDef.baseRechargeInterval = MultScaling(1.3f, -0.1f, level);
            Logger.Debug("FireFirebolt after - baseMaxStock: {0}, baseRechargeInterval: {1}", skillDef.baseMaxStock, skillDef.baseRechargeInterval);
        }

    }

    [SkillLevelModifier("MageBodyFireLightningBolt", typeof(FireLightningBolt))]
    internal class ArtificerLightningBoltSkillModifier : SimpleSkillModifier<FireLightningBolt>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("FireFirebolt before - baseMaxStock: {0}, baseRechargeInterval: {1}", skillDef.baseMaxStock, skillDef.baseRechargeInterval);
            skillDef.baseMaxStock = AdditiveScaling(4, 2, level);
            skillDef.baseRechargeInterval = MultScaling(1.3f, -0.1f, level);
            Logger.Debug("FireFirebolt after - baseMaxStock: {0}, baseRechargeInterval: {1}", skillDef.baseMaxStock, skillDef.baseRechargeInterval);
        }

    }

    [SkillLevelModifier("MageBodyNovaBomb", typeof(ChargeNovabomb), typeof(ThrowNovabomb))]
    internal class MageNovaBombSkillModifier : BaseSkillModifier
    {

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is ChargeNovabomb chargeBomb)
            {
                chargeBomb.baseDuration = MultScaling(chargeBomb.baseDuration, 0.1f, level);
            }
            if (skillState is ThrowNovabomb throwBomb)
            {
                if (throwBomb.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion))
                {
                    projectileImpactExplosion.blastRadius = MultScaling(14, 0.1f, level);
                    if (projectileImpactExplosion.impactEffect && projectileImpactExplosion.impactEffect.TryGetComponent(out EffectComponent effectComponent))
                    {
                        effectComponent.applyScale = true;
                    }
                    throwBomb.minDamageCoefficient = MultScaling(throwBomb.minDamageCoefficient, 0.2f, level);
                    throwBomb.maxDamageCoefficient = MultScaling(throwBomb.maxDamageCoefficient, 0.25f, level); // 30% to keep up with charge duration + 20% damage bonus
                    throwBomb.force = MultScaling(throwBomb.force, 0.2f, level);
                }
            }
        }
    }

    [SkillLevelModifier("MageBodyIceBomb", typeof(ChargeIcebomb), typeof(ThrowIcebomb))]
    internal class MageIceBombSkillModifier : BaseSkillModifier
    {

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is ChargeIcebomb chargeBomb)
            {
                // chargeBomb.baseDuration = MultScaling(chargeBomb.baseDuration, 0.1f, level);
            }
            if (skillState is ThrowIcebomb throwBomb)
            {
                throwBomb.minDamageCoefficient = MultScaling(throwBomb.minDamageCoefficient, 0.20f, level);
                throwBomb.maxDamageCoefficient = MultScaling(throwBomb.maxDamageCoefficient, 0.20f, level); // 30% to keep up with charge duration + 20% damage bonus
                throwBomb.force = MultScaling(throwBomb.force, 0.2f, level);
            }
        }

    }

    [SkillLevelModifier("MageBodyWall", typeof(PrepWall))]
    internal class MageWallSkillModifier : SimpleSkillModifier<PrepWall>
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (skillDef != characterBody.skillLocator.utility.skillDef) return;
            Logger.Debug("upgraded util");

            Logger.Debug("baseDuration: {0}, damageCoefficient: {1}", PrepWall.baseDuration, PrepWall.damageCoefficient);
            PrepWall.damageCoefficient = AdditiveScaling(1, 0.3f, level);

            if (PrepWall.projectilePrefab.TryGetComponent(out ProjectileMageFirewallWalkerController projectileMageFirewallWalkerController))
            {
                projectileMageFirewallWalkerController.totalProjectiles = (int)AdditiveScaling(6, 1, level);
                Logger.Debug("projectileMageFirewallWalkerController.totalProjectiles: {0}", projectileMageFirewallWalkerController.totalProjectiles);
                // this just like ,.., doesnt update the ones not newly added ?? for no reason ???? ive spent like 4 days rethinking this over just gonna remove it ,.,
                /*if (projectileMageFirewallWalkerController.firePillarPrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion))
                {
                    projectileImpactExplosion.lifetime = AdditiveScaling(8, 8f * 0.15f, level);
                    if (projectileImpactExplosion.TryGetComponent(out ProjectileController projectileController))
                    {
                        if (projectileController.ghostPrefab.transform.Find("Mesh").TryGetComponent(out ParticleSystem particleSystem))
                        {
                            ParticleSystem.MainModule main = particleSystem.main;

                            main.startLifetimeMultiplier = AdditiveScaling(8, 8f * 0.15f, level);
                            
                            particleSystem.startLifetime = AdditiveScaling(8, 8f * 0.15f, level);
                        }
                    }

                    pillar = projectileMageFirewallWalkerController.firePillarPrefab;
                }*/
            }

            float newXScale = AdditiveScaling(23.69f, 23.69f / 6f * 2, level);
            PrepWall.areaIndicatorPrefab.transform.localScale = new Vector3(newXScale, PrepWall.areaIndicatorPrefab.transform.localScale.y, PrepWall.areaIndicatorPrefab.transform.localScale.z);

            //PrepWall.projectilePrefab = PrepWall.projectilePrefab.gameObject.InstantiateClone(PrepWall.projectilePrefab.gameObject.name);
            PrepWall.areaIndicatorPrefab = PrepWall.areaIndicatorPrefab.gameObject.InstantiateClone(PrepWall.areaIndicatorPrefab.gameObject.name);

            Logger.Debug("baseDuration: {0}, damageCoefficient: {1}", PrepWall.baseDuration, PrepWall.damageCoefficient);
            Logger.Debug(PrepWall.projectilePrefab.name);
        }
    }

    [SkillLevelModifier(new string[] { "MageBodyFlamethrower", "Dragon's Breath" }, typeof(Flamethrower))]
    internal class MageFlamethrowerSkillModifier : SimpleSkillModifier<Flamethrower>
    {

        private float baseRadius = 2f;
        private float newRadius = 2f;
        private float totalDamageCoeff = 20;

        public override void OnSkillEnter(Flamethrower skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            Logger.Debug(skillState.maxDistance);
            skillState.maxDistance = MultScaling(skillState.maxDistance, 0.2f, level);

            Logger.Debug(skillState.flamethrowerEffectPrefab.transform.localScale);
            skillState.totalDamageCoefficient = totalDamageCoeff;
            skillState.flamethrowerEffectPrefab.transform.localScale = new Vector3(baseRadius, baseRadius, AdditiveScaling(1, 1, level));
            //if (skillState.flamethrowerEffectPrefab.TryGetComponent(out DestroyOnTimer timer)) {
            //    timer.duration = Flamethrower.baseFlamethrowerDuration;
            //}
            Logger.Debug(skillState.maxDistance);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("Flamethrower stats - totalDamageCoefficient: {0}, radius: {1}", totalDamageCoeff, baseRadius);
            baseRadius = MultScaling(2f, 0.25f, level);
            //Flamethrower.baseFlamethrowerDuration = AdditiveScaling(baseFlamethrowerDuration, 2, level);
            totalDamageCoeff = MultScaling(20, 0.20f, level);
            Logger.Debug("Flamethrower stats - totalDamageCoefficient: {0}, radius: {1}", totalDamageCoeff, baseRadius);
        }

    }

    [SkillLevelModifier(new string[] { "MageBodyFlyUp", "Antimatter Surge" }, typeof(FlyUpState))]
    internal class MageFlyUpSkillModifier : SimpleSkillModifier<FlyUpState>
    {

        public override void OnSkillEnter(FlyUpState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("FlyUpState stats - duration: {0}, blastAttackDamageCoefficient: {1}, blastAttackRadius: {2}, blastAttackProcCoefficient: {3}", FlyUpState.duration, FlyUpState.blastAttackDamageCoefficient, FlyUpState.blastAttackRadius, FlyUpState.blastAttackProcCoefficient);
            FlyUpState.blastAttackDamageCoefficient = MultScaling(8, 0.25f, level);
            FlyUpState.blastAttackRadius = MultScaling(8, 0.3f, level);
            Logger.Debug("FlyUpState stats - duration: {0}, blastAttackDamageCoefficient: {1}, blastAttackRadius: {2}, blastAttackProcCoefficient: {3}", FlyUpState.duration, FlyUpState.blastAttackDamageCoefficient, FlyUpState.blastAttackRadius, FlyUpState.blastAttackProcCoefficient);
        }

    }
}