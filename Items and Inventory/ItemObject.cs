using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    //当脚本被加载时，或Inspector中任何值被修改，即会自动调用OnCalidate()

    //渲染物品的图片
    private void setupVisuals()
    {
        if (itemData == null) return;

        //在游戏一开始就渲染图片，并且改名
        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
        gameObject.name = "Item Object - " + itemData.itemName;
    }

    public void setupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        setupVisuals();
    }

    //捡起物品
    public void pickupItem()
    {
        //判断：看背包是否已满，从而判断是否可以拾取物品
        if(!Inventory.instance.canAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7); //让物品跳起一下
            return;
        }

        Inventory.instance.addItem(itemData); //拾取物品
        Destroy(gameObject);                  //将物品销毁
    }
}
