﻿using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CalamityEntropy.Common
{
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        [Header("Misc")]
        [SliderColor(224, 165, 56, 128)]
        [Range(0f, 1920f)]
        [DefaultValue(900f)]
        [Increment(1f)]
        public float VoidChargeBarX { get; set; }

        [SliderColor(224, 165, 56, 128)]
        [Range(0f, 1080f)]
        [DefaultValue(100f)]
        [Increment(1f)]
        public float VoidChargeBarY { get; set; }

        [DefaultValue(true)]
        public bool ItemAdditionalInfo { get; set; }

        [DefaultValue(true)]
        public bool ScreenWarpEffects { get; set; }

        [DefaultValue(true)]
        public bool ChainsawShakeScreen { get; set; }

        [DefaultValue(1f)]
        [Range(0f, 2f)]
        [Increment(0.05f)]
        public float EntropyMeleeWeaponSoundVolume { get; set; }


        [DefaultValue(true)]
        public bool MariviumArmorSetOnlyProvideStealthBarWhenHoldingRogueWeapons { get; set; }

        [Header("Other")]

        [DefaultValue(false)]
        public bool BindingOfIsaac_Rep_BossMusic { get; set; }

        [DefaultValue(false)]
        public bool RepBossMusicReplaceCalamityMusic { get; set; }

        

    }
}
