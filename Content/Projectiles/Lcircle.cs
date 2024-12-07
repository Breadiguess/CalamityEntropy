using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Projectiles
{
    public class Lcircle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Generic;
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.light = 1f;
            Projectile.timeLeft = 30;
            Projectile.penetrate = -1;
        }

        public override void AI(){

        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return false;
        }

        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D tx = ModContent.Request<Texture2D>("CalamityEntropy/Content/Projectiles/Lcircle").Value;
            Main.spriteBatch.Draw(tx, Projectile.Center - Main.screenPosition, null, Color.White * ((float)Projectile.timeLeft / 20f), Projectile.rotation, new Vector2(tx.Width, tx.Height) / 2, 1 + ((float)(60 - Projectile.timeLeft)) / 120f, SpriteEffects.None, 0);

            return false;
        }
    }
    

}