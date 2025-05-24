﻿using CalamityEntropy.Content.Buffs;
using CalamityEntropy.Content.Projectiles;
using CalamityEntropy.Utilities;
using CalamityMod;
using CalamityMod.Buffs.DamageOverTime;
using CalamityMod.Dusts;
using CalamityMod.Items;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Projectiles.Melee;
using CalamityMod.World;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Weapons.Fractal
{
    public class SpiritFractal : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 440;
            Item.crit = 5;
            Item.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Item.width = 48;
            Item.height = 60;
            Item.useTime = Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.knockBack = 2;
            Item.value = CalamityGlobalItem.RarityPinkBuyPrice;
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = null;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<SpiritFractalHeld>();
            Item.shootSpeed = 12f;
        }
        public int atkType = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            int at = 2;
            if(atkType == 0 || atkType == 2)
            {
                at = -1;
            }
            if(atkType == 1 || atkType == 3)
            {
                at = 1;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, at, 0, Main.MouseWorld.Distance(position) + 180);
            atkType += 1;
            if(atkType > 4)
            {
                atkType = 0;
            }
            return false;
        }


        public override bool MeleePrefix()
        {
            return true;
        }
        public override void AddRecipes()
        {
            CreateRecipe().AddIngredient<ElementalFractal>()
                .AddIngredient<VoidEdge>()
                .AddIngredient<TerrorBlade>()
                .AddIngredient<RuinousSoul>(4)
                .AddTile(TileID.LunarCraftingStation).Register();
        }
    }
    public class SpiritFractalHeld : ModProjectile
    {
        public override string Texture => "CalamityEntropy/Content/Items/Weapons/Fractal/SpiritFractal";
        List<float> odr = new List<float>();
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 12;

        }
        public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<TrueMeleeDamageClass>();
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
            Projectile.timeLeft = 100000;
            Projectile.extraUpdates = 3;
        }
        public float counter = 0;
        public float scale = 1;
        public float alpha = 0;
        public bool init = true;
        public bool shoot = true;
        public float spawnProjCounter = 0;
        public override void AI()
        {
            Player owner = Projectile.getOwner();
            float MaxUpdateTimes = owner.itemTimeMax * Projectile.MaxUpdates;
            float progress = (counter / MaxUpdateTimes);
            counter += 1 * (Projectile.ai[0] == 2 ? 0.5f : 1);
            if (init)
            {
                if (Projectile.ai[0] == 2)
                {
                    Util.PlaySound("sf_use", 0.6f, Projectile.Center, volume: 0.8f);
                    Projectile.scale *= 1.3f;
                }
                if (Projectile.ai[0] < 2)
                {
                    Util.PlaySound("sf_use", 1 + Projectile.ai[0] * 0.12f, Projectile.Center, volume: 0.6f);
                }
                init = false;
            }
            odr.Add(Projectile.rotation);
            Projectile.timeLeft = 3;

            if (Projectile.ai[0] == 2)
            {
                for (int i = 0; i < Projectile.localNPCImmunity.Length; i++)
                {
                    if (Projectile.localNPCImmunity[i] == -1)
                        Projectile.localNPCImmunity[i] = (int)(Projectile.MaxUpdates * 4 / owner.GetTotalAttackSpeed(Projectile.DamageType));
                }
                Projectile.localNPCHitCooldown = (int)(Projectile.MaxUpdates * 4 / owner.GetTotalAttackSpeed(Projectile.DamageType));

                float RotF = MathHelper.ToRadians(280) + MathHelper.TwoPi * 3;
                if (progress > 0.3f && progress < 0.7f)
                    spawnProjCounter += owner.GetTotalAttackSpeed(Projectile.DamageType);
                if (spawnProjCounter >= 6f)
                {
                    spawnProjCounter -= 6f;
                    Vector2 spawnPos = Projectile.Center + Projectile.rotation.ToRotationVector2() * 48 * scale * Projectile.scale;
                    if (Main.myPlayer == Projectile.owner)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromAI(), spawnPos, Util.randomPointInCircle(0.1f) + Projectile.rotation.ToRotationVector2() * 8, ModContent.ProjectileType<GhastlySoulLarge>(), Projectile.damage / 7, Projectile.knockBack, Projectile.owner, 0, 1);
                    }
                }
                alpha = 1;
                scale = 1.8f;
                Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.ToRadians(-140) + RotF * Util.GetRepeatedCosFromZeroToOne(progress, 1)) * (Projectile.velocity.X > 0 ? 1 : -1);
                Projectile.Center = Projectile.getOwner().MountedCenter + Projectile.velocity.normalize() * (Util.Parabola(progress, Projectile.ai[2]));
                if (Projectile.velocity.X > 0)
                {
                    owner.direction = 1;
                    owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (Projectile.Center - owner.Center).ToRotation() - (float)(Math.PI * 0.5f));
                }
                else
                {
                    owner.direction = -1;
                    owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (Projectile.Center - owner.Center).ToRotation() - (float)(Math.PI * 0.5f));
                }
            }
            else
            {
                float RotF = 4f;
                alpha = 1;
                scale = 1.8f;
                Projectile.rotation = Projectile.velocity.ToRotation() + (RotF * -0.5f + RotF * Util.GetRepeatedCosFromZeroToOne(progress, 3)) * Projectile.ai[0] * (Projectile.velocity.X > 0 ? -1 : 1);
                Projectile.Center = Projectile.getOwner().MountedCenter;
                if (progress > 0.2f && shoot)
                {
                    shoot = false;

                    for (int i = 0; i < 2; i++)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.normalize() * 28 + Util.randomPointInCircle(8), ModContent.ProjectileType<FractalGhostBlade>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                    }
                }
                if (Projectile.velocity.X > 0)
                {
                    owner.direction = 1;
                    owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)(Math.PI * 0.5f));
                }
                else
                {
                    owner.direction = -1;
                    owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)(Math.PI * 0.5f));
                }
            }

            if (odr.Count > 60)
            {
                odr.RemoveAt(0);
            }
            
            owner.heldProj = Projectile.whoAmI;
            owner.itemTime = 2;
            owner.itemAnimation = 2;
            if (counter > MaxUpdateTimes)
            {
                owner.itemTime = 1;
                owner.itemAnimation = 1;
                Projectile.Kill();
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public bool playHitSound = true;
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<SoulDisorder>(), 460);
            if (playHitSound || Projectile.ai[0] == 2)
            {
                playHitSound = false;
                Util.PlaySound("sf_hit", 1, Projectile.Center);
                Util.PlaySound("FractalHit", 1, Projectile.Center);
            }
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.TrueExcalibur, new ParticleOrchestraSettings
            {
                PositionInWorld = target.Center,
                MovementVector = Vector2.Zero
            });
        }
        public bool spawnProj = true;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Projectile.GetTexture();
            int dir = (int)(Projectile.ai[0]) * (Projectile.velocity.X > 0 ? -1 : 1);
            if (Projectile.ai[0] == 2)
            {
                dir = Math.Sign(Projectile.velocity.X);
            }
            Vector2 origin = dir > 0 ? new Vector2(0, tex.Height) : new Vector2(tex.Width, tex.Height);
            if (Projectile.ai[0] == 2)
            {
                origin = tex.Size() * 0.5f;
            }
            SpriteEffects effect = dir > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            float rot = dir > 0 ? Projectile.rotation + MathHelper.PiOver4 : Projectile.rotation + MathHelper.Pi * 0.75f;

            float MaxUpdateTime = Projectile.getOwner().itemTimeMax * Projectile.MaxUpdates;

            Main.EntitySpriteDraw(tex, Projectile.Center + Projectile.getOwner().gfxOffY * Vector2.UnitY - Main.screenPosition, null, lightColor * alpha, rot, origin, Projectile.scale * scale * 1.1f, effect);

            Main.spriteBatch.UseBlendState(BlendState.Additive);
            Texture2D bs = Util.getExtraTex("SemiCircularSmear");

            Color color1 = new Color(48, 52, 79);
            Color color2 = new Color(242, 201, 190);

            float zScale = Projectile.ai[0] == 2 ? 0.5f : 1;

            float zAlpha = (float)(Math.Cos(Util.GetRepeatedCosFromZeroToOne(counter / MaxUpdateTime, 3) * MathHelper.Pi - MathHelper.PiOver2));
            if (Projectile.ai[0] == 2)
            {
                zAlpha = 1;
            }
            Main.spriteBatch.Draw(bs, Projectile.Center + Projectile.getOwner().gfxOffY * Vector2.UnitY - Main.screenPosition, null, Color.Lerp(color1, color2, (float)counter / (Projectile.MaxUpdates * Projectile.getOwner().itemTimeMax)) * zAlpha, Projectile.rotation + MathHelper.ToRadians(32) * -dir, bs.Size() / 2f, Projectile.scale * 1.9f * scale * zScale, SpriteEffects.None, 0);
            Main.spriteBatch.Draw(bs, Projectile.Center + Projectile.getOwner().gfxOffY * Vector2.UnitY - Main.screenPosition, null, Color.Lerp(color2, color1, (float)counter / (Projectile.MaxUpdates * Projectile.getOwner().itemTimeMax)) * zAlpha, Projectile.rotation + MathHelper.ToRadians(32) * -dir, bs.Size() / 2f, Projectile.scale * 1.7f * scale * zScale, SpriteEffects.None, 0);

            Main.spriteBatch.ExitShaderRegion();

            return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return Utilities.Util.LineThroughRect(Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * (152) * Projectile.scale * scale, targetHitbox, 64);
        }
        public override void CutTiles()
        {
            Utils.PlotTileLine(Projectile.Center, Projectile.Center + Projectile.rotation.ToRotationVector2() * (160) * Projectile.scale * scale, 84, DelegateMethods.CutTiles);
        }

    }
}