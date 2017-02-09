using System;
using System.Linq;

using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{

    public class PopupNotifications : BasePlugin
    {
        public TopLabelWithTitleDecorator PopupDecorator { get; set; }
        public float RatioX { get; set; }
        public float RatioY { get; set; }
        public float RatioW { get; set; }
        public float RatioH { get; set; }
        public float VerticalGap { get; set; }

        public class Popup : IQueueItem
        {
            public string Text { get; set; }
            public string Title { get; set; }
            public string Hint { get; set; }
            public DateTime QueuedOn { get; private set; }
            public TimeSpan LifeTime { get; private set; }
            public Popup(string text, string title, TimeSpan lifetime, string hint)
            {
                this.Text = text;
                this.Title = title;
                this.LifeTime = lifetime;
                this.Hint = hint;
                this.QueuedOn = DateTime.Now;
            }
        }


        public void Show(string text, string title, int duration, string hint = null)
        {
            Hud.Queue.AddItem(new Popup(text, title, new TimeSpan(0, 0, 0, 0, duration), hint));
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

            RatioX = 0.6f;
            RatioY = 0.75f;
            RatioW = 0.18f;
            RatioH = 0.05f;
            VerticalGap = 1.20f;
        }

        public override void PaintWorld(WorldLayer layer)
        {
            int ctr = 0;

            var w = Hud.Window.Size.Height * RatioW;
            var h = Hud.Window.Size.Height * RatioH;
            var x = Hud.Window.Size.Width * RatioX;
            var y = Hud.Window.Size.Height * RatioY;

            foreach (Popup p in Hud.Queue.GetItems<Popup>().Take(13))
            {
                    PopupDecorator.Paint(x, y - ctr * (h * VerticalGap), w, h, p.Text, p.Title, p.Hint);
                    ctr++;
            }

        }


    }

}
