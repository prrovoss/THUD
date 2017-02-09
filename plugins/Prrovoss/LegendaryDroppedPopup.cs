using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{

    public class LegendaryDroppedPopup : BasePlugin, ILootGeneratedHandler
    {

        public void OnLootGenerated(IItem item, bool gambled)
        {
            if (item.Quality >= ItemQuality.Legendary)
                Hud.GetPlugin<PopupNotifications>().Show(item.SnoItem.NameLocalized + (item.AncientRank == 1 ? " (A)" : ""), "Legendary dropped", 10000, "my hint");
        }

        public LegendaryDroppedPopup()
        {
            Enabled = true;
        }

    }

}
