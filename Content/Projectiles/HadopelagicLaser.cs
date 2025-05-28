﻿using CalamityEntropy.Utilities;
using CalamityMod.Particles;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Projectiles
{

    public class HadopelagicLaser : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 8000;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.timeLeft = 16;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.ArmorPenetration = 36;
        }
        public float width = 0;
        public int length = 3000;
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                if (Projectile.Distance(Main.LocalPlayer.Center) < 1200)
                {
                    CalamityEntropy.FlashEffectStrength = 0.46f;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (Projectile.ai[0] < 3)
            {
                width += 0.33334f;
            }
            Projectile.ai[0]++;
            if (Projectile.timeLeft < 12)
            {
                width -= 1f / 12f;
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            CalamityEntropy.Instance.screenShakeAmp = 6;
            CalamityMod.Particles.Particle pulse = new DirectionalPulseRing(target.Center, Vector2.Zero, new Color(170, 170, 255), new Vector2(2f, 2f), 0, 0.1f, 1f, 30);
            GeneralParticleHandler.SpawnParticle(pulse);

            CalamityMod.Particles.Particle explosion2 = new DetailedExplosion(target.Center, Vector2.Zero, new Color(140, 140, 255), Vector2.One, Main.rand.NextFloat(-5, 5), 0f, 0.8f, 20);
            GeneralParticleHandler.SpawnParticle(explosion2);

            float sparkCount = 16;
            for (int i = 0; i < sparkCount; i++)
            {
                Vector2 sparkVelocity2 = new Vector2(Main.rand.NextFloat(10, 20), 0).RotateRandom(1f).RotatedBy(Projectile.velocity.ToRotation());
                int sparkLifetime2 = Main.rand.Next(26, 35);
                float sparkScale2 = Main.rand.NextFloat(2f, 3.6f);
                Color sparkColor2 = Color.Lerp(Color.SkyBlue, Color.LightSkyBlue, Main.rand.NextFloat(0, 1));
                LineParticle spark = new LineParticle(target.Center, sparkVelocity2, false, (int)(sparkLifetime2), sparkScale2, sparkColor2);
                GeneralParticleHandler.SpawnParticle(spark);

            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return CEUtils.LineThroughRect(Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * length, targetHitbox, 30, 24);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex1 = CEUtils.getExtraTex("hadlaser");
            Main.spriteBatch.UseBlendState(BlendState.Additive);
            Main.spriteBatch.Draw(tex1, Projectile.Center - Main.screenPosition, null, Color.Blue, Projectile.rotation, new Vector2(0, tex1.Height / 2), new Vector2(length, width * 1f), SpriteEffects.None, 0); ;
            Main.spriteBatch.Draw(tex1, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, new Vector2(0, tex1.Height / 2), new Vector2(length, width * 0.4f), SpriteEffects.None, 0); ;

            Main.spriteBatch.End();
            Main.spriteBatch.begin_();
            return false;
        }
    }

}