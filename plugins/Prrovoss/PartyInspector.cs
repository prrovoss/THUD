using Turbo.Plugins.Default;
using SharpDX.DirectInput;
using SharpDX;
using System.Linq;
using System.Collections.Generic;
using Turbo.Plugins.Jack.Extensions;
using System;
using System.Globalization;

namespace Turbo.Plugins.Prrovoss
{

    public class PartyInspector : BasePlugin, IInGameTopPainter, IKeyEventHandler
    {
        public bool Show { get; set; }
        public IKeyEvent ToggleKeyEvent { get; set; }
        public float SkillRatio { get; set; }
        public float GemRatio { get; set; }
        public float KanaiRatio { get; set; }
        public float ItemRatio { get; set; }
        public SkillPainter SkillPainter { get; set; }
        public Dictionary<uint, uint> LegendaryGemItemIDs { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }
        public float Gap { get; set; }
        public bool DrawKanai { get; set; }
        public bool DrawLegendaryItems { get; set; }
        public bool DrawGems { get; set; }
        public bool DrawSkills { get; set; }
        public bool DrawMetaInformations { get; set; }
        public List<int> ElementOrder { get; set; }
        public List<MetaElement> UpperMeta = new List<MetaElement>();
        public List<MetaElement> LowerMeta = new List<MetaElement>();

        public TopLabelWithTitleDecorator LabelDecorator { get; set; }

        private float currentX { get; set; }

        public PartyInspector()
        {
            Enabled = true;
            Order = 99;
        }


        public class MetaElement
        {
            public string Hint { get; set; }
            public string Title { get; set; }
            public Func<IPlayer, string> Text { get; set; }

            public MetaElement(Func<IPlayer, string> text, string title = null, string hint = null)
            {
                Hint = hint;
                Title = title;
                Text = text;
            }
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            Show = false;
            ToggleKeyEvent = Hud.Input.CreateKeyEvent(true, Key.F8, false, false, false);

            LegendaryGemItemIDs = new Dictionary<uint, uint>();

            SkillPainter = new SkillPainter(Hud, true)
            {
                TextureOpacity = 1.0f,
                EnableSkillDpsBar = true,
                EnableDetailedDpsHint = true,
                CooldownFont = Hud.Render.CreateFont("arial", 8, 255, 255, 255, 255, true, false, 255, 0, 0, 0, true),
                SkillDpsBackgroundBrushesByElementalType = new IBrush[]
                {
                    Hud.Render.CreateBrush(222, 255, 6, 0, 0),
                    Hud.Render.CreateBrush(222, 183, 57, 7, 0),
                    Hud.Render.CreateBrush(222, 0, 38, 119, 0),
                    Hud.Render.CreateBrush(222, 77, 102, 155, 0),
                    Hud.Render.CreateBrush(222, 50, 106, 21, 0),
                    Hud.Render.CreateBrush(222, 138, 0, 94, 0),
                    Hud.Render.CreateBrush(222, 190, 117, 0, 0),
                },
                SkillDpsFont = Hud.Render.CreateFont("tahoma", 7, 222, 255, 255, 255, false, false, 0, 0, 0, 0, false),
            };
            SkillRatio = 0.025f;
            KanaiRatio = 0.025f;
            ItemRatio = 0.025f;
            GemRatio = 0.0166666666666667f;

            LabelDecorator = new TopLabelWithTitleDecorator(Hud)
            {
                BorderBrush = Hud.Render.CreateBrush(255, 180, 147, 109, -1),
                BackgroundBrush = Hud.Render.CreateBrush(128, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 6, 255, 255, 210, 150, false, false, false),
                TitleFont = Hud.Render.CreateFont("tahoma", 6, 255, 180, 147, 109, false, false, false),
            };

            LegendaryGemItemIDs.Add(428348, 3249948847); //Stricken
            LegendaryGemItemIDs.Add(383014, 3248511367); //BotP
            LegendaryGemItemIDs.Add(403456, 3248547304); //BotT
            LegendaryGemItemIDs.Add(403470, 3249805099); //Hoarder
            LegendaryGemItemIDs.Add(428352, 3250847272); //Boyarskys
            LegendaryGemItemIDs.Add(403466, 3249661351); //Enforcer
            LegendaryGemItemIDs.Add(428029, 3249876973); //Esoteric
            LegendaryGemItemIDs.Add(403459, 3248583241); //Ease
            LegendaryGemItemIDs.Add(403461, 3248655115); //Toxin
            LegendaryGemItemIDs.Add(403464, 3248762926); //Gogok
            LegendaryGemItemIDs.Add(428354, 3250883209); //Iceblink
            LegendaryGemItemIDs.Add(403465, 3248798863); //Invigoration
            LegendaryGemItemIDs.Add(403463, 3248726989); //Mirinae
            LegendaryGemItemIDs.Add(428031, 3249912910); //Gizzard
            LegendaryGemItemIDs.Add(403467, 3249697288); //Moratorium
            LegendaryGemItemIDs.Add(428350, 3249984784); //Mutilation
            LegendaryGemItemIDs.Add(403462, 3248691052); //PE
            LegendaryGemItemIDs.Add(454736, 3250919146); //Soul Shard
            LegendaryGemItemIDs.Add(403469, 3249769162); //Simplicitys
            LegendaryGemItemIDs.Add(403471, 3249841036); //Taeguk
            LegendaryGemItemIDs.Add(403460, 3248619178); //WoL
            LegendaryGemItemIDs.Add(403468, 3249733225); //Zeis

            XOffset = Hud.Window.Size.Width * 0.14f;
            YOffset = Hud.Window.Size.Width * 0.012f;
            Gap = Hud.Window.Size.Width * 0.012f;

            DrawGems = true;
            DrawKanai = true;
            DrawSkills = true;
            DrawLegendaryItems = true;
            DrawMetaInformations = true;

            ElementOrder = new List<int>(new int[] { 0, 1, 2, 3, 4 });

            UpperMeta.Add(new MetaElement(((p) => p.Offense.AttackSpeed.ToString("0.000")), "AS", "Attack Speed"));
            UpperMeta.Add(new MetaElement(((p) => p.Offense.AreaDamageBonus.ToString("F0", CultureInfo.InvariantCulture) + "%"), "AD", "Area Damage"));
            UpperMeta.Add(new MetaElement(((p) => (p.Stats.CooldownReduction * 100).ToString("F2", CultureInfo.InvariantCulture) + "%"), "CDR", "Cooldown reduction"));
            UpperMeta.Add(new MetaElement(((p) => (p.Stats.ResourceCostReduction * 100).ToString("F2", CultureInfo.InvariantCulture) + "%"), "RCR", "Resource cost reduction"));

            LowerMeta.Add(new MetaElement(((p) => ValueToString(p.Defense.EhpMax, ValueFormat.ShortNumber)), "EHP", "Effective health pool"));
            LowerMeta.Add(new MetaElement(((p) => ValueToString(p.Defense.LifeRegen, ValueFormat.ShortNumber)), "LPS", "Life per second"));
            LowerMeta.Add(new MetaElement(((p) => ValueToString(p.Stats.PickupRange, ValueFormat.ShortNumber)), "PR", "Pickup range"));
        }



        public void PaintTopInGame(ClipState clipState)
        {
            if (clipState != ClipState.BeforeClip) return;
            if (Show)
            {
                if (XOffset == 0) XOffset = Hud.Window.Size.Width * 0.14f;
                if (YOffset == 0) YOffset = Hud.Window.Size.Width * 0.012f;
                if (Gap == 0) Gap = Hud.Window.Size.Width * 0.012f;

                foreach (IPlayer player in Hud.Game.Players)
                {
                    currentX = XOffset;

                    foreach (int element in ElementOrder)
                    {
                        switch (element)
                        {
                            case 0:
                                if (DrawKanai)
                                {
                                    DrawKanaiItems(player);
                                }
                                break;

                            case 1:
                                if (DrawGems)
                                {
                                    DrawLegendaryGems(player);
                                }
                                break;

                            case 2:
                                if (DrawSkills)
                                {
                                    DrawPlayerSkills(player);
                                }
                                break;
                            case 3:
                                if (DrawLegendaryItems)
                                {
                                    DrawBuffs(player);
                                }
                                break;
                            case 4:
                                if (DrawMetaInformations)
                                {
                                    DrawMeta(player);
                                }
                                break;
                        }
                    }

                }
            }
        }



        private void DrawMeta(IPlayer player)
        {
            var size = Hud.Window.Size.Width * SkillRatio;
            var xUpper = currentX;
            var yUpper = player.PortraitUiElement.Rectangle.Y + YOffset - player.PortraitUiElement.Rectangle.Width * 0.1f;

            var xLower = currentX;
            var yLower = player.PortraitUiElement.Rectangle.Y + YOffset + size + player.PortraitUiElement.Rectangle.Width * 0.1f;

            var width = Hud.Window.Size.Width * ItemRatio;
            var height = Hud.Window.Size.Width * ItemRatio * 0.8f;


            foreach (MetaElement element in UpperMeta)
            {
                LabelDecorator.Paint(xUpper, yUpper, size, size, element.Text(player), element.Title, element.Hint);
                xUpper += size;
            }

            foreach (MetaElement element in LowerMeta)
            {
                LabelDecorator.Paint(xLower, yLower, size, size, element.Text(player), element.Title, element.Hint);
                xLower += size;
            }

            currentX += Math.Max(LowerMeta.Count, UpperMeta.Count) * size + Gap;
        }


        private void DrawBuffs(IPlayer player)
        {
            var cubedItemSnos = player.CubedItems.Where(d => d != null).Select(d => d.Sno);

            foreach (IBuff buff in player.Powers.UsedLegendaryPowers.AllLegendaryPowerBuffs().Where(b => b.Active))
            {
                var itemSno = buff.SnoPower.GetItemSno();
                if (itemSno == 0) continue;
                if (cubedItemSnos.Contains(itemSno)) continue;

                DrawItem(Hud.Inventory.GetSnoItem(itemSno), player);
            }
            currentX += Gap;
        }

        private void DrawKanaiItems(IPlayer player)
        {
            if (player.CubeSnoItem1 != null) DrawItem(player.CubeSnoItem1, player);
            if (player.CubeSnoItem2 != null) DrawItem(player.CubeSnoItem2, player);
            if (player.CubeSnoItem3 != null) DrawItem(player.CubeSnoItem3, player);
            currentX += Gap;
        }

        private void DrawItem(ISnoItem snoItem, IPlayer player)
        {
            var portraitRect = player.PortraitUiElement.Rectangle;

            var itemRect = new System.Drawing.RectangleF(currentX, portraitRect.Y, Hud.Window.Size.Width * ItemRatio, Hud.Window.Size.Width * ItemRatio * 2);
            itemRect.Offset(0, YOffset);
            if (snoItem.ItemHeight == 1)
            {
                itemRect.Offset(0, YOffset);
                itemRect.Height /= 2;
            }

            var slotTexture = Hud.Texture.InventorySlotTexture;
            slotTexture.Draw(itemRect.X, itemRect.Y, itemRect.Width, itemRect.Height);

            if (Hud.Window.CursorInsideRect(itemRect.X, itemRect.Y, itemRect.Width, itemRect.Height))
            {
                var itemName = snoItem.NameLocalized;

                var power = snoItem.LegendaryPower;
                var powerDesc = "";
                if (power != null)
                {
                    itemName += "\n\n";
                    powerDesc = power.DescriptionLocalized;
                    var patternBegin = powerDesc.IndexOf("{c_magic}");
                    if (patternBegin != -1)
                    {
                        var patternEnd = powerDesc.IndexOf("{/c}") != -1 ? powerDesc.IndexOf("{/c}") + 4 : powerDesc.IndexOf("{/c_magic}") + 10;
                        if (patternEnd != -1)
                        {
                            var toReplace = powerDesc.Substring(patternBegin, patternEnd - patternBegin);
                            var replacement = toReplace.Contains("%") ? "X%" : "X";
                            powerDesc = powerDesc.Replace(toReplace, replacement);
                        }
                    }
                }

                Hud.Render.SetHint(itemName + powerDesc);
            }

            var backgroundTexture = snoItem.ItemHeight == 2 ? Hud.Texture.InventoryLegendaryBackgroundLarge : Hud.Texture.InventoryLegendaryBackgroundSmall;
            backgroundTexture.Draw(itemRect.X, itemRect.Y, itemRect.Width, itemRect.Height);

            var itemTexture = Hud.Texture.GetItemTexture(snoItem);
            if (itemTexture != null)
            {
                itemTexture.Draw(itemRect.X, itemRect.Y, itemRect.Width, itemRect.Height);
            }
            currentX += itemRect.Width;
        }


        private void DrawLegendaryGems(IPlayer player)
        {
            IEnumerable<IBuff> gemBuffs = player.Powers.UsedLegendaryGems.AllGemPrimaryBuffs().Where(b => b.Active);
            var size = Hud.Window.Size.Width * GemRatio;
            var portraitRect = player.PortraitUiElement.Rectangle;
            var index = 0;

            foreach (IBuff buff in gemBuffs)
            {
                if (index == 3)
                {
                    currentX += size;
                    index = 0;
                }

                var y = portraitRect.Y + YOffset + size * index;

                var rect = new RectangleF(currentX, y, size, size);

                var texture = Hud.Texture.GetItemTexture(Hud.Inventory.GetSnoItem(LegendaryGemItemIDs[buff.SnoPower.Sno]));

                if (Hud.Window.CursorInsideRect(rect.X, rect.Y, rect.Width, rect.Height))
                {
                    Hud.Render.SetHint(buff.SnoPower.NameLocalized);
                }

                if (texture != null)
                {
                    texture.Draw(rect.X, rect.Y, rect.Width, rect.Height);
                }

                index++;
            }
            currentX += size + Gap;
        }


        private void DrawPlayerSkills(IPlayer player)
        {
            var size = Hud.Window.Size.Width * SkillRatio;
            var portraitRect = player.PortraitUiElement.Rectangle;
            var index = 0;
            var passivesX = currentX;
            foreach (var skill in player.Powers.SkillSlots)
            {
                if (skill != null)
                {
                    index = skill.Key <= ActionKey.RightSkill ? (int)skill.Key + 4 : (int)skill.Key - 2;

                    var y = portraitRect.Y + YOffset - portraitRect.Width * 0.1f;

                    var rect = new RectangleF(currentX, y, size, size);

                    SkillPainter.Paint(skill, rect);

                    if (Hud.Window.CursorInsideRect(rect.X, rect.Y, rect.Width, rect.Height))
                    {
                        Hud.Render.SetHint(skill.RuneNameLocalized);
                    }

                }
                currentX += size;
            }

            index = 0;
            foreach (var skill in player.Powers.UsedPassives)
            {
                if (skill != null)
                {
                    var y = portraitRect.Y + YOffset + size + portraitRect.Width * 0.1f;

                    var rect = new RectangleF(passivesX, y, size, size);

                    var texture = Hud.Texture.GetTexture(skill.NormalIconTextureId);
                    texture.Draw(rect.X, rect.Y, rect.Width, rect.Height);

                    if (Hud.Window.CursorInsideRect(rect.X, rect.Y, rect.Width, rect.Height))
                    {
                        Hud.Render.SetHint(skill.NameLocalized);
                    }
                }
                passivesX += size * 1.666666f;
                index++;
            }
            currentX += Gap;
        }

        public void OnKeyEvent(IKeyEvent keyEvent)
        {
            if (keyEvent.IsPressed && ToggleKeyEvent.Matches(keyEvent))
            {
                Show = !Show;
            }
        }
    }
}
