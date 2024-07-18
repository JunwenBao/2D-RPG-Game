using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//当鼠标放到装备时，显示装备的属性面板

public class UI_Item_ToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 20;

    //显示属性面板，并赋值
    public void showToolTip(ItemData_Equipment item)
    {
        if (item == null) return; //防报错

        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.getDiscription();

        //如果字数超过了一定数量，则缩小字体
        if (itemNameText.text.Length > 12)
            itemNameText.fontSize = itemNameText.fontSize * .9f;
        else
            itemNameText.fontSize = defaultFontSize;

        gameObject.SetActive(true);
    }

    //隐藏属性面板
    public void hideToolTip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
