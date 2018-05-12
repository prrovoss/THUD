using Turbo.Plugins.Default;
using System.Collections.Generic;

namespace Turbo.Plugins.Prrovoss.Popups
{

    public class ItemDroppedPopup : BasePlugin, ILootGeneratedHandler
    {
        public class MyItem
        {
            public uint? SNO { get; set; }
            public string Hint { get; set; }
            public string Title { get; set; }
            public int Duration { get; set; }
            public ItemQuality? Quality { get; set; }
            public int? AncientRank { get; set; }
            public TopLabelWithTitleDecorator Decorator { get; set; }

            public MyItem(uint? sno, ItemQuality? quality, int? ancientRank, string hint, string title, int duration, TopLabelWithTitleDecorator decorator = null)
            {
                this.SNO = sno;
                this.Title = title;
                this.Duration = duration;
                this.Quality = quality;
                this.AncientRank = ancientRank;
                this.Decorator = decorator;
            }
        }

        public List<MyItem> ItemsToWatch { get; set; }

        public void Add (uint? sno, ItemQuality? quality, int? ancientRank, string hint, string title, int duration, TopLabelWithTitleDecorator decorator = null) {
            this.ItemsToWatch.Add(new MyItem(sno, quality, ancientRank, hint, title, duration, decorator));
        }

        public void OnLootGenerated(IItem item, bool gambled)
        {            
            foreach (MyItem myItem in ItemsToWatch)
            {
                if (ItemMatchCheck(item, myItem))
                {
                    Hud.RunOnPlugin<PopupNotifications>(plugin =>
                    {
                        plugin.Show(item.SnoItem.NameLocalized, myItem.Title, myItem.Duration, myItem.Hint, myItem.Decorator);
                    });
                }
            }
        }

        private bool ItemMatchCheck(IItem droppedItem, MyItem itemToCheck)
        {

            bool resultSNO = (itemToCheck.SNO == null ? true : (itemToCheck.SNO == droppedItem.SnoItem.Sno) );
            bool resultQuality = (itemToCheck.Quality == null ? true : (itemToCheck.Quality == droppedItem.Quality)); 
            bool resultAncientRank = (itemToCheck.AncientRank == null ? true : (itemToCheck.AncientRank == droppedItem.AncientRank));
            
            return (resultSNO && resultQuality && resultAncientRank);
        }

        public ItemDroppedPopup()
        {
            Enabled = true;
            ItemsToWatch = new List<MyItem>();
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
        }

    }

}