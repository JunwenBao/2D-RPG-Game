using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//作用：记录背包中的单一物品的信息和数量

[Serializable]
public class InventoryItem
{
    public ItemData data;   //记录物品的形式（数据）
    public int stackSize;   //记录物品的数量

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        addStack();
    }

    //物品数量增加/减少的函数
    public void addStack() => stackSize++;
    public void removeStack() => stackSize--;
}
