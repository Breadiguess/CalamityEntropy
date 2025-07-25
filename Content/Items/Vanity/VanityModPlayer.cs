using Terraria.ModLoader;

namespace CalamityEntropy.Content.Items.Vanity
{
    public class VanityModPlayer : ModPlayer
    {
        //һ��ModPlayer�����ڽ�ʱװЧ�����������

        //��ǰװ����ʱװ������Ϊδװ��
        public string vanityEquipped = "";

        public override void ResetEffects()
        {
            //ÿ֡����Ч��
            vanityEquipped = "";
        }

        public override void FrameEffects()
        {
            //��һ���ǰ�����װ����ʱװ��vanityEquipped��Ϊ�գ����������ͼ��Ϊ��ʱװ
            if (vanityEquipped != "")
            {
                Player.legs = EquipLoader.GetEquipSlot(Mod, vanityEquipped, EquipType.Legs);
                Player.body = EquipLoader.GetEquipSlot(Mod, vanityEquipped, EquipType.Body);
                Player.head = EquipLoader.GetEquipSlot(Mod, vanityEquipped, EquipType.Head);
            }
        }
    }
}