﻿using CalamityMod.Cooldowns;
using Terraria.Audio;
using Terraria.Localization;

namespace CalamityEntropy.Content.Cooldowns
{
    public class AnimasolaCd : CooldownHandler
    {
        public static new string ID => "AnimasolaCd";
        public override bool ShouldDisplay => true;
        public override LocalizedText DisplayName => Language.GetOrRegister("Mods.CalamityEntropy.CdAS");
        public override string Texture => "CalamityEntropy/Content/Cooldowns/AnimasolaCd";
        public override Color OutlineColor => new Color(210, 130, 160);
        public override Color CooldownStartColor => new Color(200, 111, 145);
        public override Color CooldownEndColor => new Color(200, 111, 145);

        public override SoundStyle? EndSound => new("CalamityEntropy/Assets/Sounds/chspawn");
    }
}
