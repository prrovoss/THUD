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
            public uint Sno { get; set; }
            public string Name { get; set; }
            public string Hint { get; set; }
            public string Title { get; set; }
            public int Duration { get; set; }

            public MyActor(uint sno, string name, string hint, string title, int duration)
            {
                FloorCoords = new List<IWorldCoordinate>();
                this.Sno = sno;
                this.Hint = hint;
                this.Name = name;
                this.Title = title;
                this.Duration = duration;
            }
        }

        public List<MyActor> ActorsToWatch { get; set; }

        public override void Load(IController hud)
        {
            base.Load(hud);
            ActorsToWatch = new List<MyActor>();
            
            ActorsToWatch.Add(new MyActor(330698, "Shield Pylon", "", "Appeared", 5000));
            ActorsToWatch.Add(new MyActor(330697, "Channeling Pylon", "", "Appeared", 5000));
            ActorsToWatch.Add(new MyActor(330695, "Power Pylon", "", "Appeared", 5000));
            ActorsToWatch.Add(new MyActor(398654, "Conduit Pylon", "", "Appeared", 5000));

        }

        private bool EqualCoordinates(IWorldCoordinate a, IWorldCoordinate b)
        {
            return ((a.X == b.X) && (a.Y == b.Y) && (a.Z==b.Z));
        }

        public void Add(uint sno, string name, string hint, string title, int duration)
        {
            ActorsToWatch.Add(new MyActor(sno, name, hint, title, duration));
        }

        public void PaintWorld(WorldLayer layer)
        {
            foreach (MyActor actor in ActorsToWatch)
            {
                
                var candidates = Hud.Game.Actors.Where(a => a.SnoActor.Sno == actor.Sno && !(actor.FloorCoords.Any(c => EqualCoordinates(c, a.FloorCoordinate))));
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
