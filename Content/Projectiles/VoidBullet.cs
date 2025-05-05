﻿using CalamityEntropy.Common;
using CalamityEntropy.Content.Particles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Projectiles
{

    public class VoidBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.DrawScreenCheckFluff[Projectile.type] = 5000;

        }
        public override void OnSpawn(IEntitySource source)
        {
            CalamityEntropy.CheckProjs.Add(Projectile);
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
            Projectile.scale = 1f;
            Projectile.timeLeft = 400;
            Projectile.extraUpdates = 1;
        }
        public float ap = 0;
        public override void AI()
        {
            Particle p = new Particle();
            p.alpha = 0.5f;
            p.position = Projectile.Center;
            VoidParticles.particles.Add(p);
            NPC target = Projectile.FindTargetWithinRange(900, false);
            Projectile.rotation = Projectile.velocity.ToRotation();
            if (target != null)
            {
                Projectile.velocity *= 0.9f;
                Vector2 v = target.Center - Projectile.Center;
                v.Normalize();

                Projectile.velocity += v * 2f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            EGlobalNPC.AddVoidTouch(target, 120, 4, 800, 16);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }

}