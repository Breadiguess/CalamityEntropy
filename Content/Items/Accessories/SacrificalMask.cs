﻿using CalamityEntropy.Content.Projectiles;
using CalamityMod;
using CalamityMod.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Accessories
{
    public class SacrificalMask : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 86;
            Item.height = 86;
            Item.value = CalamityGlobalItem.RarityRedBuyPrice;
            Item.rare = ItemRarityID.Red;
            Item.Entropy().stroke = true;
            Item.Entropy().strokeColor = Color.Red;
            Item.Entropy().NameColor = Color.Black;
            Item.accessory = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SacrificalDagger>()] < 8)
            {
                Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, ModContent.ProjectileType<SacrificalDagger>(), (int)player.GetTotalDamage<AverageDamageClass>().ApplyTo(60), 1, player.whoAmI);
            }
            player.Entropy().sacrMask = true;
        }

        public override void AddRecipes()
        {
        }
    }
}
