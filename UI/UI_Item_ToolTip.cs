using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//�����ŵ�װ��ʱ����ʾװ�����������

public class UI_Item_ToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 20;

    //��ʾ������壬����ֵ
    public void showToolTip(ItemData_Equipment item)
    {
        if (item == null) return; //������

        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.getDiscription();

        //�������������һ������������С����
        if (itemNameText.text.Length > 12)
            itemNameText.fontSize = itemNameText.fontSize * .9f;
        else
            itemNameText.fontSize = defaultFontSize;

        gameObject.SetActive(true);
    }

    //�����������
    public void hideToolTip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
