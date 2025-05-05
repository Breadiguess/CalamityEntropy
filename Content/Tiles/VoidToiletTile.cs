﻿using CalamityEntropy.Content.Items;
using CalamityEntropy.Content.Projectiles;
using CalamityMod;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace CalamityEntropy.Content.Tiles
{
    public class VoidToiletTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            RegisterItemDrop(ModContent.ItemType<VoidToilet>());

            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.Width = 2;
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.DrawYOffset = 4;
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.Origin = new Point16(1, 1);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 2, 0);
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
            TileID.Sets.DisableSmartCursor[Type] = true;
            AddMapEntry(new Color(162, 162, 255), Language.GetText("MapObject.Toilet"));
            TileID.Sets.CanBeSatOnForNPCs[Type] = true;
            TileID.Sets.CanBeSatOnForPlayers[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, DustID.Firework_Blue, 0f, 0f, 1, new Color(255, 255, 255), 1f);
            type = DustID.BlueCrystalShard;
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 4;
        }
        public override void ModifySittingTargetInfo(int i, int j, ref TileRestingInfo info) => CalamityUtils.ChairSitInfo(i, j, ref info, 40, true, shitter: true);

        public override bool RightClick(int i, int j)
        {
            Player plr = Main.LocalPlayer;
            CalamityUtils.ChairRightClick(i, j);

            return true;
        }

        public override void MouseOver(int i, int j) => CalamityUtils.ChairMouseOver(i, j, ModContent.ItemType<VoidToilet>(), true);

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return settings.player.IsWithinSnappngRangeToTile(i, j, PlayerSittingHelper.ChairSittingMaxDistance);
        }

        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];

            int spawnX = i;
            int spawnY = j - (tile.TileFrameY % 40) / 18;

            Wiring.SkipWire(spawnX, spawnY);
            Wiring.SkipWire(spawnX, spawnY + 1);

            if (Wiring.CheckMech(spawnX, spawnY, 60))
            {
                SoundEngine.PlaySound(new SoundStyle("CalamityEntropy/Assets/Sounds/void_laser"), new Vector2(spawnX * 16 + 8, spawnY * 16 + 12));
                Projectile.NewProjectile(Wiring.GetProjectileSource(spawnX, spawnY), spawnX * 16 + 8, spawnY * 16 + 12, 0f, 0f, ProjectileID.ToiletEffect, 0, 0f, Main.myPlayer);
                Projectile.NewProjectile(Wiring.GetProjectileSource(spawnX, spawnY), spawnX * 16 + 8, spawnY * 16 + 6 * 16, 0f, -8f, ModContent.ProjectileType<ToiletLaser>(), 1100, 10f, Main.myPlayer);
                for (int i_ = 0; i_ < 64; i_++)
                {
                    Projectile.NewProjectile(Wiring.GetProjectileSource(spawnX, spawnY), spawnX * 16 + 8, spawnY * 16, Main.rand.NextFloat(-24, 24), Main.rand.NextFloat(-20, -36), ModContent.ProjectileType<ToiletVoidPoop>(), 400, 10f, Main.myPlayer);
                }
            }
        }
    }
}
