using System.Collections.Generic;
using System.Linq;
using Turbo.Plugins.Default;


namespace Turbo.Plugins.Prrovoss.Popups
{

    public class MonsterAppearedPopup : BasePlugin, IInGameWorldPainter
    {
        public class MyMonster
        {
            public List<double> MaxHitpoints { get; set; }
            public uint Sno { get; set; }
            public string Name { get; set; }
            public string Hint { get; set; }
            public string Title { get; set; }
            public int Duration { get; set; }
            public TopLabelWithTitleDecorator Decorator { get; set; }

            public MyMonster(uint sno, string name, string hint, string title, int duration, TopLabelWithTitleDecorator decorator = null)
            {
                MaxHitpoints = new List<double>();
                this.Sno = sno;
                this.Hint = hint;
                this.Name = name;
                this.Title = title;
                this.Duration = duration;
                this.Decorator = decorator;
            }
        }

        public List<MyMonster> MonstersToWatch { get; set; }

        public override void Load(IController hud)
        {
            base.Load(hud);
            MonstersToWatch = new List<MyMonster>();            
        }

        public void Add(uint sno, string name, string hint, string title, int duration, TopLabelWithTitleDecorator decorator = null)
        {
            MonstersToWatch.Add(new MyMonster(sno, name, hint, title, duration, decorator));
        }

        public void PaintWorld(WorldLayer layer)
        {
            foreach (MyMonster monster in MonstersToWatch)
            { var candidates = Hud.Game.Monsters.Where(m => m.SnoActor.Sno == monster.Sno && !monster.MaxHitpoints.Contains(m.MaxHealth));
                if (candidates.Count() != 0)
                {
                    foreach (IMonster candidate in candidates)
                    {
                        Hud.RunOnPlugin<PopupNotifications>(plugin =>
                        {
                            plugin.Show(monster.Name, monster.Title, monster.Duration, monster.Hint);
                        });
                        monster.MaxHitpoints.Add(candidate.MaxHealth);
                    }
                }
            }

        }

        public MonsterAppearedPopup()
        {
            Enabled = true;
        }

    }

}