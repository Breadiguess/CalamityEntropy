﻿using CalamityEntropy.Content.Rarities;
using CalamityEntropy.Content.Tiles;
using CalamityMod.Tiles.Furniture.CraftingStations;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items
{
    public class VoidToilet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 30;
            Item.maxStack = 9999;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<VoidToiletTile>();
            Item.rare = ModContent.RarityType<VoidPurple>();
        }

        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ItemID.Toilet).
                AddIngredient<VoidBar>(5).
                AddTile<VoidCondenser>().
                Register();
        }
    }
}
