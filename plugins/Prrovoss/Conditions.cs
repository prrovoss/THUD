using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{

    public class Conditions : BasePlugin
    {

        public Conditions()
        {
            Enabled = true;
        }
        public override void Load(IController controller)
        {
            base.Load(controller);
        }

        public class isClass : ICondition
        {   
            public HeroClass heroClass { get; set; }

            public isClass(HeroClass heroClass)
            {
                this.heroClass = heroClass;
            }

            public bool evaluate(IController hud)
            {
                return hud.Game.Me.HeroClassDefinition.HeroClass == this.heroClass;
            }
        } 

        public class onCooldown : ICondition
        {
            public uint SNO { get; set; }

            public onCooldown(uint sno)
            {
                this.SNO = sno;
            }

            public bool evaluate(IController hud)
            {
                foreach (IPlayerSkill skill in hud.Game.Me.Powers.SkillSlots)
                {
                    if (skill.SnoPower.Sno == this.SNO && skill.IsOnCooldown)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public class notOnCooldown : ICondition
        {
            public uint SNO { get; set; }

            public notOnCooldown(uint sno)
            {
                this.SNO = sno;
            }

            public bool evaluate(IController hud)
            {
                foreach (IPlayerSkill skill in hud.Game.Me.Powers.SkillSlots)
                {
                    if (skill.SnoPower.Sno == this.SNO && skill.IsOnCooldown)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public class buffActive : ICondition
        {
            public uint SNO { get; set; }
            public int Icon { get; set; }
            public buffActive(uint sno)
            {
                this.SNO = sno;
                this.Icon = -1;
            }
            public buffActive(uint sno, int iconIndex)
            {
                this.SNO = sno;
                this.Icon = iconIndex;
            }
            public bool evaluate(IController hud)
            {
                if (Icon == -1)
                {
                    return hud.Game.Me.Powers.BuffIsActive(this.SNO);
                }
                else
                {
                    return hud.Game.Me.Powers.BuffIsActive(this.SNO, this.Icon);
                }
            }
        }

        public class buffNotActive : ICondition
        {
            public uint SNO { get; set; }
            public int Icon { get; set; }
            public buffNotActive(uint sno)
            {
                this.SNO = sno;
                this.Icon = -1;
            }
            public buffNotActive(uint sno, int iconIndex)
            {
                this.SNO = sno;
                this.Icon = iconIndex;
            }
            public bool evaluate(IController hud)
            {
                if (Icon == -1)
                {
                    return !hud.Game.Me.Powers.BuffIsActive(this.SNO);
                }
                else
                {
                    return !hud.Game.Me.Powers.BuffIsActive(this.SNO, this.Icon);
                }
            }
        }

        public class notInTown : ICondition
        {
            public bool evaluate(IController hud)
            {
                return !hud.Game.Me.IsInTown;
            }
        }

        public class inTown : ICondition
        {
            public bool evaluate(IController hud)
            {
                return hud.Game.Me.IsInTown;
            }
        }

    }

}