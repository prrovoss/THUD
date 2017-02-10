using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{

    public class UtilityCollection : BasePlugin
    {

        private IBrush cursorBrush;
        public bool drawCursorCircle { get; set; }
        public bool drawCursorLabel { get; set; }
        public TopLabelWithTitleDecorator cursorDecorator { get; set; }

        public UtilityCollection()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            cursorBrush = Hud.Render.CreateBrush(200, 255, 255, 255, 4);

            drawCursorCircle = false;
            drawCursorLabel = false;

            cursorDecorator = new TopLabelWithTitleDecorator(Hud)
            {
                TextFont = hud.Render.CreateFont("tahoma", 10, 255, 255, 255, 255, false, false, false),
                BorderBrush = Hud.Render.CreateBrush(255, 255, 255, 255, -1),
                BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
            };
        }

        public int monsterDensityAroundPlayer(float distance)
        {
            if (!Hud.Game.Me.IsInTown)
            {
                IWorldCoordinate me = Hud.Game.Me.FloorCoordinate;
                int count = 0;
                foreach (IMonster monster in Hud.Game.AliveMonsters)
                {
                    if (monster.FloorCoordinate.XYDistanceTo(me) < distance) count++;
                }
                return count;
            }
            return 0;
        }

        public int monsterDensityAroundCursor(float distance)
        {
            if (!Hud.Game.Me.IsInTown)
            {
                IScreenCoordinate coord = Hud.Game.Me.FloorCoordinate.ToScreenCoordinate(); //Hud.Render.GetCursorPos();
                coord.X = Hud.Window.CursorX;
                coord.Y = Hud.Window.CursorY;
                IWorldCoordinate cursor = coord.ToWorldCoordinate();
                int count = 0;
                foreach (IMonster monster in Hud.Game.AliveMonsters)
                {
                    if (monster.FloorCoordinate.XYDistanceTo(cursor) < distance) count++;
                }
                if (drawCursorCircle)
                {
                    cursorBrush.DrawWorldEllipse(distance, -1, cursor);
                }
                if (drawCursorLabel)
                {
                    cursorDecorator.Paint(Hud.Window.CursorX - 10, Hud.Window.CursorY + 30, 50, 20, count.ToString(), null, "");
                }
                return count;
            }
            return 0;
        }

    }

}