using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{

    public class MouseCircle : BasePlugin, IInGameWorldPainter 
    {
        private IBrush gBrush;
        private IBrush rBrush;

        public MouseCircle()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            gBrush = Hud.Render.CreateBrush(255, 0, 255, 0, 5);
            rBrush = Hud.Render.CreateBrush(255, 255, 0, 0, 5);
        }

        public void PaintWorld(WorldLayer layer)
        {
            if (!Hud.Game.Me.IsInTown)
            {
                gBrush.DrawEllipse(Hud.Window.CursorX, Hud.Window.CursorY, 9, 9, 0);
                rBrush.DrawEllipse(Hud.Window.CursorX, Hud.Window.CursorY, 18, 18, 0);
                gBrush.DrawEllipse(Hud.Window.CursorX, Hud.Window.CursorY, 27, 27, 0);
            }
        }
    }
}