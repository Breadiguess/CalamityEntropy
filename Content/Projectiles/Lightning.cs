﻿using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Projectiles
{
    public class Lightning : ModProjectile
    {
        public SoundStyle sound = new SoundStyle("CalamityEntropy/Assets/Sounds/spark");
        public bool drawEnd = false;
        public Vector2 endPos;
        List<Vector2> points = new List<Vector2>();
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;

        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.light = 0f;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.ArmorPenetration = 128;

        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
        {
            if (Projectile.ai[0] == 0)
            {
                Random r = new Random();
                sound.Pitch = (float)r.Next(80, 110) / 100;
                sound.Volume = 2f;
                SoundEngine.PlaySound(sound, Projectile.Center);
                Vector2 vc;
                vc = Projectile.Center;

                Vector2 avc = Projectile.velocity;
                avc.Normalize();
                List<NPC> hited = new List<NPC>();
                List<NPC> close = new List<NPC>();
                foreach (NPC n in Main.npc)
                {
                    if (n.active && !n.friendly && !n.dontTakeDamage && Util.Util.getDistance(n.Center, Projectile.Center) < 800)
                    {
                        close.Add(n);
                    }
                }
                for (int i = 0; i < 16; i++)
                {
                    NPC target = null;
                    float targetDist = 1000;
                    foreach (NPC n in close)
                    {
                        if (Util.Util.getDistance(n.Center, vc) < targetDist && !hited.Contains(n))
                        {
                            target = n;
                            targetDist = Util.Util.getDistance(target.Center, vc);
                        }
                    }
                    if (target != null)
                    {
                        if (new Rectangle((int)vc.X - 7, (int)vc.Y - 7, 14, 14).Intersects(target.Hitbox))
                        {
                            hited.Add(target);
                        }
                        else
                        {
                            avc = new Vector2(avc.Length(), 0).RotatedBy((target.Center - vc).ToRotation());
                        }
                    }
                    Vector2 pv = vc + new Vector2(r.Next(-12, 13), r.Next(-12, 13));
                    if (i == 0)
                    {
                        pv = vc;
                    }
                    Tile tile = Main.tile[(int)pv.X / 16, (int)pv.Y / 16];
                    points.Add(pv);

                    vc += avc * Projectile.velocity.Length();
                }
            }
            Projectile.ai[0] += 1;
            if (Projectile.ai[0] > 36)
            {
                Projectile.Kill();
            }




        }
        public override string Texture => "CalamityEntropy/Assets/Extra/white";
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.ai[0] > 8)
            {
                return false;
            }
            if (points.Count < 1)
            {
                return false;
            }
            for (int i = 1; i < points.Count; i++)
            {
                if (Util.Util.LineThroughRect(points[i - 1], points[i], targetHitbox, 4))
                {
                    return true;
                }
            }
            return false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[2] == 1)
            {
                target.AddBuff(ModContent.BuffType<GalvanicCorrosion>(), 240);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            if (points.Count < 1)
            {
                return false;
            }
            Texture2D px = ModContent.Request<Texture2D>("CalamityEntropy/Assets/Extra/white").Value;
            Texture2D lm = ModContent.Request<Texture2D>("CalamityEntropy/Assets/Extra/lightmask").Value;
            float jd = 1;
            float lw = 0.7f * ((36f - Projectile.ai[0]) / 36f);
            Color color = Color.White;
            if (Projectile.ai[2] == 1)
            {
                color = Color.Red;
            }
            for (int i = 1; i < points.Count; i++)
            {
                Vector2 jv = points[i] - points[i - 1];
                jv.Normalize();
                jv *= 2;
                Util.Util.drawLine(Main.spriteBatch, px, points[i - 1], points[i] + jv, color * jd, 2f * lw);
                Util.Util.drawLine(Main.spriteBatch, px, points[i - 1], points[i] + jv, color * 0.8f * jd, 4f * lw);
                Util.Util.drawLine(Main.spriteBatch, px, points[i - 1], points[i] + jv, color * 0.6f * jd, 8f * lw);
                Util.Util.drawLine(Main.spriteBatch, px, points[i - 1], points[i] + jv, color * 0.4f * jd, 12f * lw);
                Util.Util.drawLine(Main.spriteBatch, px, points[i - 1], points[i] + jv, color * 0.2f * jd, 16f * lw);
                lw -= 0.7f * ((36f - Projectile.ai[0]) / 36f) / ((float)points.Count + 1);
            }
            if (drawEnd && Projectile.ai[0] < 12)
            {
                Util.Util.drawTexture(lm, endPos, 0, Color.White * ((12 - Projectile.ai[0]) / 12), new Vector2(1f, 1f));

            }
            return false;
        }
    }

}