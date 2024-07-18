using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_ShopSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private Image itemImage1;

    public InventoryItem item;

    private void Start()
    {
        updateSlot(item);
    }

    private void Update()
    {
        
    }

    private void updateSlot(InventoryItem _inventoryItem)
    {
        itemImage.sprite = _inventoryItem.data.backgroundIcon;
        amount.text = _inventoryItem.stackSize.ToString();
    }
}
