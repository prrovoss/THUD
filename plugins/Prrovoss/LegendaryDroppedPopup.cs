using Turbo.Plugins.Default;

namespace Turbo.Plugins.Prrovoss
{

    public class LegendaryDroppedPopup : BasePlugin, ILootGeneratedHandler
    {

        public void OnLootGenerated(IItem item, bool gambled)
        {
           if (item.AncientRank >= 1 && item.SnoItem.Sno==2059435674)
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