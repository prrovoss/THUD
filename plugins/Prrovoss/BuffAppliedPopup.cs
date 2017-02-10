using System.Collections.Generic;
using System;
using System.Linq;

using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{

    public class BuffApplied : BasePlugin
    {
        public class Buff
        {
            public bool Displayed { get; set; }
            public uint SNO { get; set; }
            public int Icon { get; set; }
            public string Name { get; set; }

            public Buff(uint sno, int icon, string name)
            {
                this.SNO = sno;
                this.Icon = icon;
                this.Displayed = false;
                this.Name = name;
            }
        }

        public List<Buff> BuffsToWatch { get; set; }

        public override void Load(IController hud)
        {
            base.Load(hud);
            BuffsToWatch = new List<Buff>();
            BuffsToWatch.Add(new Buff(402461, 2, "Occulus"));
            BuffsToWatch.Add(new Buff(246562, 1, "Flying Dragon"));
        }

        public override void PaintWorld(WorldLayer layer)
        {

            foreach (Buff buff in BuffsToWatch)
            {
                if (Hud.Game.Me.Powers.BuffIsActive(buff.SNO, buff.Icon))
                {
                    if (!buff.Displayed)
                    {
                        Hud.RunOnPlugin<PopupNotifications>(plugin =>
                    {
                        plugin.Show(buff.Name, "Buff activated", 3000, "So much power");
                    });
                        buff.Displayed = true;
                    }
                }
                else
                {
                    buff.Displayed = false;
                }
            }

        }

        public BuffApplied()
        {
            Enabled = true;
        }

    }

}
