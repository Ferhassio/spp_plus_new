using EntityStates;
using EntityStates.Drone.Command;
using EntityStates.DroneTech;
using EntityStates.DroneTech.Weapon;
using RoR2;
using RoR2.Skills;
using SkillsPlusPlus.Modifiers;

namespace SkillsPlusPlus.Source.Modifiers
{
    /*[SkillLevelModifier("FireNanoPistol", typeof(FireNanoPistol))]
    class DroneTechFireNanoPistolSkillModifier : BaseSkillModifier
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is FireNanoPistol)
            {
                Logger.Debug("FireNanoPistol");
            }
        }
    }

    [SkillLevelModifier("Command", typeof(Command), typeof(CommandCarry))]
    class DroneTechCommandSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is Command)
            {
                Logger.Debug("Command");
            }
        }
    }

    [SkillLevelModifier("CommandHeadbutt", typeof(CommandHeadbutt))]
    class DroneTechCommandHeadbuttSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is CommandHeadbutt)
            {
                Logger.Debug("CommandHeadbutt");
            }
        }
    }

    [SkillLevelModifier("DroneLeap", typeof(DroneLeap), typeof(DroneLeapRepeat))]
    class DroneTechDroneLeapSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is DroneLeap)
            {
                Logger.Debug("DroneLeap");
            } 
            if (skillState is DroneLeapRepeat)
            {
                Logger.Debug("DroneLeapRepeat");
            } 
        }
    }

    [SkillLevelModifier("CommandShieldFormation", typeof(CommandShield))]
    class DroneTechCommandShieldFormationSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is CommandShield)
            {
                Logger.Debug("CommandShield");
            } 
        }
    }
    
    [SkillLevelModifier("DroneBall", typeof(ThrowDroneBall))]
    class DroneTechDroneBallSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is ThrowDroneBall)
            {
                Logger.Debug("ThrowDroneBall");
            } 
        }
    }
    
    [SkillLevelModifier("DroneBallShootable", typeof(ThrowDroneBallShootable))]
    class DroneTechDroneBallShootableSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is ThrowDroneBallShootable)
            {
                Logger.Debug("ThrowDroneBallShootable");
            } 
        }
    }*/
}