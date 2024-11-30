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
    public class Recovery : SamsaraSword
    {
        public bool sticked = false;
        public int stickedNPC = -1;
        public Vector2 offset;
        public int counter = 0;
        public float frame = 0;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.localNPCHitCooldown = 8;
        }
        public override void AI()
        {
            Projectile.extraUpdates = 0;
            base.AI();
            
            if (sticked)
            {
                if(frame < 3)
                {
                    frame+=0.1f;
                }
            }
            else
            {
                if(frame > 0)
                {
                    frame-=0.1f;
                }
            }
        }
        public override void attackAI(NPC t)
        {
            setDamage(1f);
            Projectile.Resize(46, 46);
            if(sticked && stickedNPC != t.whoAmI)
            {
                sticked = false;
            }
            if (sticked)
            {
                Projectile.Center = t.Center + offset;
                Projectile.rotation = (t.Center - Projectile.Center).ToRotation();
                counter++;
                Projectile.velocity *= 0;
                if(Main.myPlayer == Projectile.owner)
                {
                    if(counter % 30 == 0)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), t.Center + new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101)), Vector2.Zero, ModContent.ProjectileType<FlowerSkeleton0>(), (int)(Projectile.damage * 0.4f), Projectile.knockBack, Projectile.owner);
                    }
                    if (counter % 60 == 0)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), t.Center, Util.Util.randomRot().ToRotationVector2() * 26, ModContent.ProjectileType<RecoveryVineTop>(), (int)(Projectile.damage * 0.7f), Projectile.knockBack, Projectile.owner);
                    }
                }
            }
            else
            {
                Projectile.extraUpdates = 6;
                Projectile.velocity = (t.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * (10);
                
                Projectile.rotation = (t.Center - Projectile.Center).ToRotation();
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if(target == this.target && !sticked)
            {
                stickedNPC = target.whoAmI;
                sticked = true;
                offset = Projectile.Center - target.Center;
                Projectile.netUpdate = true;
            }
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(sticked);
            writer.Write(stickedNPC);
            writer.WriteVector2(offset);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            sticked = reader.ReadBoolean();
            stickedNPC = reader.ReadInt32();
            offset = reader.ReadVector2();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (hideTime > 0)
            {
                return false;
            }
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            if ((int)frame > 0)
            {
                tex = ModContent.Request<Texture2D>("CalamityEntropy/Projectiles/SamsaraCasket/Recovery" + ((int)(frame)).ToString()).Value;
            }
            for (int i = 0; i < oldPos.Count; i++)
            {
                Main.spriteBatch.Draw(tex, oldPos[i] - Main.screenPosition, null, lightColor * ((float)i / (float)oldPos.Count) * 0.4f, oldRot[i] + MathHelper.PiOver4, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
            }
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver4, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }
    }
}
