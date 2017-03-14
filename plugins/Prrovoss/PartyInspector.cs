using Turbo.Plugins.Default;
using SharpDX.DirectInput;
using SharpDX;
using System.Linq;
using System.Collections.Generic;

namespace Turbo.Plugins.Prrovoss
{

    public class PartyInspector : BasePlugin, IInGameWorldPainter, IKeyEventHandler
    {
        public bool Show { get; set; }
        public IKeyEvent ToggleKeyEvent { get; set; }
        public float SkillRatio { get; set; }
        public float GemRatio { get; set; }
        public float KanaiRatio { get; set; }
        public SkillPainter SkillPainter { get; set; }
        public Dictionary<uint, uint> LegendaryGemItemIDs { get; set; }
        public float XOffset { get; set; }
        public float YOffset { get; set; }
        public float Gap { get; set; }
        public bool DrawKanai { get; set; }
        public bool DrawGems { get; set; }
        public bool DrawSkills { get; set; }
        public List<int> ElementOrder { get; set; }

        private float currentX { get; set; }

        public PartyInspector()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            Show = true;
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
                    Hud.Render.CreateBrush(0, 255, 6, 0, 0),
                    Hud.Render.CreateBrush(0, 183, 57, 7, 0),
                    Hud.Render.CreateBrush(0, 0, 38, 119, 0),
                    Hud.Render.CreateBrush(0, 77, 102, 155, 0),
                    Hud.Render.CreateBrush(0, 50, 106, 21, 0),
                    Hud.Render.CreateBrush(0, 138, 0, 94, 0),
                    Hud.Render.CreateBrush(0, 190, 117, 0, 0),
                },
                SkillDpsFont = Hud.Render.CreateFont("tahoma", 7, 0, 255, 255, 255, false, false, 0, 0, 0, 0, false),
            };
            SkillRatio = 0.025f;
            KanaiRatio = 0.025f;
            GemRatio = 0.0166666666666667f;

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

            XOffset = Hud.Window.Size.Width * 0.16f;
            YOffset = Hud.Window.Size.Width * 0.012f;
            Gap = Hud.Window.Size.Width * 0.012f;

            DrawGems = true;
            DrawKanai = true;
            DrawSkills = true;

            ElementOrder = new List<int>(new int[] { 0, 1, 2 });
        }

        public void PaintWorld(WorldLayer layer)
        {
            if (Show)
            {
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
                                    if (player.CubeSnoItem1 != null) DrawKanaiItem(player.CubeSnoItem1, player.PortraitUiElement.Rectangle);
                                    if (player.CubeSnoItem2 != null) DrawKanaiItem(player.CubeSnoItem2, player.PortraitUiElement.Rectangle);
                                    if (player.CubeSnoItem3 != null) DrawKanaiItem(player.CubeSnoItem3, player.PortraitUiElement.Rectangle);
                                    currentX += Gap;
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
                        }
                    }
                }
            }
        }

        private void DrawLegendaryGems(IPlayer player)
        {
            IEnumerable<IBuff> gemBuffs = LegendaryPowerInfoExtensions.EquippedLegendaryGemsPrimaryBuffs(player.Powers.UsedLegendaryPowers).Where(b => b.Active);
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

                    var y = portraitRect.Y + YOffset;

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
                    var y = portraitRect.Y + YOffset + size;

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


        private void DrawKanaiItem(ISnoItem snoItem, System.Drawing.RectangleF portraitRect)
        {
            var inventoryRect = Hud.Inventory.InventoryMainUiElement.Rectangle;

            var itemRect = new System.Drawing.RectangleF(currentX, portraitRect.Y, Hud.Window.Size.Width * KanaiRatio, Hud.Window.Size.Width * KanaiRatio * 2);
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
                var description = snoItem.NameLocalized;

                var power = snoItem.LegendaryPower;
                if (power != null)
                {
                    description += "\n\n" + power.DescriptionLocalized;
                }

                Hud.Render.SetHint(description);
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


        public void OnKeyEvent(IKeyEvent keyEvent)
        {
            if (keyEvent.IsPressed && ToggleKeyEvent.Matches(keyEvent))
            {
                Show = !Show;
            }
        }
    }
}
