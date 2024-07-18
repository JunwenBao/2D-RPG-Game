using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    //���ű�������ʱ����Inspector���κ�ֵ���޸ģ������Զ�����OnCalidate()

    //��Ⱦ��Ʒ��ͼƬ
    private void setupVisuals()
    {
        if (itemData == null) return;

        //����Ϸһ��ʼ����ȾͼƬ�����Ҹ���
        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
        gameObject.name = "Item Object - " + itemData.itemName;
    }

    public void setupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        setupVisuals();
    }

    //������Ʒ
    public void pickupItem()
    {
        //�жϣ��������Ƿ��������Ӷ��ж��Ƿ����ʰȡ��Ʒ
        if(!Inventory.instance.canAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7); //����Ʒ����һ��
            return;
        }

        Inventory.instance.addItem(itemData); //ʰȡ��Ʒ
        Destroy(gameObject);                  //����Ʒ����
    }
}
