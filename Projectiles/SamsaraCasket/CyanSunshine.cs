﻿using CalamityEntropy.Items;
using CalamityEntropy.Util;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Events;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Biomes;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Projectiles.SamsaraCasket
{
    public class CyanSunshine : SamsaraSword
    {
        public float xscale = 1;
        public float light2;
        public float light = 0;
        public int charge = 60;
        public List<Vector2> lightningPoints = new List<Vector2>();
        public float lightningWidth = 0;
        public override void AI()
        {
            if (lightningWidth > 0)
            {
                lightningWidth -= 0.1f;
            }
            if (light > 0)
            {
                light -= 0.1f;
            }
            if (xscale < 1)
            {
                xscale += 0.1f;
            }
            if (light2 > 0)
            {
                light2 -= 1f / 60f;
            }
            base.AI();

        }
        
        public override void attackAI(NPC t)
        {
            if(charge == 50)
            {
                SoundEngine.PlaySound(new SoundStyle("CalamityEntropy/Sounds/light_bolt_delayed"), Projectile.Center);
            }
            if(charge > 0)
            {
                charge--;
                if(charge == 0)
                {

                    SoundEngine.PlaySound(new SoundStyle("CalamityEntropy/Sounds/light_bolt"), Projectile.Center);
                }
                Vector2 targetpos = t.Center + new Vector2(-160 - (t.width + t.height) / 4, -160 - (t.width + t.height) / 4);
                Projectile.velocity *= 0.92f;
                Projectile.velocity += (targetpos - Projectile.Center).SafeNormalize(Vector2.Zero) * 2f;
                light2 += 2f / 60f;
            }
            else
            {
                if (Util.Util.getDistance(t.Center, Projectile.Center) < Projectile.velocity.Length() * 1.16f)
                {
                    Projectile.Center = t.Center;
                    Projectile.velocity *= 0f;
                }
                else
                {
                    light2 = 1;
                    Projectile.velocity = (t.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (42 + t.velocity.Length());
                }
            }
            setDamage(3);
            Projectile.rotation = (t.Center - Projectile.Center).ToRotation();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (charge <= 0)
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
                setDamage(1);
                if(Main.myPlayer == Projectile.owner)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Util.Util.randomRot().ToRotationVector2() * Main.rand.Next(6, 10) - new Vector2(0, 8), ModContent.ProjectileType<CyanFeather>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
                charge = 60;
                NPC t = target;
                Projectile.Center = t.Center + new Vector2(-160 - (t.width + t.height) / 4, -160 - (t.width + t.height) / 4);
                light2 = 0;
                light = 1;
                xscale = 0;
                lightningWidth = 1;
                lightningPoints = Util.LightningGenerator.GenerateLightning(target.Center, target.Center + new Vector2(Main.rand.Next(-36, 37), Main.rand.Next(-160, -100) - t.height / 2), 9, 7);
                Projectile.velocity *= 0;
                
            }
        }
        public override bool PreDraw(ref Color lightColor)
        {

            if (hideTime > 0)
            {
                return false;
            }
            
            for(int i = 1; i < lightningPoints.Count; i++)
            {
                Util.Util.drawLine(lightningPoints[i - 1], lightningPoints[i], Color.White, (float)(Math.Cos(-MathHelper.PiOver2 + ((float)i / (float)lightningPoints.Count) * MathHelper.Pi) * lightningWidth * 6), 2);
            }
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            if(charge <= 0)
            {
                for (int i = 0; i < oldPos.Count; i++)
                {
                    Main.spriteBatch.Draw(tex, oldPos[i] - Main.screenPosition, null, lightColor * ((float)i / (float)oldPos.Count) * 0.4f, oldRot[i] + MathHelper.PiOver4, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                }
            }
            tex = TextureAssets.Projectile[Projectile.type].Value;
            if(charge <= 0 || target == null)
            {
                for (int i = 0; i < oldPos.Count; i++)
                {
                    Main.spriteBatch.Draw(tex, oldPos[i] - Main.screenPosition, null, lightColor * ((float)i / (float)oldPos.Count) * 0.4f, oldRot[i] + MathHelper.PiOver4, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
                }
            }
            SpriteBatch sb = Main.spriteBatch;
            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            tex = ModContent.Request<Texture2D>("CalamityEntropy/Projectiles/SamsaraCasket/csglow1").Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, new Color(153, 200, 193) * light2, Projectile.rotation + MathHelper.PiOver4, tex.Size() / 2, Projectile.scale * new Vector2(1, xscale), SpriteEffects.None, 0);

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            tex = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.Lerp(lightColor, Color.White, light2), Projectile.rotation + MathHelper.PiOver4, tex.Size() / 2, Projectile.scale * new Vector2(1, xscale), SpriteEffects.None, 0);

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            tex = ModContent.Request<Texture2D>("CalamityEntropy/Projectiles/SamsaraCasket/csglow").Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.White * light, Projectile.rotation + MathHelper.PiOver4, tex.Size() / 2, Projectile.scale * new Vector2(1, xscale), SpriteEffects.None, 0);

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

    }
}
