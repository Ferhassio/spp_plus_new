using System;
using EntityStates;
using EntityStates.Drifter;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using On.EntityStates.FalseSonBoss;
using On.RoR2.Projectile;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;
using UnityEngine;

namespace SkillsPlusPlus.Source.Modifiers
{
    /*[SkillLevelModifier(new [] {"BluntForce", "Bludgeon"}, typeof(BluntForceBase), typeof(BluntForceEntry), typeof(BluntForceHit1), typeof(BluntForceHit2), typeof(BluntForceHit3), typeof(BluntForceTornado))]
    class DrifterBluntForceSkillModifier : BaseSkillModifier
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is BluntForceBase)
            {
                Logger.Debug("BluntForceBase");
            }
            switch (skillState)
            {
                case BluntForceHit1 _:
                    Logger.Debug("BluntForceHit1");
                    break;
                case BluntForceHit2 _:
                    Logger.Debug("BluntForceHit2");
                    break;
                case BluntForceHit3 _:
                    Logger.Debug("BluntForceHit3");
                    break;
            }
        }
    }

    [SkillLevelModifier("Cleanup", typeof(Cleanup))]
    class DrifterCleanupSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is Cleanup)
            {
                Logger.Debug("Cleanup");
            }
        }
    }

    [SkillLevelModifier("JunkCube", typeof(JunkCube))]
    class DrifterJunkCubeSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is JunkCube)
            {
                Logger.Debug("JunkCube");
            }
        }
    }

    [SkillLevelModifier(new string[] {"Repossess", "Discard"}, typeof(Repossess), typeof(AimRepossess), typeof(RepossessBullseyeSearch))]
    class DrifterRepossessSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is Repossess)
            {
                Logger.Debug("Repossess");
            } 
            else if (skillState is AimRepossess)
            {
                Logger.Debug("AimRepossess");
            } 
        }
    }

    [SkillLevelModifier("TornadoSlam", typeof(TornadoSlam), typeof(ChargeTornadoSlam))]
    class DrifterTornadoSlamSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is TornadoSlam)
            {
                Logger.Debug("TornadoSlam");
            } 
            else if (skillState is ChargeTornadoSlam)
            {
                Logger.Debug("ChargeTornadoSlam");
            }
        }
    }*/
    
    [SkillLevelModifier("Salvage", typeof(Salvage))]
    class DrifterSalvageSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void SetupSkill()
        {
            base.SetupSkill();
            
            IL.EntityStates.Drifter.Salvage.OnEnter += SalvageOnOnEnter;
        }

        private void SalvageOnOnEnter(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                    x => x.MatchLdarg(0),
                    x => x.MatchLdcI4(4),
                    x => x.MatchStfld<Salvage>("itemsToDrop")
                ))
            {
                //c.Index -= 3;
                c.RemoveRange(3);
            }
            else 
            {
                Logger.Error(il.Method.Name + " IL Hook failed!");
            }
            Logger.Debug("salvage = " + il.ToString());
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is Salvage salvage)
            {
                Logger.Debug("Salvage");
                salvage.itemsToDrop = AdditiveScaling(4, 1, level);
                salvage.delayBetweenDrops = (salvage.delayBetweenDrops * 4) / salvage.itemsToDrop;
                Logger.Debug("Salvage delay = " + salvage.delayBetweenDrops);
                //salvage.delayBetweenDrops = AdditiveScaling(salvage.itemsToDrop, 1, level);
            } 
            
        }
    }
    
    [SkillLevelModifier("Tinker", typeof(CastTinker))]
    class DrifterTinkerSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void SetupSkill()
        {
            base.SetupSkill();
            
            IL.RoR2.Projectile.TinkerProjectile.ManageMonster += TinkerProjectileOnManageMonster;
            IL.RoR2.Projectile.TinkerProjectile.TransmuteTargetObject += TinkerProjectileOnTransmuteTargetObject;
        }

        private void TinkerProjectileOnTransmuteTargetObject(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                    x => x.MatchLdloca(7),
                    x => x.MatchLdcR4(1),
                    x => x.MatchStfld(typeof(RoR2.UniquePickup), "decayValue")
                ))
            {
                c.Index += 1;
                c.Remove();
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RoR2.Projectile.TinkerProjectile, float>>((tinkerProjectile) =>
                    {
                        int level = 0;
                        float newdecay = 1;
                        SkillUpgrade[] skillUpgrades = tinkerProjectile.ownerBody.gameObject.GetComponents<SkillUpgrade>();
                        foreach (var upgrade in skillUpgrades)
                        {
                            Logger.Debug(upgrade.targetBaseSkillName);
                            if (upgrade.targetBaseSkillName == "Tinker")
                            {
                                level = upgrade.skillLevel;
                            }
                        }

                        if (level >= 2)
                        {
                            newdecay += 0.5f/2f * (level - level % 2);
                        }
                        Logger.Debug("decayvalue time = " + newdecay);
                        return newdecay;
                    }
                );
            }
            else 
            {
                Logger.Error(il.Method.Name + " IL Hook failed!");
            }
            
            if (c.TryGotoNext(
                    x => x.MatchLdloca(6),
                    x => x.MatchLdcR4(1),
                    x => x.MatchStfld(typeof(RoR2.UniquePickup), "decayValue")
                ))
            {
                c.Index += 1;
                c.Remove();
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RoR2.Projectile.TinkerProjectile, float>>((tinkerProjectile) =>
                    {
                        int level = 0;
                        float newdecay = 1;
                        SkillUpgrade[] skillUpgrades = tinkerProjectile.ownerBody.gameObject.GetComponents<SkillUpgrade>();
                        foreach (var upgrade in skillUpgrades)
                        {
                            Logger.Debug(upgrade.targetBaseSkillName);
                            if (upgrade.targetBaseSkillName == "Tinker")
                            {
                                level = upgrade.skillLevel;
                            }
                        }

                        if (level >= 2)
                        {
                            newdecay += 0.5f/2f * (level - level % 2);
                        }
                        Logger.Debug("decayvalue else time = " + newdecay);
                        return newdecay;
                    }
                );
            }
            else 
            {
                Logger.Error(il.Method.Name + " IL Hook failed!");
            }
            
            Logger.Debug("managemonster = " + il.ToString());
        }

        private void TinkerProjectileOnManageMonster(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (c.TryGotoNext(
                    x => x.MatchLdloc(0),
                    x => x.MatchLdloc(1),
                    x => x.MatchLdcR4(4),
                    x => x.MatchCallvirt(typeof(RoR2.CharacterBody), "AddTimedBuff")
                ))
            {
                c.Index += 2;
                c.Remove();
                c.Emit(OpCodes.Ldarg_0);
                c.EmitDelegate<Func<RoR2.Projectile.TinkerProjectile, float>>((tinkerProjectile) =>
                    {
                        int level = 0;
                        SkillUpgrade[] skillUpgrades = tinkerProjectile.ownerBody.gameObject.GetComponents<SkillUpgrade>();
                        foreach (var upgrade in skillUpgrades)
                        {
                            Logger.Debug(upgrade.targetBaseSkillName);
                            if (upgrade.targetBaseSkillName == "Tinker")
                            {
                                level = upgrade.skillLevel;
                            }
                        }

                        Logger.Debug("debuff time = " + MultScaling(4f, 0.25f, level));
                        return MultScaling(4f, 0.25f, level);
                    }
                );
            }
            else 
            {
                Logger.Error(il.Method.Name + " IL Hook failed!");
            }
            
            Logger.Debug("managemonster = " + il.ToString());
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is CastTinker tinker)
            {
                Logger.Debug("CastTinker");
                
                tinker.projectilePrefab.transform.localScale = new Vector3(
                    MultScaling(1, .25f, level),
                    MultScaling(1, .25f, level),
                    MultScaling(1, .25f, level));
                Logger.Debug("scale = " + tinker.projectilePrefab.transform.localScale);
                //tinker.
            } 
        }
    }
}