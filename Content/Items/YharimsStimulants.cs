using CalamityEntropy.Content.Buffs;
using CalamityMod;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items;

public class YharimsStimulants : ModItem
{
        //Yharim's Stimulants
        //Gives decent buffs to ALL offensive and defensive stats


    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;
        Item.useTurn = true;
        Item.maxStack = 30;
        Item.rare = ItemRarityID.Orange;
        Item.useAnimation = 17;
        Item.useTime = 17;
        Item.useStyle = ItemUseStyleID.EatFood;
        Item.UseSound = SoundID.Item3;
        Item.consumable = true;
        Item.buffType = ModContent.BuffType<YharimPower>();
        Item.buffTime = CalamityUtils.SecondsToFrames(1800f);
        Item.value = Item.buyPrice(0, 2, 0, 0);
    }
}
