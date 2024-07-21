using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Shop_InventorySlot : UI_ItemSlot
{
    [SerializeField] private GameObject confirmBoard;
    private bool isOpend_confirm = false;

    //按下按键后，唤出确认界面
    public override void OnPointerDown(PointerEventData eventData)
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
    }
}
