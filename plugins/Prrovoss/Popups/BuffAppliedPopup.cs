using System.Collections.Generic;

using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss.Popups
{

    public class BuffAppliedPopup : BasePlugin, IInGameWorldPainter
    {
        public class Buff
        {
            public bool Displayed { get; set; }
            public uint SNO { get; set; }
            public int Icon { get; set; }
            public string Name { get; set; }
            public string Hint { get; set; }
            public string Title { get; set; }
            public int Duration { get; set; }

            public Buff(uint sno, int icon, string name, string hint, string title, int duration)
            {
                this.SNO = sno;
                this.Icon = icon;
                this.Displayed = false;
                this.Name = name;
                this.Title = title;
                this.Duration = duration;
            }
        }

        public List<Buff> BuffsToWatch { get; set; }

        public override void Load(IController hud)
        {
            base.Load(hud);
            BuffsToWatch = new List<Buff>();
            
            BuffsToWatch.Add(new Buff(465952, 1, "Final Service", "", "Buff activated", 6000));
            BuffsToWatch.Add(new Buff(262935, 0, "Power Pylon", "", "Buff activated", 4000));
            BuffsToWatch.Add(new Buff(266258, 0, "Channeling Pylon", "", "Buff activated", 4000));
            BuffsToWatch.Add(new Buff(266254, 0, "Shield Pylon", "", "Buff activated", 4000));
            BuffsToWatch.Add(new Buff(402461, 2, "Occulus", "", "Buff activated", 3000));    
            BuffsToWatch.Add(new Buff(246562, 1, "Flying Dragon", "", "Buff activated", 3000)); 
        }

        public void Add(uint sno, int icon, string name, string hint, string title, int duration) {
            BuffsToWatch.Add(new Buff(sno, icon, name, hint, title, duration));
        }

        public void PaintWorld(WorldLayer layer)
        {

            foreach (Buff buff in BuffsToWatch)
            {
                if (Hud.Game.Me.Powers.BuffIsActive(buff.SNO, buff.Icon))
                {
                    if (!buff.Displayed)
                    {
                        Hud.RunOnPlugin<PopupNotifications>(plugin =>
                    {
                        plugin.Show(buff.Name, buff.Title, buff.Duration, buff.Hint);
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

        public BuffAppliedPopup()
        {
            Enabled = true;
        }

    }

}