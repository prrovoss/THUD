using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{
    [Serializable]
    public class CustomLabel
    {
        
        public bool Enabled { get; set; }
        
        public IController Hud { get; set; }
        //public int ID {get; set;}
        public float x { get; set; }
        public float y { get; set; }
        public float defaultX { get; set; }
        public float defaultY { get; set; }
        
        public int width { get; set; }
        
        public int heigth { get; set; }
        public string baseText { get; set; }
        
        public string text { get; set; }
        public string hint { get; set; }

         public IBrush BorderBrush { get { return Decorator.BorderBrush; } set { Decorator.BorderBrush = value; } }

         public IBrush BackgroundBrush { get { return Decorator.BackgroundBrush; } set { Decorator.BackgroundBrush = value; } }
         public IFont TextFont { get { return Decorator.TextFont; } set { Decorator.TextFont = value; } }
         public IFont TitleFont { get { return Decorator.TitleFont; } set { Decorator.TitleFont = value; } }

        
        public ICondition condition { get; set; }
        
        public TopLabelWithTitleDecorator Decorator { get; set; }
        
        public List<ICondition> conditions { get; set; }
        
        public Func<string> customExpression { get; set; }

        public bool show()
        {
            foreach (ICondition c in this.conditions)
            {
                if (!c.evaluate(Hud))
                {
                    return false;
                }
            }
            return true;
        }

        public void addCondition(ICondition c)
        {
            conditions.Add(c);

        }

        public CustomLabel()
        {

        }

        public CustomLabel(float x, float y, int width, int heigth, string text, string hint, IController hud, ICondition condition)
            : this(x, y, width, heigth, text, hint, hud)
        {
            addCondition(condition);
        }

        public CustomLabel(float x, float y, int width, int heigth, string text, string hint, IController hud)
        {
            this.customExpression = null;
            this.conditions = new List<ICondition>();
            this.Enabled = true;
            this.Hud = hud;
            this.x = x;
            this.y = y;
            this.defaultX = x;
            this.defaultY = y;
            this.width = width;
            this.heigth = heigth;
            this.baseText = text;
            this.hint = hint;

            Decorator = new TopLabelWithTitleDecorator(Hud)
            {
                BorderBrush = Hud.Render.CreateBrush(255, 180, 147, 109, -1),
                BackgroundBrush = Hud.Render.CreateBrush(128, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 255, 210, 150, true, false, false),
                TitleFont = Hud.Render.CreateFont("tahoma", 6, 255, 180, 147, 109, true, false, false),
            };
        }

        public void prepare()
        {
            try
            {
                if ((customExpression != null) && (this.baseText.Contains("*custom-expression")))
                {
                    this.text = baseText.Replace("*custom-expression", customExpression());
                }
                else
                {
                    this.text = this.baseText;
                }

                this.text = Regex.Replace(this.text, @"\*(\S+)", m => Expressions.Value(m.Groups[0].Value));
            }
            catch
            {
                this.text = this.baseText;
            }
        }

    }
}