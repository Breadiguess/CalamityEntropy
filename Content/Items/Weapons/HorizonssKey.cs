using System.Collections.Generic;
using CalamityEntropy.Common;
using CalamityEntropy.Content.Projectiles.SamsaraCasket;
using CalamityEntropy.Util;
using CalamityMod;
using CalamityMod.Buffs.StatDebuffs;
using CalamityMod.Items;
using CalamityMod.Items.LoreItems;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Weapons
{
    public class NoneTypeDamageClass : DamageClass
    {
        internal static NoneTypeDamageClass Instance;


        public override void Load()
        {
            Instance = this;
        }

        public override void Unload()
        {
            Instance = null;
        }

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            if(damageClass == Util.CUtil.rogueDC)
            {
                return new StatInheritanceData(0.35f, 0.35f, 0.35f, 0.35f, 0.35f);
            }
            return StatInheritanceData.Full;
        }
    }
    public class HorizonssKey : ModItem
	{
        public override bool AltFunctionUse(Player player) => true;
        public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
            Item.useTime = 64;
            Item.useAnimation = 64;
            Item.useStyle = ItemUseStyleID.RaiseLamp;
            Item.damage = 15;
            Item.crit = 5;
            Item.DamageType = NoneTypeDamageClass.Instance;
            Item.noMelee = true;
            Item.value = Item.buyPrice(silver: 1);
            Item.rare = ItemRarityID.Red;
            Item.Entropy().Legend = true;
		}

        public override void UpdateInventory(Player player)
        {
            Item.damage = (int)(15 * damageMul());
        }
        public override bool? UseItem(Player player)
        {

            if (player.altFunctionUse == 2)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    float dist = 400;
                    int npc = -1;
                    foreach (NPC n in Main.npc)
                    {
                        if (n.active && !n.friendly && Util.Util.getDistance(n.Center, Main.MouseWorld) < dist)
                        {
                            npc = n.whoAmI;
                            dist = Util.Util.getDistance(n.Center, Main.MouseWorld);
                        }
                    }
                    if (npc >= 0) {
                        player.MinionAttackTargetNPC = npc;
                    }
                }
            }
            else
            {
                player.Entropy().samsaraCasketOpened = !player.Entropy().samsaraCasketOpened;
                if (Main.myPlayer == player.whoAmI && player.Entropy().samsaraCasketOpened)
                {
                    int p = Projectile.NewProjectile(player.GetSource_FromAI(), player.Center - new Vector2(0, 60), Vector2.Zero, ModContent.ProjectileType<e0>(), 0, 0, -1);
                    SoundEngine.PlaySound(new SoundStyle("CalamityEntropy/Assets/Sounds/AscendantActivate"), player.Center);
                }
            } 
            
            return true;
        }


        public override void HoldItem(Player player)
        {
            player.Entropy().sCasketLevel = 0;
            if (DownedBossSystem.downedHiveMind || DownedBossSystem.downedPerforator)
            {
                player.Entropy().sCasketLevel = 1;
            }
            if (Main.hardMode)
            {
                player.Entropy().sCasketLevel = 2;
            }
            if (NPC.downedPlantBoss)
            {
                player.Entropy().sCasketLevel = 3;
            }
            if (NPC.downedMoonlord)
            {
                player.Entropy().sCasketLevel = 4;
            }
            if (DownedBossSystem.downedDoG)
            {
                player.Entropy().sCasketLevel = 5;
            }
            if (DownedBossSystem.downedYharon)
            {
                player.Entropy().sCasketLevel = 6;
            }
            //Main.NewText(player.GetWeaponDamage(Item).ToString());
            if (player.ownedProjectileCounts[ModContent.ProjectileType<SamsaraCasketProj>()] < 1 && !player.HasBuff(ModContent.BuffType<NOU>()))
            {
                int p = Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<SamsaraCasketProj>(), Item.damage, player.GetWeaponKnockback(Item), player.whoAmI);
     
            }


        }

        public static float getVoidTouchLevel()
        {
            return EDownedBosses.downedCruiser ? 4 : 0;
        }

        public static int getArmorPen()
        {
            int ap = 0;
            if (NPC.downedAncientCultist)
            {
                ap += 20;
            }
            if (DownedBossSystem.downedSignus)
            {
                ap += 30;
            }
            ap += 46 * Main.LocalPlayer.Entropy().WeaponBoost;
            return ap;
        }
        public static int getLevel()
        {
            int j = 0;
            if (NPC.downedSlimeKing)
            {
                j++;
            }
            if (NPC.downedBoss1)
            {
                j++;
            }
            if (NPC.downedBoss2)
            {
                j++;
            }
            if (DownedBossSystem.downedPerforator || DownedBossSystem.downedHiveMind)
            {
                j++;
            }
            if (NPC.downedBoss3)
            {
                j++;
            }
            if (Main.hardMode)
            {
                j++;
            }
            if (DownedBossSystem.downedCryogen)
            {
                j++;
            }
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                j++;
            }
            if (NPC.downedPlantBoss)
            {
                j++;
            }
            if (NPC.downedGolemBoss)
            {
                j++;
            }
            if (NPC.downedAncientCultist)
            {
                j++;
            }
            if (NPC.downedMoonlord)
            {
                j++;
            }
            if (DownedBossSystem.downedDragonfolly)
            {
                j++;
            }
            if (DownedBossSystem.downedProvidence)
            {
                j++;
            }
            if (DownedBossSystem.downedSignus)
            {
                j++;
            }
            if (DownedBossSystem.downedPolterghast)
            {
                j++;
            }
            if (DownedBossSystem.downedDoG)
            {
                j++;
            }
            if (DownedBossSystem.downedYharon)
            {
                j++;
            }
            if (DownedBossSystem.downedExoMechs)
            {
                j++;
            }
            if (DownedBossSystem.downedCalamitas)
            {
                j++;
            }
            return j;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "Caskept Level", Mod.GetLocalization("hkLevel") + " " + getLevel().ToString() + "/20"));
        }

        public float damageMul()
        {
            float ad = 0.8f;  //��ʼ
            if (NPC.downedSlimeKing) //ʷ��ķ��
            {
                ad += 0.2f;
            }
            if (NPC.downedBoss1) //����
            {
                ad += 0.25f;
            }
            if (NPC.downedBoss2) //���Ի�����
            {
                ad += 0.25f;
            }
            if (NPC.downedBoss3) //������
            {
                ad += 0.6f;
            }
            if (Main.hardMode)  //��ɽ
            {
                ad += 0.6f;
            }
            if (DownedBossSystem.downedCryogen) //����
            {
                ad += 0.3f;
            }
            if (NPC.downedGolemBoss)   //ʯ����
            {
                ad += 0.3f;
            }
            if (NPC.downedAncientCultist)   //����а��ͽ
            {
                ad += 0.1f;
            }
            if (NPC.downedMoonlord)  //����
            {
                ad += 2f;
            }
            if (DownedBossSystem.downedProvidence)  //��������
            {
                ad += 0.5f;
            }
            if (DownedBossSystem.downedDragonfolly)  //�������
            {
                ad += 0.5f;
            }
            if (DownedBossSystem.downedSignus)   //����
            {
                ad += 0.5f;
            }
            if (DownedBossSystem.downedPolterghast)  //�Ļ�
            {
                ad += 0.5f;
            }
            if (DownedBossSystem.downedDoG) //����
            {
                ad += 0.5f;
            }
            if (DownedBossSystem.downedYharon)   //Ѽ��
            {
                ad += 1.5f;
            }
            if (DownedBossSystem.downedExoMechs)  //��е
            {
                ad += 2;
            }
            if (DownedBossSystem.downedCalamitas)   //����
            {
                ad += 2;
            }
            return ad;
        }
        public override void ModifyWeaponCrit(Player player, ref float crit)
        {
            float c = 0.0f;
            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
                c += 15f;
            }
            if (NPC.downedGolemBoss)
            {
                c += 10f;
            }
            if (NPC.downedAncientCultist)
            {
                c += 15f;
            }
            crit += c;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FallenStar, 2)
                .AddIngredient(ItemID.WoodenSword)
                .AddIngredient(ModContent.ItemType<LoreAwakening>())
                .AddTile(TileID.WorkBenches).Register();
        }

    }
}
