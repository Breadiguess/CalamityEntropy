using CalamityEntropy.Content.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Projectiles.SamsaraCasket
{
    public class LunarExplode0 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }
        public override void SetDefaults()
        {
            Projectile.DamageType = NoneTypeDamageClass.Instance;
            Projectile.width = 90;
            Projectile.height = 90;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.extraUpdates = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 60;
        }
        int framecounter = 4;
        int frame = 0;
        public override void AI(){
            Projectile.ArmorPenetration = HorizonssKey.getArmorPen();
            if (frame == 0 && framecounter == 4) {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            }
            framecounter--;
            if (framecounter == 0)
            {
                frame++;
                framecounter = 4;
                if(frame > 6)
                {
                    Projectile.Kill();
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            lightColor = Color.White;
            Texture2D tex = ModContent.Request<Texture2D>("CalamityEntropy/Content/Projectiles/SamsaraCasket/LunarExplode" + frame.ToString()).Value;
            Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, tex.Size() / 2, Projectile.scale, SpriteEffects.None, 0);

            return false;
        }

    }

}