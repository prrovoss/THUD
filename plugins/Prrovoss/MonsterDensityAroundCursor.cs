using Turbo.Plugins.Default;
using System.Globalization;

namespace Turbo.Plugins.Prrovoss
{

    public class MonsterDensityAroundCursor : BasePlugin, IInGameWorldPainter
    {
        public bool DrawCursorCircle { get; set; }
        public bool DrawCursorLabel { get; set; }
        public bool DrawTopLabel { get; set; }
        public bool DrawCursorLine { get; set; }
        public bool DrawDistanceLabel { get; set; }
        public bool DrawDistanceLabelOnLine { get; set; }

        public int Distance { get; set; }

        public TopLabelWithTitleDecorator CursorLabelDecorator { get; set; }
        public TopLabelWithTitleDecorator TopLabelDecorator { get; set; }
        public TopLabelWithTitleDecorator DistanceLabelDecorator { get; set; }
        public TopLabelWithTitleDecorator DistanceLabelOnLineDecorator { get; set; }
        public IBrush CursorCircleBrush { get; set; }
        public IBrush LineBrush { get; set; }

        public float DistanceLabelXRatio { get; set; }
        public float DistanceLabelYRatio { get; set; }
        public float DistanceLabelWRatio { get; set; }
        public float DistanceLabelHRatio { get; set; }

        public float DistanceLabelOnLineWRatio { get; set; }
        public float DistanceLabelOnLineHRatio { get; set; }

        public float TopLabelXRatio { get; set; }
        public float TopLabelYRatio { get; set; }
        public float TopLabelWRatio { get; set; }
        public float TopLabelHRatio { get; set; }

        public float CursorLabelXOffset { get; set; }
        public float CursorLabelYOffset { get; set; }
        public float CursorLabelWRatio { get; set; }
        public float CursorLabelHRatio { get; set; }

        public MonsterDensityAroundCursor()
        {
            Enabled = true;
        }




        public override void Load(IController hud)
        {
            base.Load(hud);
            CursorCircleBrush = Hud.Render.CreateBrush(200, 255, 255, 255, 4);

            DrawCursorCircle = true;
            DrawCursorLabel = true;
            DrawTopLabel = true;
            DrawCursorLine = false;
            DrawDistanceLabel = true;
            DrawDistanceLabelOnLine = false;
            Distance = 12;

            TopLabelDecorator = new TopLabelWithTitleDecorator(Hud)
            {
                TextFont = hud.Render.CreateFont("tahoma", 9, 255, 255, 255, 255, false, false, true),
                BorderBrush = Hud.Render.CreateBrush(255, 255, 255, 255, -1),
                BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
            };
            TopLabelXRatio = 0.44f;
            TopLabelYRatio = 0.001f;
            TopLabelWRatio = 0.05f;
            TopLabelHRatio = 0.015f;

            DistanceLabelDecorator = new TopLabelWithTitleDecorator(Hud)
            {
                TextFont = hud.Render.CreateFont("tahoma", 9, 255, 255, 255, 255, false, false, true),
                BorderBrush = Hud.Render.CreateBrush(255, 255, 255, 255, -1),
                BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
            };
            DistanceLabelXRatio = 0.5f;
            DistanceLabelYRatio = 0.35f;
            DistanceLabelWRatio = 0.045f;
            DistanceLabelHRatio = 0.015f;

            CursorLabelDecorator = new TopLabelWithTitleDecorator(Hud)
            {
                TextFont = hud.Render.CreateFont("tahoma", 9, 255, 255, 255, 255, false, false, true),
                BorderBrush = Hud.Render.CreateBrush(255, 255, 255, 255, -1),
                BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
            };
            CursorLabelXOffset = -10f;
            CursorLabelYOffset = 35f;
            CursorLabelWRatio = 0.025f;
            CursorLabelHRatio = 0.015f;

            DistanceLabelOnLineDecorator = new TopLabelWithTitleDecorator(Hud)
            {
                TextFont = hud.Render.CreateFont("tahoma", 9, 255, 255, 255, 255, false, false, true),
                BorderBrush = Hud.Render.CreateBrush(255, 255, 255, 255, -1),
                BackgroundBrush = Hud.Render.CreateBrush(150, 0, 0, 0, 0),
            };
            DistanceLabelOnLineWRatio = 0.045f;
            DistanceLabelOnLineHRatio = 0.015f;

            LineBrush = Hud.Render.CreateBrush(255, 255, 255, 255, 3.0f);
        }

        public void PaintWorld(WorldLayer layer)
        {
            if (!Hud.Game.Me.IsInTown)
            {
                IScreenCoordinate coord = Hud.Window.CreateScreenCoordinate(Hud.Window.CursorX, Hud.Window.CursorY);
                IWorldCoordinate cursor = coord.ToWorldCoordinate();
                int count = 0;
                float x, y, width, height = 0;
                foreach (IMonster monster in Hud.Game.AliveMonsters)
                {
                    if (monster.FloorCoordinate.XYDistanceTo(cursor) < Distance) count++;
                }
                if (DrawCursorCircle)
                {
                    CursorCircleBrush.DrawWorldEllipse(Distance, -1, cursor);
                }
                if (DrawCursorLabel)
                {
                    width = Hud.Window.Size.Height * CursorLabelWRatio;
                    height = Hud.Window.Size.Height * CursorLabelHRatio;
                    CursorLabelDecorator.Paint(coord.X + CursorLabelXOffset, coord.Y + CursorLabelYOffset, width, height, count.ToString(), null, "");
                }
                if (DrawTopLabel)
                {
                    x = Hud.Window.Size.Width * TopLabelXRatio;
                    y = Hud.Window.Size.Height * TopLabelYRatio;
                    width = Hud.Window.Size.Height * TopLabelWRatio;
                    height = Hud.Window.Size.Height * TopLabelHRatio;
                    TopLabelDecorator.Paint(x - width / 2, y, width, height, count.ToString(), null, "");
                }
                if (DrawCursorLine)
                {
                    var player = Hud.Game.Me.ScreenCoordinate;
                    LineBrush.DrawLine(player.X, player.Y, coord.X, coord.Y);
                }
                if (DrawDistanceLabel)
                {
                    var distance = Hud.Game.Me.FloorCoordinate.XYDistanceTo(cursor);
                    x = Hud.Window.Size.Width * DistanceLabelXRatio;
                    y = Hud.Window.Size.Height * DistanceLabelYRatio;
                    width = Hud.Window.Size.Height * DistanceLabelWRatio;
                    height = Hud.Window.Size.Height * DistanceLabelHRatio;
                    TopLabelDecorator.Paint(x - width / 2, y, width, height, distance.ToString("F1", CultureInfo.InvariantCulture));
                }
                if (DrawDistanceLabelOnLine)
                {
                    var distance = Hud.Game.Me.FloorCoordinate.XYDistanceTo(cursor);
                    var meScreenCoord = Hud.Game.Me.ScreenCoordinate;
                    x = (meScreenCoord.X + coord.X) / 2;
                    y = (meScreenCoord.Y + coord.Y) / 2;
                    width = Hud.Window.Size.Height * DistanceLabelOnLineWRatio;
                    height = Hud.Window.Size.Height * DistanceLabelOnLineHRatio;
                    DistanceLabelOnLineDecorator.Paint(x - width / 2, y, width, height, distance.ToString("F1", CultureInfo.InvariantCulture));
                }
            }
        }

    }

}
