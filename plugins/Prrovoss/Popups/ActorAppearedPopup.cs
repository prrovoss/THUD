using System.Collections.Generic;
using System.Linq;
using Turbo.Plugins.Default;


namespace Turbo.Plugins.Prrovoss.Popups
{

    public class ActorAppearedPopup : BasePlugin, IInGameWorldPainter
    {
        public class MyActor
        {
            public List<IWorldCoordinate> FloorCoords { get; set; }
            public List<float> CreatedTicks { get; set; }            
            public uint Sno { get; set; }
            public string Name { get; set; }
            public string Hint { get; set; }
            public string Title { get; set; }
            public int Duration { get; set; }
            public TopLabelWithTitleDecorator Decorator { get; set; }

            public MyActor(uint sno, string name, string hint, string title, int duration, TopLabelWithTitleDecorator decorator = null)
            {
                FloorCoords = new List<IWorldCoordinate>();
                CreatedTicks = new List<float>();
                this.Sno = sno;
                this.Hint = hint;
                this.Name = name;
                this.Title = title;
                this.Duration = duration;
                this.Decorator = decorator;
            }
        }

        public List<MyActor> ActorsToWatch { get; set; }

        public override void Load(IController hud)
        {
            base.Load(hud);
            ActorsToWatch = new List<MyActor>();
        }

        private bool EqualCoordinates(IWorldCoordinate a, IWorldCoordinate b)
        {
            return ((a.X == b.X) && (a.Y == b.Y) && (a.Z==b.Z));
        }

        public void Add(uint sno, string name, string hint, string title, int duration, TopLabelWithTitleDecorator decorator = null)
        {
            ActorsToWatch.Add(new MyActor(sno, name, hint, title, duration, decorator));
        }

        public void PaintWorld(WorldLayer layer)
        {
            foreach (MyActor actor in ActorsToWatch)
            {
                var candidates = Hud.Game.Actors.Where(a => a.SnoActor.Sno == actor.Sno && !(actor.FloorCoords.Any(c => EqualCoordinates(c, a.FloorCoordinate))) && !(actor.CreatedTicks.Contains(a.CreatedAtInGameTick)) );
                if (candidates.Count() != 0)
                {
                    foreach (IActor candidate in candidates)
                    {
                        Hud.RunOnPlugin<PopupNotifications>(plugin =>
                        {
                            plugin.Show(actor.Name, actor.Title, actor.Duration, actor.Hint);
                        });
                        if (candidate.IsClickable) {
                            actor.FloorCoords.Add(candidate.FloorCoordinate);
                        }
                        actor.CreatedTicks.Add(candidate.CreatedAtInGameTick);
                    }
                }
            }
            
        }

        public ActorAppearedPopup()
        {
            Enabled = true;
        }

    }

}