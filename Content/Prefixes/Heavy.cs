﻿using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Prefixes
{
    public class Heavy : ModPrefix
    {

        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override float RollChance(Item item)
        {
            return 0.8f;
        }

        public override bool CanRoll(Item item)
        {
            return true;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {

        }
        public override void ApplyAccessoryEffects(Player player)
        {
            player.Entropy().FallSpeed += 0.1f;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult *= 1.05f;
        }

        public override void Apply(Item item)
        {
        }
        public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
        {

            yield return new TooltipLine(Mod, "PrefixWeaponAwesomeDescription", AdditionalTooltip.Value)
            {
                IsModifier = true,
            };
        }


        public LocalizedText AdditionalTooltip => Mod.GetLocalization($"PrefixHeavyDesc");

        public override void SetStaticDefaults()
        {
            _ = AdditionalTooltip;
        }
    }
}
