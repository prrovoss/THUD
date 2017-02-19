using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{

    public class LegendaryDroppedPopup : BasePlugin, ILootGeneratedHandler
    {

        public void OnLootGenerated(IItem item, bool gambled)
        {
            if (item.Quality >= ItemQuality.Legendary)
                Hud.RunOnPlugin<PopupNotifications>(plugin =>
                    {
                        plugin.Show(item.SnoItem.NameLocalized + (item.AncientRank == 1 ? " (A)" : (item.AncientRank == 2 ? " (P)" : "")) , "Legendary dropped", 10000, "Hurray");
                    });
        }

        public LegendaryDroppedPopup()
        {
            Enabled = true;
        }

    }

}