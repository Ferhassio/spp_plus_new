using EntityStates.Commando;
using EntityStates.Commando.CommandoWeapon;
using R2API;
using RoR2;
using RoR2.Projectile;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static R2API.RecalculateStatsAPI;
using static RoR2.RoR2Content;

namespace SkillsPlusPlus.Modifiers
{

    [SkillLevelModifier("CommandoBodyFirePistol", typeof(FirePistol2))]
    internal class CommandoFirePistolSkillModifier : SimpleSkillModifier<FirePistol2>
    {
        private float originalDuration = 0;
        private float originalRecoil = 0;
        private float originalSpread = 0;
        public override void OnSkillEnter(FirePistol2 skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);

            if (Mathf.Abs(originalDuration) < 0.01f)
            {
                originalDuration = FirePistol2.baseDuration;
                originalRecoil = FirePistol2.recoilAmplitude;
                originalSpread = FirePistol2.spreadBloomValue;
            }

            FirePistol2.baseDuration = MultScaling(originalDuration, -0.20f, level);
            FirePistol2.recoilAmplitude = MultScaling(originalRecoil, -0.1f, level);
            FirePistol2.spreadBloomValue = MultScaling(originalSpread, -0.1f, level);
        }

    }

    [SkillLevelModifier("CommandoBodyFireFMJ", typeof(FireFMJ))]
    internal class CommandoFMJSkillModifier : SimpleSkillModifier<FireFMJ>
    {
        private float originalForwardSpeed = 0;
        public override void OnSkillEnter(FireFMJ skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("recoilAmplitude: {0},s damageCoefficient: {1}", skillState.recoilAmplitude, skillState.damageCoefficient);
            skillState.projectilePrefab.transform.localScale = new Vector3(2.90f, 2.19f, 3.86f) * AdditiveScaling(1, 0.2f, level);
            if (skillState.projectilePrefab.TryGetComponent(out ProjectileSimple projectileSimple))
            {

                if (Mathf.Abs(originalForwardSpeed) < 0.01f)
                {
                    originalForwardSpeed = projectileSimple.desiredForwardSpeed;
                }

                projectileSimple.desiredForwardSpeed = MultScaling(originalForwardSpeed, 0.3f, level);
            }
            skillState.recoilAmplitude = MultScaling(skillState.recoilAmplitude, 0.1f, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.3f, level);
        }

    }

    [SkillLevelModifier("CommandoBodyFireShotgunBlast", typeof(FireShotgunBlast))]
    internal class CommandoShotgunBlastSkillModifier : SimpleSkillModifier<FireShotgunBlast>
    {

        public override void OnSkillEnter(FireShotgunBlast skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("procCoefficient: {0}, damageCoefficient: {1}, maxDistance: {2}", skillState.procCoefficient, skillState.damageCoefficient, skillState.maxDistance);
            skillState.bulletCount = (int)MultScaling(skillState.bulletCount, 0.3f, level);
            skillState.maxDistance = MultScaling(skillState.maxDistance, 0.20f, level);
        }
    }

    [SkillLevelModifier("CommandoBodyRoll", typeof(DodgeState))]
    internal class CommandoRollSkillModifier : SimpleSkillModifier<DodgeState>
    {

        public override void OnSkillEnter(DodgeState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            float duration = AdditiveScaling(0, 0.75f, level);
            if (duration > 0)
            {
                Logger.Debug("adding buff for {0} seconds", duration);
                skillState.outer.commonComponents.characterBody.AddTimedBuff(Buffs.Immune, duration);
            }
        }
    }

    [SkillLevelModifier("CommandoSlide", typeof(SlideState))]
    internal class CommandoDiveSkillModifier : SimpleSkillModifier<SlideState>
    {
        public float baseSlideDuration = 0f;
        public float baseJumpDuration = 0f;
        private static BuffDef CommandoSlideBuff;

        public static void RegisterCommandoSlideBuff()
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();

            buffDef.buffColor = new Color(0.78f, 0.5f, 0);
            buffDef.buffIndex = (BuffIndex)64;
            buffDef.canStack = true;
            buffDef.eliteDef = null;
            buffDef.iconSprite = Addressables.LoadAssetAsync<BuffDef>("RoR2/Base/Bandit2/bdCloakSpeed.asset").WaitForCompletion().iconSprite;
            buffDef.isDebuff = false;
            buffDef.name = "CommandoSlideBuff";

            CommandoSlideBuff = buffDef;
            ContentAddition.AddBuffDef(buffDef);
        }

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            if (Mathf.Abs(baseSlideDuration) < 0.0001f)
            {
                baseSlideDuration = SlideState.slideDuration;
                baseJumpDuration = SlideState.jumpDuration;
            }
            SlideState.slideDuration = baseSlideDuration + (0.125f * level);
            SlideState.jumpDuration = baseJumpDuration + (0.125f * level);
        }
        public override void OnSkillEnter(SlideState skillState, int level)
        {
            if (level > 0)
            {
                skillState.outer.commonComponents.characterBody.SetBuffCount(CommandoSlideBuff.buffIndex, level);
            }
            base.OnSkillEnter(skillState, level);
        }

        public override void OnSkillExit(SlideState skillState, int level)
        {
            skillState.outer.commonComponents.characterBody.SetBuffCount(CommandoSlideBuff.buffIndex, 0);
            base.OnSkillExit(skillState, level);
        }

        public static void RecalculateStats(CharacterBody sender, StatHookEventArgs args)
        {
            if (!sender)
            {
                return;
            }

            if (sender.HasBuff(CommandoSlideBuff))
            {
                int buffLevel = sender.GetBuffCount(CommandoSlideBuff);
                float speedDrop = sender.moveSpeed * (1 - (10.0f / (10.0f + (float)buffLevel)));
                args.baseMoveSpeedAdd -= speedDrop;

                args.baseAttackSpeedAdd += speedDrop;
                args.baseDamageAdd += speedDrop;
                args.armorAdd += speedDrop * 10;
            }
        }

        public override void SetupSkill()
        {
            base.SetupSkill();

            RegisterCommandoSlideBuff();

            GetStatCoefficients += RecalculateStats;
        }
    }

    [SkillLevelModifier(new string[] { "CommandoBodyBarrage", "Death Blossom" }, typeof(FireBarrage))]
    internal class CommandoBarrageSkillModifier : SimpleSkillModifier<FireBarrage>
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
            Logger.Debug("Barrage - baseBulletCount: {0}, baseDurationBetweenShots: {1}, totalDuration: {2}, bulletRadius: {3}", FireBarrage.baseBulletCount, FireBarrage.baseDurationBetweenShots, FireBarrage.totalDuration, FireBarrage.bulletRadius);

            FireBarrage.baseBulletCount = (int)MultScaling(6, 0.3f, level);
            FireBarrage.baseDurationBetweenShots = MultScaling(0.12f, -0.20f, level);
            FireBarrage.totalDuration = FireBarrage.baseBulletCount * FireBarrage.baseDurationBetweenShots + 0.3f;
            // FireBarrage.baseDurationBetweenShots = AdditiveScaling(0.12f, -0.01f, level);
        }

    }

    [SkillLevelModifier(new string[] { "ThrowGrenade", "Carpet Bomb" }, typeof(ThrowGrenade))]
    internal class CommandoGrenadeSkillModifier : SimpleSkillModifier<ThrowGrenade>
    {

        public override void OnSkillEnter(ThrowGrenade skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            Logger.Debug("force: {0}, damageCoefficient: {1}", skillState.force, skillState.damageCoefficient);
            skillState.force = MultScaling(skillState.force, 0.2f, level);
            skillState.damageCoefficient = MultScaling(skillState.damageCoefficient, 0.2f, level);
            if (skillState.projectilePrefab.TryGetComponent(out ProjectileImpactExplosion projectileImpactExplosion))
            {
                projectileImpactExplosion.blastRadius = MultScaling(11, 0.20f, level);
            }
        }
    }
}