﻿using CalamityEntropy.Content.Projectiles;
using CalamityMod.Items;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Books.BookMarks
{
    public class BookMarkOfNight : BookMark
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Pink;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
        }
        public override Texture2D UITexture => BookMark.GetUITexture("Night");
        public override void ModifyStat(EBookStatModifer modifer)
        {
            modifer.Damage += 0.1f;
        }
        public override Color tooltipColor => Color.DarkRed;
        public override EBookProjectileEffect getEffect()
        {
            return new DarkSoulBMEffect();
        }
    }
    public class DarkSoulBMEffect : EBookProjectileEffect
    {
        public override void OnHitNPC(Projectile projectile, NPC target, int damageDone)
        {
            if (Main.rand.NextBool(projectile.HasEBookEffect<APlusBMEffect>() ? 4 : 6))
            {
                (Projectile.NewProjectile(projectile.GetSource_FromThis(), target.Center, CEUtils.randomRot().ToRotationVector2() * Main.rand.NextFloat(30, 34), ModContent.ProjectileType<DarkSoul>(), damageDone / 2, projectile.knockBack / 3, projectile.owner).ToProj().ModProjectile as EBookBaseProjectile).homing = ((EBookBaseProjectile)projectile.ModProjectile).homing;
            }
        }
    }
}