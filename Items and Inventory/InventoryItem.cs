using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���ã���¼�����еĵ�һ��Ʒ����Ϣ������

[Serializable]
public class InventoryItem
{
    public ItemData data;   //��¼��Ʒ����ʽ�����ݣ�
    public int stackSize;   //��¼��Ʒ������

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        addStack();
    }

    //��Ʒ��������/���ٵĺ���
    public void addStack() => stackSize++;
    public void removeStack() => stackSize--;
}
