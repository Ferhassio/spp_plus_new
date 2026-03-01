using EntityStates;
using RoR2;
using RoR2.Skills;

namespace SkillsPlusPlus.Modifiers
{

    /// <summary>
    /// A simple implementation of the <see cref="BaseSkillModifier"/> that is designed for character skills that have a single entity state
    /// </summary>
    /// <typeparam name="SkillState">The single <see cref="EntityStates.BaseState"/> this skill is coupled with</typeparam>
    public abstract class SimpleSkillModifier<SkillState> : BaseSkillModifier where SkillState : BaseState
    {

        /// <inheritdoc/>
        public sealed override void OnSkillEnter(BaseState skillState, int level)
        {
            if (skillState is SkillState)
            {
                Logger.Debug("OnSkillEnter({0}, {1})", skillState, level);
                this.OnSkillEnter(skillState as SkillState, level);
            }
            else
            {
                Logger.Warn("Unable to cast {0} to {1}", skillState, typeof(SkillState).FullName);
            }
        }

        /// <inheritdoc/>
        public sealed override void OnSkillExit(BaseState skillState, int level)
        {
            if (skillState is SkillState)
            {
                Logger.Debug("OnSkillExit({0}, {1})", skillState, level);
                this.OnSkillExit(skillState as SkillState, level);
            }
            else
            {
                Logger.Warn("Unable to cast {0} to {1}", skillState, typeof(SkillState).FullName);
            }
        }

        /// <inheritdoc/>
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            //also very log heavy in debug
            //Logger.Debug("OnSkillLeveledUp({0}, {1}, {2})", level, characterBody, skillDef);
            FindSkillUpgrade(characterBody, "blank", true);
            // no-op
        }

        /// <summary>
        /// Called immediately before the character enters this modifier's associated skill state
        /// </summary>
        /// <param name="skillState">The entity state instance</param>
        /// <param name="level">The current level of the associated skill</param>
        public virtual void OnSkillEnter(SkillState skillState, int level)
        {
            // no-op
        }

        /// <summary>
        /// Called when the character exits this modifier's associated skill state
        /// </summary>
        /// <param name="skillState">The entity state instance</param>
        /// <param name="level">The current level of the associated skill</param>
        public virtual void OnSkillExit(SkillState skillState, int level)
        {
            // no-op
        }
    }
}