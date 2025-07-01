﻿using CalamityOverhaul.Content;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalamityEntropy.Common
{
    internal static partial class CWRWeakRef
    {
        [JITWhenModsEnabled("CalamityOverhaul")]
        internal static class CWRRef
        {
            public static float GetPlayersPressure(Player plr)
            {
                return plr.GetModPlayer<CWRPlayer>().PressureIncrease;
            }
        }
    }
}
