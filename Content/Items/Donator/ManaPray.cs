﻿using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Donator
{
    public class ManaPray : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
        }


        public override void Update(Player player, ref int buffIndex)
        {
            player.Entropy().lifeRegenPerSec += 2;
        }
    }
}
