using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{

    public class Conditions : BasePlugin
    {
        public static ICondition notInTown { get; set; }
        public static ICondition inTown { get; set; }

        public Conditions()
        {
            Enabled = true;
        }
        public override void Load(IController controller)
        {
            base.Load(controller);
            notInTown = new notInTownCondition();
            inTown = new inTownCondition();
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

        public class notInTownCondition : ICondition
        {
            public bool evaluate(IController hud)
            {
                return !hud.Game.Me.IsInTown;
            }
        }

        public class inTownCondition : ICondition
        {
            public bool evaluate(IController hud)
            {
                return hud.Game.Me.IsInTown;
            }
        }

    }

}