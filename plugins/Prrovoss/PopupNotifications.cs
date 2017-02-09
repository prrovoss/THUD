using System.Collections.Generic;
using System.Linq;

using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{

    public class PopupNotifications : BasePlugin
    {
        public List<Popup> Popups { get; set; }
        public TopLabelWithTitleDecorator PopupDecorator { get; set; }
        public float X, Y, W, H;

        public class Popup
        {
            public string Text { get; set; }
            public string Title { get; set; }
            public string Hint { get; set; }
            public long EndMilli { get; set; }
            public Popup(string text, string title, long endMilli, string hint)
            {
                this.Text = text;
                this.Title = title;
                this.EndMilli = endMilli;
                this.Hint = hint;
            }
        }


        public bool Show(string text, string title, int duration, string hint = null)
        {
            if (Popups.Count < 50)
            {
                Popups.Add(new Popup(text, title, Hud.Game.CurrentRealTimeMilliseconds + duration, hint));
                return true;
            }
            else
            {
                return false;
            }
        }

        public PopupNotifications()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            PopupDecorator = new TopLabelWithTitleDecorator(Hud)
            {
                BorderBrush = Hud.Render.CreateBrush(255, 180, 147, 109, -1),
                BackgroundBrush = Hud.Render.CreateBrush(200, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 255, 255, true, false, false),
                TitleFont = Hud.Render.CreateFont("tahoma", 6, 255, 180, 147, 109, true, false, false),
            };

            W = Hud.Window.Size.Height * 0.18f;
            H = Hud.Window.Size.Height * 0.05f;
            X = Hud.Window.Size.Width * 0.6f;
            Y = Hud.Window.Size.Height * 0.75f;


            Popups = new List<Popup>();
        }

        public override void PaintWorld(WorldLayer layer)
        {
            int ctr = 0;
            Popup p;
            while ((ctr < Popups.Count) && (ctr < 13))
            {
                p = Popups.ElementAt(ctr);
                if (p.EndMilli < Hud.Game.CurrentRealTimeMilliseconds)
                {
                    Popups.Remove(p);
                }
                else
                {
                    PopupDecorator.Paint(X, Y - ctr * (H * 1.20f), W, H, p.Text, p.Title, p.Hint);
                    ctr++;
                }
            }

        }


    }

}
