﻿using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Projectiles
{
    public class VoidseekerProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<RogueDamageClass>();
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.timeLeft = 120;
            Projectile.extraUpdates = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 55;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.scale = 2;
            Projectile.tileCollide = false;
        }
        float rotspeed = 0.12f;
        public bool playsound = true;
        float scale = 0;
        public override void AI()
        {
            bool stl = Projectile.Calamity().stealthStrike;
            if (Projectile.ai[0] == 0)
            {
                if (stl)
                {
                    rotspeed = 0.23f;
                    CEUtils.PlaySound("voidseeker", 1f, Projectile.Center, volume: 1f);
                    CEUtils.PlaySound("voidSound", 1f, Projectile.Center);
                }
                Projectile.rotation = MathHelper.PiOver2;
                if (stl)
                {
                    Projectile.rotation += MathHelper.Pi;
                }
            }
            Projectile.ai[0]++;
            rotspeed *= 0.94f;
            if (Projectile.ai[0] <= 26)
            {
                scale += (1 + (stl ? 1 : 0) - scale) * 0.06f;
            }
            if (Projectile.ai[0] == 26)
            {
                if (stl)
                {

                    if (Main.myPlayer == Projectile.owner)
                    {
                        Main.LocalPlayer.Entropy().screenPos = Projectile.Center;
                        Main.LocalPlayer.Entropy().screenShift = 1;
                        Main.LocalPlayer.Entropy().immune = 30;
                        Main.LocalPlayer.Calamity().GeneralScreenShakePower = 24;
                        Main.LocalPlayer.Center = Main.MouseWorld;

                    }
                }
                CEUtils.PlaySound("da3", 1, Projectile.Center);

            }
            if (Projectile.ai[0] > 26 && Projectile.ai[0] < 38 + (stl ? 5 : 0))
            {
                scale = 1 + (stl ? 1 : 0);
                rotspeed -= 0.09f;
                for (int i = 0; i < 12; i++)
                {
                    float rot = Projectile.rotation + rotspeed * ((float)i / 8f);
                    
                    Vector2 direction = rot.ToRotationVector2().RotatedBy((Projectile.velocity.X > 0 ? -MathHelper.PiOver2 : MathHelper.PiOver2));
                    Vector2 smokeSpeed = direction * Main.rand.NextFloat(10f, 18f);
                    /*
                    CalamityMod.Particles.Particle smoke = new HeavySmokeParticle(Projectile.Center + rot.ToRotationVector2() * Main.rand.NextFloat(120, 230) * scale, smokeSpeed, Color.Lerp(Color.Purple, Color.Indigo, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 6f)), 30, Main.rand.NextFloat(0.6f, 1.2f), 0.8f, 0, false, 0, true);
                    GeneralParticleHandler.SpawnParticle(smoke);
                    */
                    var smokeGlow = new HeavySmokeParticle(Projectile.Center + rot.ToRotationVector2() * Main.rand.NextFloat(145, 205) * scale, smokeSpeed, new Color(60, 60, 200), 30, Main.rand.NextFloat(1f, 1.4f), 0.8f, 0.008f, true, 0.01f, true);
                    GeneralParticleHandler.SpawnParticle(smokeGlow);
                    
                }
            }
            if (Projectile.ai[0] >= 37 + (stl ? 0 : 0))
            {

                rotspeed *= 0.57f;
            }
            if (Projectile.ai[0] > 46 + (stl ? 0 : 0))
            {
                scale *= 0.93f;
            }
            if (Projectile.ai[0] > 60 + (stl ? 0 : 0))
            {
                Projectile.Kill();
            }

            Projectile.rotation += rotspeed * (Projectile.velocity.X > 0 ? 1 : -1) * (stl ? -1 : 1);
            Projectile.Center = Projectile.owner.ToPlayer().gfxOffY * Vector2.UnitY + Projectile.owner.ToPlayer().Center;
            Player owner = Projectile.owner.ToPlayer();
            owner.heldProj = Projectile.whoAmI;
            if (Projectile.velocity.X > 0)
            {
                owner.direction = 1;
            }
            else
            {
                owner.direction = -1;
            }
            if (Projectile.velocity.X * (Projectile.Calamity().stealthStrike ? -1 : 1) > 0)
            {
                owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)(Math.PI * 0.75f));
            }
            else
            {
                owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + (float)(Math.PI * 1.75f));
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int i = 0; i < 3; i++)
            {
                Color impactColor = Main.rand.NextBool(3) ? Color.SkyBlue : Color.White;
                float impactParticleScale = Main.rand.NextFloat(1f, 1.75f);

                SparkleParticle impactParticle2 = new SparkleParticle(target.Center + Main.rand.NextVector2Circular(target.width * 0.75f, target.height * 0.75f), Vector2.Zero, Color.White, Color.Blue, impactParticleScale * 1.2f, 8, 0, 4.5f);
                GeneralParticleHandler.SpawnParticle(impactParticle2);

                SparkleParticle impactParticle = new SparkleParticle(target.Center + Main.rand.NextVector2Circular(target.width * 0.75f, target.height * 0.75f), Vector2.Zero, impactColor, Color.Blue, impactParticleScale, 8, 0, 2.5f);
                GeneralParticleHandler.SpawnParticle(impactParticle);
            }

            float sparkCount = MathHelper.Clamp(18 - Projectile.numHits * 3 + (Projectile.Calamity().stealthStrike ? 8 : 0), 0, 18);
            for (int i = 0; i < sparkCount; i++)
            {
                Vector2 sparkVelocity2 = (Projectile.rotation + (Projectile.velocity.X * (Projectile.Calamity().stealthStrike ? -1 : 1) > 0 ? -MathHelper.PiOver2 : MathHelper.PiOver2)).ToRotationVector2().RotatedByRandom(0.14f) * 20 * Main.rand.NextFloat(0.5f, 1.8f);
                int sparkLifetime2 = Main.rand.Next(23, 35);
                float sparkScale2 = Main.rand.NextFloat(0.95f, 1.8f);
                Color sparkColor2 = Main.rand.NextBool(3) ? Color.Blue : Color.AliceBlue;
                if (Main.rand.NextBool())
                {
                    AltSparkParticle spark = new AltSparkParticle(target.Center + Main.rand.NextVector2Circular(target.width * 0.5f, target.height * 0.5f), sparkVelocity2 * (1f), false, (int)(sparkLifetime2 * (1.2f)), sparkScale2 * (1.4f), sparkColor2);
                    GeneralParticleHandler.SpawnParticle(spark);
                }
                else
                {
                    LineParticle spark = new LineParticle(target.Center + Main.rand.NextVector2Circular(target.width * 0.5f, target.height * 0.5f), sparkVelocity2 * (Projectile.frame == 7 ? 1f : 0.65f), false, (int)(sparkLifetime2 * (Projectile.frame == 7 ? 1.2f : 1f)), sparkScale2 * (Projectile.frame == 7 ? 1.4f : 1f), Main.rand.NextBool() ? Color.Red : Color.Firebrick);
                    GeneralParticleHandler.SpawnParticle(spark);
                }
            }
            float dustCount = MathHelper.Clamp(25 - Projectile.numHits * 3, 0, 25);
            for (int i = 0; i <= dustCount; i++)
            {
                int dustID = DustID.MagicMirror;
                Dust dust2 = Dust.NewDustPerfect(target.Center + Main.rand.NextVector2Circular(target.width * 0.5f, target.height * 0.5f), dustID, (Projectile.rotation + (Projectile.velocity.X > 0 ? -MathHelper.PiOver2 : MathHelper.PiOver2)).ToRotationVector2() * Main.rand.NextFloat(0.3f, 1.1f));
                dust2.scale = Main.rand.NextFloat(0.9f, 2.4f);
                dust2.noGravity = true;
            }
            if (Projectile.Calamity().stealthStrike)
            {
                if (playsound)
                {
                    playsound = false;
                    CEUtils.PlaySound("voidseekercrit", 1, Projectile.Center, 4);
                    CEUtils.PlaySound("voidseekercrit", 1, Projectile.Center, 4);
                }
            }
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.ai[0] > 26)
            {
                return CEUtils.LineThroughRect(Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * 240 * scale, targetHitbox, 100);
            }
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        float balpha = 0;
        float bsize = 0.8f;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.velocity.X * (Projectile.Calamity().stealthStrike ? -1 : 1) > 0)
            {
                Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation - MathHelper.Pi / 2f, new Vector2(0, 0), scale * Projectile.scale, SpriteEffects.FlipVertically, 0);
            }
            else
            {
                Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation + MathHelper.Pi / 2f, new Vector2(0, tex.Height), scale * Projectile.scale, SpriteEffects.None, 0);

            }
            if (Projectile.Calamity().stealthStrike)
            {
                if (balpha < 1)
                {
                    balpha += 0.05f;
                }
                if (Projectile.ai[0] > 30)
                {
                    balpha -= 0.1f;
                }
                for (float i = 1; i <= 1.6f; i += 0.1f)
                {
                    Main.spriteBatch.Draw(CEUtils.getExtraTex("blackg"), Projectile.Center - Main.screenPosition, null, Color.Black * 0.06f * balpha, Main.GameUpdateCount * 0.9f * (i - 0.9f), new Vector2(1200, 1200), (1 + (i - 1) * 0.1f) * bsize * 6f, SpriteEffects.None, 0);
                }
            }
            return false;
        }
    }
}