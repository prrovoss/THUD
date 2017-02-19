using System.Collections.Generic;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{
    public class CustomLabelPlugin : BasePlugin, IInGameTopPainter 
    {


        public List<CustomLabel> labels { get; set; }

        public Dictionary<string, string> expressions { get; set; }


        public CustomLabelPlugin()
        {
            Enabled = true;
        }

        public void add(float x, float y, int width, int heigth, string text, string hint, ICondition condition)
        {
            labels.Add(new CustomLabel(x, y, width, heigth, text, hint, Hud, condition));
        }

        public void add(float x, float y, int width, int heigth, string text, string hint)
        {
            labels.Add(new CustomLabel(x, y, width, heigth, text, hint, Hud));
        }

        public void add(CustomLabel label)
        {
            labels.Add(label);
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            labels = new List<CustomLabel>();
        }

        public void PaintTopInGame(ClipState clipState)
        {
            //if (clipState == ClipState.BeforeClip)
            //{
                foreach (CustomLabel l in this.labels)
                {
                    if (l.show())
                    {
                        l.prepare();
                        l.Decorator.Paint(l.x, l.y, l.width, l.heigth, l.text, null, l.hint);
                    }
                }
            //}
        }
    }
}
