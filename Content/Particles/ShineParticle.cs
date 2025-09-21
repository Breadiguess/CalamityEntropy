﻿using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;

namespace CalamityEntropy.Content.Particles
{
    public class ShineParticle : EParticle
    {
        public override Texture2D Texture => CEUtils.getExtraTex("Glow2");
        public Entity FollowOwner;
        public Vector2 ownerLastPos = Vector2.Zero;
        public bool flag = false;
        public float orgScale = -1;
        public override void AI()
        {
            if (FollowOwner != null)
            {
                if (ownerLastPos != Vector2.Zero)
                {
                    Position += (FollowOwner.Center - ownerLastPos);
                }
                ownerLastPos = FollowOwner.Center;
            }
            base.AI();
            this.Opacity = (float)(Math.Cos(((float)Lifetime / TimeLeftMax) * MathHelper.Pi - MathHelper.PiOver2));
            if (flag)
            {
                if (Lifetime > 20)
                {
                    Opacity = 1 - (Lifetime - 20f) / (TimeLeftMax - 20f);
                }
                else
                {
                    Opacity = Lifetime / 20f;
                }
                if (orgScale < 0)
                    orgScale = Scale;
                Scale = orgScale * Opacity;
                Opacity = 1;
                if (Lifetime > 32)
                {
                    for(int i = 0; i < (Lifetime - 32) / 16; i++)
                        Main.dust[Dust.NewDust(Position, 0, 0, DustID.MagicMirror)].velocity = CEUtils.randomPointInCircle(Scale * 3);
                }
            }
        }
    }
}
