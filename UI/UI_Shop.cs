using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

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
        itemText.text = _inventoryItem.stackSize.ToString();
    }
}
