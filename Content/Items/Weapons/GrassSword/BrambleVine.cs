﻿using CalamityEntropy.Content.Particles;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Particles;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Weapons.GrassSword
{
    public class BrambleVine : ModProjectile
    {
        public override string Texture => "CalamityEntropy/Content/Items/Weapons/GrassSword/Vine";
        public override void SetDefaults()
        {
            Projectile.FriendlySetDefaults(DamageClass.Melee, false, -1);
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 6;
        }
        public float Length = 0;
        public float progress = 0;
        public int HookNPC = -1;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(HookNPC);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            HookNPC = reader.ReadInt32();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Projectile.ai[0] == 2 && HookNPC >= 0)
            {
                Projectile.GetOwner().Entropy().BrambleBarAdd = 3;
            }
            if ((Projectile.ai[0] == 1 || Projectile.ai[0] == 2) && HookNPC == -1)
            {
                if (Projectile.ai[0] == 1)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.GetOwner().Center, Vector2.Zero, ModContent.ProjectileType<VineHookHit>(), Projectile.damage * 4, 0, Projectile.owner);
                }
                HookNPC = target.whoAmI;

            }

            CEUtils.PlaySound("GrassSwordHit" + Main.rand.Next(4).ToString(), 1.4f, target.Center, 16, HookNPC >= 0 ? 0.3f : 1);
            
            float sparkCount = 16;
            for (int i = 0; i < sparkCount; i++)
            {
                Vector2 sparkVelocity2 = CEUtils.randomRot().ToRotationVector2() * Main.rand.NextFloat(4, 12);
                int sparkLifetime2 = 12;
                float sparkScale2 = 0.5f;
                sparkScale2 *= (1 + Bramblecleave.GetLevel() * 0.05f);
                Color sparkColor2 = Color.Lerp(Color.Green, Color.LightGreen, Main.rand.NextFloat());

                AltSparkParticle spark = new AltSparkParticle(target.Center + Main.rand.NextVector2Circular(target.width * 0.5f, target.height * 0.5f), sparkVelocity2 * (1f), false, (int)(sparkLifetime2 * (1.2f)), sparkScale2 * (1.4f), sparkColor2);
                GeneralParticleHandler.SpawnParticle(spark);
            }

        }
        public float MaxLength = -1;
        public override void AI()
        {
            if (MaxLength == -1)
            {
                MaxLength = 400 + (Bramblecleave.GetLevel() * 60);
            }
            if (Projectile.localAI[0]++ == 0)
            {
                CEUtils.PlaySound("GrassSwordTrans", 1, Projectile.Center);
            }
            Player player = Projectile.GetOwner();
            float MaxTick = 36;
            progress = Projectile.ai[2]++ / MaxTick;
            Length = CEUtils.Parabola(progress, MaxLength);
            if (Projectile.ai[0] == 1 && HookNPC < 0)
            {
                if(Main.myPlayer == Projectile.owner)
                {
                    if(!Main.mouseRight)
                    {
                        HookNPC = -2;
                        Projectile.ai[0] = 0;
                    }
                }
            }
            
            if (HookNPC >= 0 && HookNPC.ToNPC().active && !HookNPC.ToNPC().dontTakeDamage)
            {
                if (Projectile.ai[0] == 1)
                {
                    if (Projectile.GetOwner().ownedProjectileCounts[ModContent.ProjectileType<VineHookHit>()] > 0)
                    {
                        Length = CEUtils.getDistance(HookNPC.ToNPC().Center, player.Center);
                        Projectile.rotation = (HookNPC.ToNPC().Center - Projectile.Center).ToRotation();
                        Projectile.localAI[1] += 6;
                        if (Projectile.localAI[1] > 60)
                        {
                            Projectile.localAI[1] = 60;
                        }
                        Projectile.ai[2] = MaxTick - 2;

                        player.velocity = Projectile.rotation.ToRotationVector2() * -8;
                        player.Center += new Vector2(Projectile.localAI[1] + 20, 0).RotatedBy(Projectile.rotation);
                        player.Entropy().immune = 25;
                    }
                    else
                    {
                        if (HookNPC >= 0)
                        {
                            progress = 1.1f;
                            player.velocity = Projectile.rotation.ToRotationVector2() * -16;
                            Projectile.ai[2] = MaxTick + 1;
                            Length = 0;
                        }
                        HookNPC = -2;
                        Projectile.netSpam = 0;
                    }
                }
                else if (Projectile.ai[0] == 2)
                {
                    if ((Main.myPlayer != Projectile.owner || Main.mouseLeft) && Projectile.localAI[1]++ <= 60 * 16 && CEUtils.getDistance(HookNPC.ToNPC().Center, Projectile.Center) < 400 + (Bramblecleave.GetLevel() * 60) * 1f)
                    {
                        if (Projectile.localAI[1] % 20 == 0)
                        {
                            player.Heal(Bramblecleave.GetLevel() / 2);
                        }
                        float lg = (HookNPC.ToNPC().Center - player.Center).Length();
                        Vector2 vj = (HookNPC.ToNPC().Center - player.Center).normalize() * lg * lg * 0.0000007f * ((400 + Bramblecleave.GetLevel() * 60) / (400 + 14f * 60));
                        player.velocity += vj;
                        HookNPC.ToNPC().velocity -= vj * HookNPC.ToNPC().knockBackResist;
                        Projectile.rotation = (HookNPC.ToNPC().Center - Projectile.Center).ToRotation();
                        Projectile.velocity = Projectile.rotation.ToRotationVector2() * Projectile.velocity.Length();
                        Projectile.ai[2] = MaxTick / 2;
                        Projectile.rotation = (HookNPC.ToNPC().Center - Projectile.Center).ToRotation();
                        Length = MaxLength = CEUtils.getDistance(Projectile.Center, HookNPC.ToNPC().Center);
                        Projectile.GetOwner().itemTime = Projectile.GetOwner().itemAnimation = 4;
                    }
                    else
                    {
                        if (HookNPC >= 0)
                        {
                            progress = 0.5f;
                            MaxLength = CEUtils.getDistance(Projectile.Center, HookNPC.ToNPC().Center) - 160;
                            Length = MaxLength;
                            NPC target = HookNPC.ToNPC();
                            CEUtils.PlaySound("GrassSwordHit" + Main.rand.Next(4).ToString(), 1.4f, target.Center, 16);
                            CEUtils.PlaySound("GrassSwordHit" + Main.rand.Next(4).ToString(), 1.4f, target.Center, 16);

                        }
                        HookNPC = -2;
                        Projectile.netSpam = 0;
                    }
                }
            }
            else
            {
                if (HookNPC >= 0)
                {
                    HookNPC = -2;
                }
            }
            int t = ModContent.ProjectileType<VineHookHit>();
            if(HookNPC < 0)
            {
                foreach(var proj in Main.ActiveProjectiles)
                {
                    if(proj.owner == Projectile.owner && proj.type == t)
                    {
                        proj.Kill();
                    }
                }
            }
            Projectile.Center = player.GetDrawCenter() + (Projectile.ai[0] == 2 ? Vector2.Zero : new Vector2(20, 6).RotatedBy(Projectile.velocity.ToRotation()) * (1 + 0.1f * Bramblecleave.GetLevel()));
            if(progress > 1)
            {
                Projectile.Kill();
                return;
            }
            player.itemTime = player.itemAnimation = 6;
            CEUtils.SetHandRot(player, Projectile.velocity.ToRotation());
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.timeLeft = 5;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return CEUtils.LineThroughRect(Projectile.Center, Projectile.Center + Projectile.velocity.normalize() * (Length + 46 + (Projectile.ai[0] == 2 ? 160 : 0)), targetHitbox, 46 + (Projectile.ai[0] == 0 ? 40 : 0));
        }
        public override bool PreDraw(ref Color lightColor)
        {
            float dScale = 1 + (Bramblecleave.GetLevel() * 0.02f);
            Player player = Projectile.GetOwner();
            int Count = (int)Math.Ceiling(((400 + Bramblecleave.GetLevel() * 60) / (dScale * 68f)));
            
            Texture2D v1 = Projectile.GetTexture();
            Texture2D v2 = this.getTextureAlt();
            float pr = CEUtils.Parabola(progress, 1);
            float l = Length;
            if (HookNPC >= 0)
            {
                pr = 1;
                l -= 160;
            }
            Vector2 e = Vector2.Zero;
            for (int i = 0; i <= Count; i++)
            {
                Vector2 pos = Vector2.Lerp(Projectile.Center, Projectile.Center + Projectile.velocity.normalize() * l, (float)i / Count);
                Texture2D draw = i == Count ? v2 : v1;
                Main.EntitySpriteDraw(draw, pos - Main.screenPosition, null, Lighting.GetColor((int)(pos.X / 16), (int)(pos.Y / 16)), Projectile.velocity.ToRotation(), new Vector2(0, draw.Height / 2), new Vector2(pr * dScale * 2, (float)Math.Sqrt(float.Min(1 / pr, 2)) * dScale), Count % 2 == 0 ? SpriteEffects.None : SpriteEffects.FlipVertically);
                e = pos;
            }
            if (Projectile.ai[0] == 2)
            {
                Texture2D sword = CEUtils.RequestTex("CalamityEntropy/Content/Items/Weapons/GrassSword/Bramblecleave");
                Main.EntitySpriteDraw(sword, e - Main.screenPosition + Projectile.rotation.ToRotationVector2() * 64, null, Color.Lerp(lightColor, Color.White, 0.6f), Projectile.rotation + MathHelper.PiOver4, sword.Size() / 2f, 1.6f + 0.1f * Bramblecleave.GetLevel(), SpriteEffects.None);
            }
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SourceDamage *= Projectile.ai[0] == 0 ? 0.4f : 0.26f;
            if(HookNPC >= 0)
            {
                modifiers.SourceDamage *= 0.5f;
            }
        }
        
        
    }
    public class VineHookHit : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.FriendlySetDefaults(DamageClass.Melee, false, -1);
            Projectile.width = Projectile.height = 140;
        }
        public override string Texture => CEUtils.WhiteTexPath;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void AI()
        {
            Projectile.Center = Projectile.GetOwner().MountedCenter;
            Projectile.timeLeft = 30;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            CEUtils.PlaySound("GrassSwordHitMetal", 1.4f, target.Center, volume: 1f);
            if (target.Organic())
            {
            }
            else
            {
                CEUtils.PlaySound("metalhit", 1.4f, target.Center, 6);
            }
            CEUtils.PlaySound("GrassSwordHit" + Main.rand.Next(4).ToString(), 1.4f, target.Center, 16);
            CEUtils.PlaySound("GrassSwordHit" + Main.rand.Next(4).ToString(), 1.4f, target.Center, 16);
            Color impactColor = Color.LightGreen;
            float impactParticleScale = Main.rand.NextFloat(2f, 3f);

            SparkleParticle impactParticle = new SparkleParticle(Projectile.GetOwner().Center, Vector2.Zero, impactColor, Color.LawnGreen, impactParticleScale, 12, 0, 5f);
            GeneralParticleHandler.SpawnParticle(impactParticle);
            Projectile.GetOwner().velocity = Projectile.GetOwner().velocity.normalize() * -20;
            Projectile.GetOwner().itemTime = Projectile.GetOwner().itemAnimation = 30;

            float sparkCount = 64 + Bramblecleave.GetLevel() / 2;
            for (int i = 0; i < sparkCount; i++)
            {
                float p = Main.rand.NextFloat();
                Vector2 sparkVelocity2 = (-Projectile.GetOwner().velocity).normalize().RotatedByRandom(0.4f) * Main.rand.NextFloat(36);
                int sparkLifetime2 = (int)((2 - p) * 16);
                float sparkScale2 = 0.6f + (1 - p);
                sparkScale2 *= (1 + Bramblecleave.GetLevel() * 0.06f);
                Color sparkColor2 = Color.Lerp(Color.Green, Color.LightGreen, p);
                if (Main.rand.NextBool())
                {
                    AltSparkParticle spark = new AltSparkParticle(target.Center + Main.rand.NextVector2Circular(target.width * 0.5f, target.height * 0.5f), sparkVelocity2 * (1f), false, (int)(sparkLifetime2 * (1.2f)), sparkScale2 * (1.4f), sparkColor2);
                    GeneralParticleHandler.SpawnParticle(spark);
                }
                else
                {
                    LineParticle spark = new LineParticle(target.Center + Main.rand.NextVector2Circular(target.width * 0.5f, target.height * 0.5f), sparkVelocity2, false, (int)(sparkLifetime2), sparkScale2 * (Projectile.frame == 7 ? 1.4f : 1f), Main.rand.NextBool() ? Color.LightGreen : Color.LimeGreen);
                    GeneralParticleHandler.SpawnParticle(spark);
                }
            }
            Projectile.Kill();
        }
    }
}