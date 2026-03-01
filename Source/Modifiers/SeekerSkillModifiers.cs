namespace SkillsPlusPlus.Source.Modifiers
{
    /*[SkillLevelModifier("SeekerBodySpiritPunchCrosshair", typeof(SpiritPunch))]
    class SeekerPunchSkillModifier : BaseSkillModifier
    {
        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);
            if (skillState is SpiritPunch)
            {
                Logger.Debug("SpiritPunch");
            }
        }
    }

    [SkillLevelModifier("SeekerBodyUnseenHand", typeof(UnseenHand))]
    class SeekerUnseenHandSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is UnseenHand)
            {
                Logger.Debug("UnseenHand");
            }
        }
    }

    [SkillLevelModifier("SeekerBodySoulSpiral", typeof(SoulSpiral))]
    class SeekerSoulSpiralSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is SoulSpiral)
            {
                Logger.Debug("SoulSpiral");
            }
        }
    }

    [SkillLevelModifier("SeekerBodySojourn", typeof(Sojourn), typeof(SojournVehicle))]
    class SeekerSojournSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is Sojourn)
            {
                Logger.Debug("Sojourn");
            } 
        }
        public override void SetupSkill()
        {
            base.SetupSkill();

            On.RoR2.VehicleSeat.OnPassengerEnter += (orig, self, passenger) =>
            {
                orig(self, passenger);
                if (self.name.Contains("SojournVehicle"))
                {
                    Logger.Debug("sojourn started !!!");
                    var car = self.GetComponent<SojournVehicle>();
                    if (car == null)
                    {
                        Logger.Debug("erm., ,. nvm ,.,..");
                    }
                    else
                    {
                        Logger.Debug("sojourn real !!");
                        car.startingSpeedBoost = 100f;
                    }
                }
            };
        }
    }

    [SkillLevelModifier("SeekerBodyMeditate2", typeof(Meditate),typeof(MeditationUI))]
    class SeekerMeditateSkillModifier : BaseSkillModifier
    {

        public override void OnSkillLeveledUp(int level, CharacterBody characterBody, SkillDef skillDef)
        {
            base.OnSkillLeveledUp(level, characterBody, skillDef);
        }

        public override void OnSkillEnter(BaseState skillState, int level)
        {
            base.OnSkillEnter(skillState, level);

            if (skillState is Meditate)
            {
                Logger.Debug("Meditate");
            } else if (skillState is MeditationUI)
            {
                Logger.Debug("MeditationUI");
            }
        }
    }*/
}