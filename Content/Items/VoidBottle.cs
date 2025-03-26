﻿using CalamityEntropy.Content.NPCs.Cruiser;
using CalamityEntropy.Content.Projectiles;
using CalamityMod.Events;
using CalamityMod.Items.Materials;
using CalamityMod.Rarities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items
{
    public class VoidBottle : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 17;
        }
        public override void SetDefaults()
        {
            Item.width = 56;
            Item.height = 56;
            Item.useAnimation = 32;
            Item.useTime = 32;
            Item.noUseGraphic = true;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = false;
            Item.rare = ModContent.RarityType<Turquoise>();

        }
        public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossItem;
        }

        public override bool CanUseItem(Player player)
        {
            return !NPC.AnyNPCs(ModContent.NPCType<CruiserHead>()) && !BossRushEvent.BossRushActive;
        }

        public override bool? UseItem(Player player)
        {
            int type = ModContent.NPCType<CruiserHead>();
            if (Main.netMode != NetmodeID.MultiplayerClient)
                NPC.SpawnOnPlayer(player.whoAmI, type);
            else
                NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
            if (player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.One) * 5.5f, ModContent.ProjectileType<VoidBottleThrow>(), 0, 0, player.whoAmI);
            }
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient(ModContent.ItemType<DarkPlasma>(), 3).
                AddIngredient(ItemID.Bottle, 1).
                AddTile(TileID.WorkBenches).
                Register();
        }
    }
}