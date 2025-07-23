﻿using CalamityEntropy.Content.Particles;
using CalamityMod;
using CalamityMod.Items;
using CalamityMod.NPCs.AstrumDeus;
using CalamityMod.Particles;
using CalamityMod.Rarities;
using InnoVault.PRT;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Donator
{
    public class TheFilthyContractWithMammon : ModItem, IDonatorItem
    {
        public string DonatorName => "Reficul";

        public override bool MagicPrefix()
        {
            return true;
        }
        public override void SetDefaults()
        {
            Item.width = 52;
            Item.height = 52;
            Item.damage = 300;
            Item.DamageType = DamageClass.Magic;
            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.channel = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 2f;
            Item.value = CalamityGlobalItem.RarityCalamityRedBuyPrice;
            Item.rare = ModContent.RarityType<CalamityRed>();
            Item.shootSpeed = 16f;
            Item.crit = 8;
            Item.shoot = ModContent.ProjectileType<FilthyCircle>();
        }
        public bool Active = false;
        public int UsingTime = 0;
        public override void UpdateInventory(Player player)
        {
            if(!Active && Item.favorited)
            {
                Active = true;
                player.Hurt(PlayerDeathReason.ByPlayerItem(player.whoAmI, Item), 20, 0);
                Item.favorited = false;
            }
            if (Active)
            {
                TextureAssets.Item[Type] = CEUtils.getExtraTexAsset("TheFilthyContractWithMammonActive");
                if (player.Entropy().UsingItemCounter > 0 && player.HeldItem == Item)
                {
                    UsingTime++;
                }
                if(UsingTime > 0 && player.Entropy().UsingItemCounter > 0 && player.HeldItem != Item)
                {
                    Active = false;
                }

            }
            else
            {
                TextureAssets.Item[Type] = CEUtils.getExtraTexAsset("TheFilthyContractWithMammon");
                if(UsingTime > 0)
                {
                    int dmg = UsingTime / 6;
                    if(dmg < 6)
                    {
                        dmg = 6;
                    }
                    if(dmg > 666)
                    {
                        dmg = 666;
                    }
                    player.Hurt(PlayerDeathReason.ByPlayerItem(player.whoAmI, Item), dmg, 0);
                    UsingTime = 0;
                }
            }
        }
        public override bool CanUseItem(Player player)
        {
            return Active;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }

    public class FilthyCircle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.FriendlySetDefaults(DamageClass.Magic, false, -1);
            Projectile.width = Projectile.height = 64;
            
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public bool r = true;
        public override void AI()
        {
            if (Projectile.localAI[0]++ == 0)
            {
                Projectile.rotation = Projectile.velocity.ToRotation();
            }
            Player player = Projectile.GetOwner();
            if(Main.myPlayer == Projectile.owner && Main.mouseRight && r)
            {
                player.channel = true;
            }
            else
            {
                r = false;
            }
            if (player.channel)
            {
                if (alpha < 1)
                    alpha += 0.05f;
                if (alpha >= 1)
                {
                    if (Main.myPlayer == Projectile.owner)
                    {
                        if (player.altFunctionUse == 2)
                        {
                            if (Projectile.ai[1]++ % 5 == 0)
                            {
                                SoundEngine.PlaySound(AstrumDeusHead.LaserSound, Projectile.Center);
                                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Projectile.velocity * 2 + Projectile.velocity.RotatedBy(MathHelper.PiOver2).normalize() * Main.rand.NextFloat(-64, 64) + Projectile.velocity * 2, Projectile.velocity * 3, ModContent.ProjectileType<FilthyShootAlt>(), Projectile.damage * 4, Projectile.knockBack, Projectile.owner);
                            }
                        }
                        else
                        {
                            if (Projectile.ai[1]++ % 2 == 0)
                            {
                                Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity * 2, ModContent.ProjectileType<FilthyProjectile>(), Projectile.damage, Projectile.knockBack, Projectile.owner); ;
                            }
                        }
                    }
                }
            }
            else
            {
                if (alpha > 0)
                {
                    alpha -= 0.1f;
                }
                else
                {
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Projectile.Kill();
                    }
                }
            }
            
            player.Calamity().mouseWorldListener = true;
            player.itemAnimation = player.itemTime = 4;
            Projectile.Center = player.GetDrawCenter();
            Projectile.rotation = CEUtils.RotateTowardsAngle(Projectile.rotation, (player.Calamity().mouseWorld - Projectile.Center).ToRotation(), 0.12f, false);
            CEUtils.SetHandRot(player, Projectile.rotation);
            Projectile.velocity = Projectile.rotation.ToRotationVector2() * 16;
        }
        public float alpha;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D circle = Projectile.GetTexture();
            float angle = Main.GameUpdateCount * 0.18f;
            float size = (int)(alpha * 160);
            Vector2 lu = new Vector2(size, 0).RotatedBy(angle - MathHelper.ToRadians(angle - 135));
            Vector2 ru = new Vector2(size, 0).RotatedBy(angle - MathHelper.ToRadians(angle - 45));
            Vector2 ld = new Vector2(size, 0).RotatedBy(angle - MathHelper.ToRadians(angle + 135));
            Vector2 rd = new Vector2(size, 0).RotatedBy(angle - MathHelper.ToRadians(angle + 45));

            float c = 0.36f;
            lu.X *= c;
            ru.X *= c;
            ld.X *= c;
            rd.X *= c;

            Vector2 dp = Projectile.Center - Main.screenPosition;
            Vector2 offset = new Vector2(70, 0);

            lu += offset;
            ru += offset;
            ld += offset;
            rd += offset;
            float rot = Projectile.rotation;
            lu = lu.RotatedBy(rot);
            ru = ru.RotatedBy(rot);
            ld = ld.RotatedBy(rot);
            rd = rd.RotatedBy(rot);

            CEUtils.drawTextureToPoint(Main.spriteBatch, circle, Color.White * alpha, dp + lu, dp + ru, dp + ld, dp + rd);
            return false;
        }
    }

    public class FilthyProjectile : ModProjectile
    {
        public StarTrailParticle trail;
        public override string Texture => CEUtils.WhiteTexPath;
        public override void SetDefaults()
        {
            Projectile.FriendlySetDefaults(DamageClass.Magic, false, -1);
            Projectile.width = Projectile.height = 64;
            Projectile.timeLeft = 120;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public Vector2 tOfs;
        public override void AI()
        {
            if(trail == null)
            {
                tOfs = CEUtils.randomPointInCircle(36);
                trail = new StarTrailParticle() { maxLength = 24 };
                EParticle.spawnNew(trail, Projectile.Center + tOfs, Projectile.velocity, Color.DarkRed, 1, 1, true, BlendState.NonPremultiplied, Projectile.velocity.ToRotation(), 30);
            }
            trail.Lifetime = 30;
            trail.Position = Projectile.Center + tOfs;
            trail.AddPoint(Projectile.Center + tOfs);
            Vector2 ver = Projectile.velocity;
            BasePRT particle = new PRT_Light(Projectile.Center + CEUtils.randomPointInCircle(32 * Projectile.scale), ver
                , Main.rand.NextFloat(2f, 4f), Color.Red, 140, 1f);
            PRTLoader.AddParticle(particle);
            if (Main.rand.NextBool())
            {
                LineParticle spark = new LineParticle(Projectile.Center + Projectile.velocity * 4 + CEUtils.randomPointInCircle(32), Projectile.velocity, false, 32, Main.rand.NextFloat(1, 2), Color.DarkRed);
                GeneralParticleHandler.SpawnParticle(spark);
            }
            else
            {
                var spark2 = new SparkParticle(Projectile.Center + CEUtils.randomPointInCircle(32), Projectile.velocity, true, 50, Main.rand.NextFloat(0.6f, 1.2f), Color.Red);
                GeneralParticleHandler.SpawnParticle(spark2);
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for(int i = 0; i < 2; i++)
            {
                float r = CEUtils.randomRot();
                LineParticle spark = new LineParticle(Projectile.Center + r.ToRotationVector2() * -160, r.ToRotationVector2() * 32, false, 32, Main.rand.NextFloat(1, 2), Color.Red);
                GeneralParticleHandler.SpawnParticle(spark);
            }
            CEUtils.PlaySound("nvspark", 1, target.Center, 10);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
    }
    public class FilthyShootAlt : ModProjectile
    {
        public override string Texture => CEUtils.WhiteTexPath;
        public override void SetDefaults()
        {
            Projectile.FriendlySetDefaults(DamageClass.Magic, false, 1);
            Projectile.width = Projectile.height = 32;
            Projectile.timeLeft = 140;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void AI()
        {
            for (int i = 0; i < 10; i++)
            {
                EParticle.NewParticle(new Smoke() { timeleftmax = 26, Lifetime = 26 }, Projectile.Center + Projectile.velocity * (i / 10f) + CEUtils.randomPointInCircle(6), CEUtils.randomPointInCircle(0.5f), Color.Red, Main.rand.NextFloat(0.06f, 0.09f), 0.5f, true, BlendState.Additive, CEUtils.randomRot());
            }
            Lighting.AddLight(Projectile.Center, 0.25f, 0f, 0f);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Color impactColor = Color.Red;
            float impactParticleScale = 3;
            SparkleParticle impactParticle = new SparkleParticle(Projectile.Center, Vector2.Zero, impactColor, Color.OrangeRed, impactParticleScale, 14, 0f, 3f);
            GeneralParticleHandler.SpawnParticle(impactParticle);
            CEUtils.PlaySound("sf_hit1", 1, Projectile.Center);
        }
    }
}
