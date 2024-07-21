using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//作用：商店界面的每一个格子

public class UI_Shop : MonoBehaviour, IPointerDownHandler
{
    [Header("Item Info")]
    [SerializeField] private InventoryItem item;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;


    [Header("Price")]
    [SerializeField] private TextMeshProUGUI price;

    [Header("Confirm Board")]
    [SerializeField] private GameObject confirmBoard;
    private bool isOpend_confirm = false;


    private void Start()
    {
        updateSlot(item);

        price.text = item.data.buyPrice.ToString();
    }

    private void Update()
    {

    }

    //点击图标：弹出确定购买界面
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isOpend_confirm)
        {
            confirmBoard.SetActive(false);
            isOpend_confirm = false;
        }
        else
        {
            confirmBoard.SetActive(true);
            isOpend_confirm = true;
        }

        UI_Shop_Slot.instance.item = item;
    }

    //更新物品的图片+数量
    private void updateSlot(InventoryItem _inventoryItem)
    {
        itemImage.sprite = _inventoryItem.data.backgroundIcon;
        itemText.text = _inventoryItem.stackSize.ToString();
    }

    public void buy()
    {
        InventoryItem tmp_item = UI_Shop_Slot.instance.item;

        //如果玩家可以买得起
        if (Inventory.instance.getCoinAmount() > tmp_item.data.buyPrice)
        {
            Inventory.instance.addItem(tmp_item.data);
            Inventory.instance.decreaseCoin(tmp_item.data.buyPrice);
            tmp_item.removeStack();
            item.stackSize = tmp_item.stackSize;
        }
        else
        {
            Debug.Log("NO MONEY");
        }
    }
}
