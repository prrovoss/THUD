using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{

    public class CustomLabelPlugin_Example : BasePlugin, ICustomizer 
    {

        public CustomLabelPlugin_Example()
        {
            // Change this to true if you want to see the new labels
            Enabled = false;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
        }

        public string testCustomExpression()
        {
            // This will just show a static string, your hero name. You can return any string you want, maybe a timestamp or whatever you can think of
            return Hud.Game.Me.HeroName;
        }

        public class testCustomCondition : ICondition
        {
            public bool evaluate(IController hud)
            {
                //Only shows the label if your CDR is greater than 15 (I'm sure you'll find more creative conditions)
                return (hud.Game.Me.Stats.CooldownReduction * 100) > 15;
            }
        }

        public void Customize()
        {
            // This plugin shows you how to use the custom label plugin. 
            // This plugin will be overwritten everytime you update the custom label plugin! 
            // So use it as a guideline or tutorial to modify your own plugins with custom labels.

            // Custom labels:
            CustomLabelPlugin labels = Hud.GetPlugin<CustomLabelPlugin>();


            // EASY WAY:
            // add a custom label at x=500, y=500 with the attributes width=100, heigth=20, text="*resource-pct-pri", hint="primary resource".
            // other attributes like background, opacity, textFont etc are set to default
            labels.add(500, 500, 100, 20, "*resource-pct-pri", "primary resource");

            // CUSTOM WAY:
            // create a new custom label at x=500, y=520 with the attributes width=100, heigth=20, text="*hp-cur-pct", hint="current health".
            // NOTE: the label will not be shown yet, you didnt actually add it yet
            // set some custom attributes:
            CustomLabel healthLabel = new CustomLabel(500, 520, 100, 20, "*hp-cur-pct", "current health", Hud);
            healthLabel.BorderBrush =  Hud.Render.CreateBrush(255, 180, 147, 109, -1);
            healthLabel.BackgroundBrush = Hud.Render.CreateBrush(50, 180, 147, 109, -1);
            healthLabel.TextFont = Hud.Render.CreateFont("Arial", 9, 150, 0, 0, 255, true, true, true);
            // add a default condition 
            healthLabel.addCondition(new Conditions.notInTown());
            // now add the label itself
            labels.add(healthLabel);

            // if you want to create and use your own conditions and expressions, implement them like shown above in the testCustomExpression() method and in the testCustomCondition class 
            // you can add conditions right away in the constructor. alternatively set them like above with myLabel2.addCondition(new testCustomCondition());
            // This will actually show your heroName if your CooldownReduction is greater than 15
            CustomLabel resourceLabel = new CustomLabel(500, 540, 200, 20, "*custom-expression", "", Hud, new testCustomCondition());
            resourceLabel.customExpression = testCustomExpression;
            labels.add(resourceLabel);


            //some more examples:
            labels.add(1225, 420, 100, 20, "Oculus", "", new Conditions.buffActive(402461, 2)); //draw label if oculus buff is active
            labels.add(500, 560, 100, 20, "*monsters-in-20", "monster in 20y"); //display number of monsters in 20y distance to player

        }

    }

}