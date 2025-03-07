using CalamityEntropy.Content.Dusts;
using CalamityEntropy.Content.Items.Books;
using CalamityEntropy.Util;
using CalamityMod.Buffs.StatBuffs;
using CalamityMod.Buffs.StatDebuffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Projectiles
{
    public class AstralBullet : EBookBaseProjectile
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Projectile.DamageType = DamageClass.Magic;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 60;
            Projectile.light = 1;
            
        }
        public override void AI(){
            base.AI();
            if (Projectile.timeLeft < 21)
            {
                if (Projectile.timeLeft > 5)
                {
                    if (Projectile.timeLeft % 5 == 0)
                    {
                        if (this.ShooterModProjectile is EntropyBookHeldProjectile mp)
                        {
                            NPC target = Projectile.FindTargetWithinRange(1400);
                            mp.ShootSingleProjectile(mp.getShootProjectileType(), Projectile.Center, (target == null ? Projectile.velocity : (target.Center - Projectile.Center)));
                        }
                    }
                }
            }
            Projectile.velocity *= 0.96f;
        }
        public override void ApplyHoming()
        {
            
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AstralExplosion>(), Projectile.damage * 3, 1, Projectile.owner);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Util.Util.getExtraTex("StarTexture");
            Main.spriteBatch.UseBlendState(BlendState.Additive);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.LightYellow, Main.GlobalTimeWrappedHourly, tex.Size() / 2, Projectile.scale * 0.6f, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, Color.LightGoldenrodYellow, -2 * Main.GlobalTimeWrappedHourly, tex.Size() / 2, Projectile.scale * 0.4f, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.begin_();
            return false;
        }
    }
    

}