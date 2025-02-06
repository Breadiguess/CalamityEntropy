﻿using CalamityEntropy.Common;
using CalamityEntropy.Content.ArmorPrefixes;
using CalamityEntropy.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace CalamityEntropy.Content.UI
{
    public class ArmorForgingStationUI : UIState
    {
        public UIImageButton reforgeButton;
        public int ReforgeCD = 0;
        Terraria.Item[] item;
        UIText text;
        public override void OnInitialize()
        {
            if (Main.dedServ)
            {
                return;
            }
            //实例化一个面板
            UIPanel panel = new UIPanel();
            //设置面板的宽度
            panel.Width.Set(180f, 0f);
            //设置面板的高度
            panel.Height.Set(100f, 0f);
            //设置面板距离屏幕最左边的距离
            panel.Left.Set(140f, 0f);
            //设置面板距离屏幕最上端的距离
            panel.Top.Set(-170f, 0.5f);
            //将这个面板注册到UIState
            Append(panel);
            panel.BackgroundColor = Color.Transparent;
            panel.BorderColor = Color.Transparent;

            text = new UIText("", 1, false);
            text.Width.Set(10, 0);
            text.Height.Set(2, 0);
            text.Left.Set(60, 0);
            text.Top.Set(56, 0);
            text.TextColor = Color.Gold;
            panel.Append(text);

            //用tr原版图片实例化一个图片按钮
            reforgeButton = new UIImageButton(ModContent.Request<Texture2D>("CalamityEntropy/Content/UI/reforge1"));
            //设置按钮距宽度
            reforgeButton.Width.Set(30f, 0f);
            //设置按钮高度
            reforgeButton.Height.Set(30f, 0f);
            reforgeButton.Left.Set(2f, 0f);
            reforgeButton.Top.Set(20f, 0f);
            reforgeButton.OnLeftMouseDown += reforge;
            reforgeButton.SetVisibility(1, 1);

            reforgeButton.SetHoverImage(ModContent.Request<Texture2D>("CalamityEntropy/Content/UI/reforge2"));
            //将按钮注册入面板中，这个按钮的坐标将以面板的坐标为基础计算
            panel.Append(reforgeButton);

            item = new Terraria.Item[1];
            item[0] = new Item();
            item[0].TurnToAir();
            UIItemSlot itemSlot = new UIItemSlot(item, 0, 0);
            
            itemSlot.Left.Set(30, 0f);
            itemSlot.Top.Set(12, 0f);
            panel.Append(itemSlot);

            base.OnInitialize();
        }
        public override void Update(GameTime gameTime)
        {
            if (Main.dedServ)
            {
                return;
            }
            if (item[0].active && Util.Util.IsArmor(item[0]))
            {
                text.SetText(Main.ValueToCoins((int)(item[0].value / 6)));
            }
            else
            {
                text.SetText("");
            }
            ReforgeCD--;
            if (reforgeButton.IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            base.Update(gameTime);
        }
        public static bool Visible = false;
        private void reforge(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.isMouseLeftConsumedByUI = true;
            Main.mouseLeft = false;
            EModSys.noItemUse = true;
            if ((!item[0].active) || (!Util.Util.IsArmor(item[0])) || ReforgeCD > 0)
            {
                return;

            }
            if(!Main.LocalPlayer.BuyItem(item[0].value / 6))
            {
                return;
            }
            SoundStyle s = new SoundStyle("CalamityEntropy/Assets/Sounds/Reforge");
            SoundEngine.PlaySound(s);
            if (Main.rand.NextBool(ArmorPrefix.instances.Count + 1))
            {
                item[0].Entropy().armorPrefix = null;
                item[0].Entropy().armorPrefixName = string.Empty;
                CombatText.NewText(Main.LocalPlayer.getRect(), Color.Blue, item[0].Name);
                return;
            }
            
            ArmorPrefix armorPrefix = ArmorPrefix.RollPrefixToItem(item[0]);
            if (armorPrefix != null)
            {
                if (armorPrefix.Dramatic())
                {
                    ReforgeCD = 60;
                    SoundEngine.PlaySound(new SoundStyle("CalamityEntropy/Assets/Sounds/AscendantActivate"));
                }
                item[0].Entropy().armorPrefix = armorPrefix;
                item[0].Entropy().armorPrefixName = armorPrefix.RegisterName();
                CombatText.NewText(Main.LocalPlayer.getRect(), armorPrefix.getColor(), armorPrefix.GivenName + " " + item[0].Name, armorPrefix.Dramatic());
            }
        }
        public void close()
        {
            if (item[0].type != ItemID.None)
            {
                Main.LocalPlayer.QuickSpawnItem(Player.GetSource_None(), item[0]);
                item[0].TurnToAir();
            }
        }
    }
}
