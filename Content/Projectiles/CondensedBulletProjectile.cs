﻿using CalamityEntropy.Utilities;
using CalamityMod.Particles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Projectiles
{
    public class CondensedBulletProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override string Texture => CEUtils.WhiteTexPath;
        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2000;
            Projectile.tileCollide = true;
            Projectile.MaxUpdates = 500;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            AIType = ProjectileID.Bullet;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = hitbox.Center.ToVector2().getRectCentered(26, 26);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        public override void AI()
        {
            Projectile.localAI[2]++;
            if (Projectile.localAI[2] > 13)
            {
                Projectile.velocity = Projectile.velocity.normalize() * 16;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Vector2 center = Projectile.Center - Projectile.velocity * 2;
                Vector2 sparkVelo = Projectile.velocity * 0.05f;
                int sparkLifetime = 8;
                float sparkScale = Main.rand.NextFloat(1f, 1.8f);
                Color sparkColor = Color.Lerp(new Color(234, 130, 138), new Color(154, 152, 184), Main.rand.NextFloat(0, 1));
                Color org = new Color(sparkColor.R, sparkColor.G, sparkColor.B);
                sparkColor *= Main.rand.NextFloat(0.6f, 1);
                sparkColor.A = 255;
                if (Main.rand.NextBool(4))
                {
                    sparkColor.R = org.R;
                }
                if (Main.rand.NextBool(4))
                {
                    sparkColor.B = org.B;
                }
                var spark = new AltSparkParticle(center, sparkVelo, false, sparkLifetime, sparkScale, sparkColor);
                GeneralParticleHandler.SpawnParticle(spark);
                var spark2 = new LineParticle(center, sparkVelo, false, (int)(sparkLifetime), sparkScale * 0.8f, sparkColor);
                GeneralParticleHandler.SpawnParticle(spark2);
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            SpawnHitParticle(oldVelocity);
            return true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            SpawnHitParticle(Projectile.velocity);
        }
        public void SpawnHitParticle(Vector2 vel)
        {
            for (int i = 0; i < 12; i++)
            {
                Vector2 center = Projectile.Center;
                Vector2 sparkVelo = vel.RotatedBy(MathHelper.Pi).RotatedByRandom(0.5f) * Main.rand.NextFloat(0.3f, 0.6f);
                int sparkLifetime = 10;
                float sparkScale = Main.rand.NextFloat(0.4f, 1f);
                Color sparkColor = Color.Lerp(new Color(234, 130, 138), new Color(154, 152, 184), Main.rand.NextFloat(0, 1));
                Color org = new Color(sparkColor.R, sparkColor.G, sparkColor.B);
                sparkColor *= Main.rand.NextFloat(0.6f, 1);
                sparkColor.A = 255;
                if (Main.rand.NextBool(4))
                {
                    sparkColor.R = org.R;
                }
                if (Main.rand.NextBool(4))
                {
                    sparkColor.B = org.B;
                }
                var spark = new AltSparkParticle(center, sparkVelo, false, sparkLifetime, sparkScale, sparkColor);
                GeneralParticleHandler.SpawnParticle(spark);
                var spark2 = new LineParticle(center + sparkVelo * 2, sparkVelo, false, (int)(sparkLifetime), sparkScale * 0.8f, sparkColor);
                GeneralParticleHandler.SpawnParticle(spark2);
            }
        }
    }
}