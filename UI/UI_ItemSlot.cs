using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
//���ڸ��²���Ⱦ����UI

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Item Info")]
    [SerializeField] private Image itemImage;           //Item Icon
    [SerializeField] private TextMeshProUGUI itemText;  //Item Amount

    private UI ui;
    public InventoryItem item;

    private void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _item)
    {
        item = _item;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.backgroundIcon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = " ";
            }
        }
    }

    //��װ����UI���Ƴ�һ����Ʒ
    public void cleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text= "";
    }

    //�������ʱ����
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (item == null) return;

        if(Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.removeItem(item.data);
            return;
        }

        if(item.data.itemType == ItemType.Equipment)
            Inventory.instance.equipItem(item.data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null) return; //����Ʒ��Ϊ�գ�ֱ�ӷ��أ���ֹ����

        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0, yOffset = 0;

        if (mousePosition.x > 600) xOffset = -150;
        else xOffset = 150;

        if (mousePosition.y > 320) yOffset = -150;
        else yOffset = 150;

        ui.itemToolTip.showToolTip(item.data as ItemData_Equipment);
        ui.itemToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(item == null) return; //����Ʒ��Ϊ�գ�ֱ�ӷ��أ���ֹ����

        ui.itemToolTip.hideToolTip();
    }
}
