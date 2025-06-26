﻿using CalamityEntropy.Content.Particles;
using CalamityMod;
using CalamityMod.Particles;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Projectiles
{
    public class PlasmaBall : ModProjectile
    {
        public override string Texture => CEUtils.WhiteTexPath;
        public List<Vector2> oldPos = new List<Vector2>();
        public float range = 760;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.light = 1f;
            Projectile.timeLeft = 1024;
            Projectile.penetrate = 1;
            Projectile.MaxUpdates = 4;
        }
        public override void AI()
        {
            oldPos.Add(Projectile.Center);
            if(oldPos.Count > 16)
            {
                oldPos.RemoveAt(0);
            }
            if (Projectile.timeLeft % 46 == 0)
            {
                if (Main.myPlayer == Projectile.owner)
                {
                    int l = 8;
                    foreach (NPC npc in Main.ActiveNPCs)
                    {
                        if (npc.Distance(Projectile.Center) < range && !npc.friendly && !npc.dontTakeDamage)
                        {
                            l--;
                            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity * 4, ModContent.ProjectileType<PlasmaLightning>(), Projectile.damage / 3, 0, Projectile.owner, npc.Center.X, npc.Center.Y);
                            if(l <= 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                CalamityMod.Particles.Particle pulse = new DirectionalPulseRing(Projectile.Center, Vector2.Zero, new Color(255, 180, 180), new Vector2(2f, 2f), 0, 0.1f, 0.5f, 16);
                GeneralParticleHandler.SpawnParticle(pulse);
                CEUtils.SpawnExplotionFriendly(Projectile.GetSource_FromAI(), Projectile.GetOwner(), Projectile.Center, Projectile.damage, 72, Projectile.DamageType);
                CEUtils.SetShake(Projectile.Center, 4);
                CEUtils.PlaySound("energyImpact", Main.rand.NextFloat(0.7f, 1.3f), Projectile.Center);
            }
            int t = ModContent.ProjectileType<PlasmaLightning>();
            foreach(Projectile p in Main.ActiveProjectiles)
            {
                if(p.type == t && p.owner == Projectile.owner)
                {
                    p.Kill();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            float scale = 1 * Projectile.scale;
            DrawEnergyBall(Projectile.Center, scale, Projectile.Opacity);
            for(int i = 0; i < oldPos.Count; i++)
            {
                float c = (i + 1f) / oldPos.Count;
                DrawEnergyBall(oldPos[i], scale * c, Projectile.Opacity * c);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            return false;
        }
        public static void DrawEnergyBall(Vector2 pos, float size, float alpha)
        {
            Texture2D tex = CEUtils.getExtraTex("a_circle");
            Main.spriteBatch.UseBlendState(BlendState.Additive);
            Main.spriteBatch.Draw(tex, pos - Main.screenPosition, null, new Color(255, 230, 230) * alpha, 0, tex.Size() * 0.5f, size * 0.24f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, pos - Main.screenPosition, null, new Color(255, 40, 40) * alpha, 0, tex.Size() * 0.5f, size * 0.4f, SpriteEffects.None, 0);
            Main.spriteBatch.End();
            Main.spriteBatch.begin_();
        }
    }


}
