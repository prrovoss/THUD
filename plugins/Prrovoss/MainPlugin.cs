using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using Turbo.Plugins.Default;
using System;
using System.Globalization;
using Turbo.Plugins.RuneB;
namespace Turbo.Plugins.Prrovoss
{
    public class MainPlugin : BasePlugin, ICustomizer 
    {

        public MainPlugin()
        {
            Enabled = true;
            Order = -10;
        }


        public override void Load(IController hud)
        {
            base.Load(hud);
        }

        public string countdownExpression()
        {
            return "";
        }

        public string testCustomExpression()
        {
            //return Hud.Collections.Me.BattleTag+" "+Hud.Collections.Me.HeroName;
            return "";
        }

        public string cursorPosExpression()
        {
            PointF cursorPos =  new PointF(Hud.Window.CursorX, Hud.Window.CursorY);  
            return "Cursor Pos. x=" + cursorPos.X + " y=" + cursorPos.Y;
        }

        public string riftTimerExpression() {
            //337492 
            var rifts = Hud.Game.Quests.Where(d => d.SnoQuest.Sno==337492);
            Hud.Debug(rifts.Count().ToString("F0", CultureInfo.InvariantCulture));
            if(rifts.Count()>0) {


            switch(rifts.First().State) {
                case QuestState.started:  return new TimeSpan(rifts.First().StartedOn.ElapsedMilliseconds  * 10000).ToString(@"hh\:mm\:ss");
                
                case QuestState.completed: return new TimeSpan((rifts.First().CompletedOn.ElapsedMilliseconds - rifts.First().StartedOn.ElapsedMilliseconds)  * 10000).ToString(@"hh\:mm\:ss") + " - completed";

                case QuestState.none: return "no rift informations";

            }
        }
            return "no rift informations";
        }

        public class controlKeyPressedCondition : ICondition
        {
            public bool evaluate(IController hud)
            {
                return hud.Input.IsKeyDown(Keys.ControlKey);
            }
        }

        public class testCustomCondition : ICondition
        {
            public bool evaluate(IController hud)
            {
                return hud.Game.Me.InGreaterRift;
            }
        }

        public void Customize()
        {

            // healing well label
            Hud.GetPlugin<ShrinePlugin>().HealingWellDecorator.Enabled = false;


            // ground circles for materials
            Hud.GetPlugin<ItemsPlugin>().NormalKeepDecorator.Enabled = false;
            Hud.GetPlugin<ItemsPlugin>().MagicKeepDecorator.Enabled = false;
            Hud.GetPlugin<ItemsPlugin>().RareKeepDecorator.Enabled = false;
            Hud.GetPlugin<BuffAppliedPopup>().Enabled = false;

            // turn on MultiplayerExperienceRangePlugin
            Hud.TogglePlugin<MultiplayerExperienceRangePlugin>(true);

            // turn off sell darkening
            Hud.GetPlugin<InventoryAndStashPlugin>().NotGoodDisplayEnabled = false;

           // Hud.GetPlugin<NearbyMonsterHealthAggregatorPlugin>().Enabled = false;

           Hud.TogglePlugin<MonsterDensityAroundCursor>(false);
           Hud.TogglePlugin<CustomLabelPlugin_Example>(false);
           Hud.GetPlugin<MonsterDensityAroundCursor>().DrawCursorLine = true;


            CustomLabelPlugin labels = Hud.GetPlugin<CustomLabelPlugin>();



            CustomLabel resourceLabel = new CustomLabel(1225, 420, 100, 20, "*resource-pct-pri", "", Hud);
            resourceLabel.addCondition(new Conditions.notInTown());
            resourceLabel.BorderBrush =  Hud.Render.CreateBrush(0, 180, 147, 109, -1);
            resourceLabel.BackgroundBrush = resourceLabel.BorderBrush;
            resourceLabel.TextFont = Hud.Render.CreateFont("tahoma", 14, 255, 255, 255, 0, true, false, true);
            labels.add(resourceLabel);

            CustomLabel cursorPosLabel = new CustomLabel(580, 0, 100, 20, "*custom-expression", "Cursor Pos", Hud, new controlKeyPressedCondition());
            cursorPosLabel.customExpression = cursorPosExpression;
            cursorPosLabel.BorderBrush =  Hud.Render.CreateBrush(0, 180, 147, 109, -1);
            cursorPosLabel.BackgroundBrush = resourceLabel.BorderBrush;
            cursorPosLabel.TextFont = Hud.Render.CreateFont("tahoma", 6, 255, 255, 255, 255, false, false, true);
            labels.add(cursorPosLabel);

            CustomLabel oculusLabel = new CustomLabel(1225, 720, 100, 20, "Oculus", "", Hud, new Conditions.buffActive(402461, 2));
            oculusLabel.TextFont = Hud.Render.CreateFont("tahoma", 13, 255, 0, 255, 0, true, false, true);
            oculusLabel.BorderBrush =  Hud.Render.CreateBrush(0, 0, 0, 0, -1);
            oculusLabel.BackgroundBrush = oculusLabel.BorderBrush;
            labels.add(oculusLabel);

            CustomLabel archonLabel = new CustomLabel(1225, 460, 100, 20, "Archon ready!", "", Hud);
            archonLabel.addCondition(new Conditions.notOnCooldown(134872));
            archonLabel.addCondition(new Conditions.buffNotActive(134872, 2));
            archonLabel.addCondition(new Conditions.notInTown());
            archonLabel.addCondition(new Conditions.isClass(HeroClass.Wizard));
            archonLabel.TextFont = Hud.Render.CreateFont("tahoma", 12, 255, 255, 0, 69, true, false, true);
            archonLabel.BorderBrush =  Hud.Render.CreateBrush(0, 0, 0, 0, -1);
            archonLabel.BackgroundBrush = archonLabel.BorderBrush;
            labels.add(archonLabel);


            var feetBuffs = Hud.GetPlugin<PlayerBottomBuffListPlugin>().RuleCalculator.Rules;
    
            feetBuffs.Add(new BuffRule(403471) { IconIndex = null, MinimumIconCount = 1, ShowStacks = true, ShowTimeLeft = true }); // Gem, Taeguk
            feetBuffs.Add(new BuffRule(383014) { IconIndex = null, MinimumIconCount = 1, ShowStacks = false, ShowTimeLeft = true }); // Gem, Bane of the Powerful
            feetBuffs.Add(new BuffRule(403464) { IconIndex = 1, MinimumIconCount = 1, ShowStacks = true, ShowTimeLeft = true }); // Gem, Gogok of Swiftness


            feetBuffs.Add(new BuffRule(263029) { IconIndex = null, MinimumIconCount = 1, ShowTimeLeft = true }); // Pylon, Conduit - Normal Rift
            feetBuffs.Add(new BuffRule(403404) { IconIndex = null, MinimumIconCount = 1, ShowTimeLeft = true }); // Pylon, Conduit - Greater Rift
            feetBuffs.Add(new BuffRule(262935) { IconIndex = null, MinimumIconCount = 1, ShowTimeLeft = true }); // Pylon, Power
            feetBuffs.Add(new BuffRule(266258) { IconIndex = null, MinimumIconCount = 1, ShowTimeLeft = true }); // Pylon, Channeling
            feetBuffs.Add(new BuffRule(266254) { IconIndex = null, MinimumIconCount = 1, ShowTimeLeft = true }); // Pylon, Shield


            feetBuffs.Add(new BuffRule(429673) { IconIndex = null, MinimumIconCount = 1, ShowStacks = true }); // Set, Raekor - 6Pcs
            feetBuffs.Add(new BuffRule(429855) { IconIndex = 5, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = true }); // Set, Tal Rasha - 6Pcs


			feetBuffs.Add(new BuffRule(402458) { IconIndex = 1, MinimumIconCount = 1, ShowTimeLeft = true }); // Legendary, In-Geom
            feetBuffs.Add(new BuffRule(430674) { IconIndex = null, MinimumIconCount = 1, ShowTimeLeft = true }); // Legendary, Convention of Elements           
           

			feetBuffs.Add(new BuffRule(79607) { IconIndex = null, MinimumIconCount = 1, ShowTimeLeft = true }); // Barbarian, Wrath of the Berserker
            feetBuffs.Add(new BuffRule(205187) { IconIndex = 1, MinimumIconCount = 1, ShowTimeLeft = false }); // Barbarian, Berserker Rage            
            feetBuffs.Add(new BuffRule(205133) { IconIndex = 1, MinimumIconCount = 1, ShowTimeLeft = false }); // Barbarian, Brawler
            feetBuffs.Add(new BuffRule(79528) { IconIndex = null, MinimumIconCount = 1, ShowTimeLeft = true }); // Barbarian, Ignore Pain
            feetBuffs.Add(new BuffRule(134872) { IconIndex = 2, MinimumIconCount = 1, ShowTimeLeft = true, ShowStacks = true }); // Archon Stacks
            feetBuffs.Add(new BuffRule(134872) { IconIndex = 5, MinimumIconCount = 0, ShowTimeLeft = true, ShowStacks = true }); // Swami Archon Stacks
        
            Hud.GetPlugin<ArchonWizPlugin>().ShowInTown = false;
			Hud.GetPlugin<ArchonWizPlugin>().AlwaysShowElements = true;


            Log.MaxMessages = 60;
        }

        private void SetEnabled<T>(bool enabled)
            where T : class, IPlugin
        {
            var plugin = Hud.GetPlugin<T>();
            if (plugin != null)
            {
                plugin.Enabled = enabled;
            }
        }


    }

}