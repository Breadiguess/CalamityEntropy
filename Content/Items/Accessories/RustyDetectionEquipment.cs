﻿using CalamityMod.Items;
using CalamityMod.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Accessories
{
    public class RustyDetectionEquipment : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.defense = 4;
            Item.height = 46;
            Item.value = CalamityGlobalItem.RarityOrangeBuyPrice;
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;

        }
        public static string ID = "RustyDetectorEquipment";

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rocketBoots += 10;
            player.noFallDmg = true;
            player.jumpSpeedBoost += 1.6f;
            player.maxRunSpeed *= 1.1f;
            player.Entropy().addEquip(ID, !hideVisual);
        }
        public override void UpdateVanity(Player player)
        {
            player.Entropy().addEquipVisual(ID);
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<DubiousPlating>(), 20).
                AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 15).
                AddIngredient(ModContent.ItemType<SuspiciousScrap>(), 1).
                AddTile(TileID.Anvils).
                Register();
        }
    }
}
