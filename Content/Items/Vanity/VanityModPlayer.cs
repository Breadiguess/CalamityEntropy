using CalamityEntropy.Content.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Vanity
{
    public class VanityModPlayer : ModPlayer
    {
        //һ��ModPlayer�����ڽ�ʱװЧ�����������

        //��ǰװ����ʱװ������Ϊδװ��
        public string vanityEquipped = "";

        public bool TheocracyMark = false;
        public override void PostUpdate()
        {
            if(TheocracyMark)
            {
                if (Player.ownedProjectileCounts[ModContent.ProjectileType<Theostring>()] < 1)
                {
                    if(Main.myPlayer == Player.whoAmI)
                    {
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<Theostring>(), 0, 0, Player.whoAmI, -1);
                        Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, Vector2.Zero, ModContent.ProjectileType<Theostring>(), 0, 0, Player.whoAmI, 1);
                    }
                }
            }
        }
        public override void ResetEffects()
        {
            //ÿ֡����Ч��
            TheocracyMark = false;
            vanityEquipped = "";
        }

        public override void FrameEffects()
        {
            //��һ���ǰ�����װ����ʱװ��vanityEquipped��Ϊ�գ����������ͼ��Ϊ��ʱװ
            if (TheocracyMark)
            {
                Player.legs = EquipLoader.GetEquipSlot(Mod, "TheocracyMark", EquipType.Legs);
                Player.body = EquipLoader.GetEquipSlot(Mod, "TheocracyMark", EquipType.Body);
            }
            if (vanityEquipped != "")
            {
                
                Player.legs = EquipLoader.GetEquipSlot(Mod, vanityEquipped, EquipType.Legs);
                Player.body = EquipLoader.GetEquipSlot(Mod, vanityEquipped, EquipType.Body);
                Player.head = EquipLoader.GetEquipSlot(Mod, vanityEquipped, EquipType.Head);
                
            }
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            if (TheocracyMark)
                drawInfo.colorHair = Color.Transparent;
        }
    }
}