using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using static UnityEditor.Progress;
//���ã�������

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItems; //��ɫ��ʼӵ�е�ȫ����Ʒ�б�

    public List<InventoryItem> equipment;//��ɫ��װ������Ʒ�б�
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;//��ɫδװ������Ʒ�б�
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List <InventoryItem> stash;   //��ɫ��������Ʒ����ҵȣ��б�
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform shop_InventorySlotParent; //�̵�ı������棬��������ĸ���ͬ
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_Shop_InventorySlot[] shop_InventoryItemnSlot; //�̵�ı��������������
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("Database")]
    public List<InventoryItem> loadedItems; //���ڴ洢��ɫ���ϵ���Ʒ
    public List<ItemData_Equipment> loadedEquipments;

    public ItemData coin;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        //��ʼ�����
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        shop_InventoryItemnSlot = shop_InventorySlotParent.GetComponentsInChildren<UI_Shop_InventorySlot>();
        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        addStratingItem();
    }

    //����ɫ��ʼӵ�е�װ����ӵ���Ӧ��װ���������
    private void addStratingItem()
    {
        foreach(ItemData_Equipment item in loadedEquipments)
        {
            equipItem(item);
        }

        if(loadedItems.Count > 0)
        {
            foreach(InventoryItem item in loadedItems)
            {
                for(int i = 0; i < item.stackSize; i++)
                {
                    addItem(item.data);
                }
            }

            return;
        }

        for (int i = 0; i < startingItems.Count; i++)
        {
            addItem(startingItems[i]);
        }
    }

    //���װ��
    public void equipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null; //��װ������Ҫ���Ƴ���װ��

        //����ֵ�����ͬ���͵�װ�������Ƴ��������װ��
        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
                oldEquipment = item.Key;
        }

        if(oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            addItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.addModifiers();

        removeItem(_item);

        UpdateSlotUI();
    }

    //�Ƴ�װ��
    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.removeModifiers();
        }
    }

    //���±�����UI
    private void UpdateSlotUI()
    {
        //1.����װ����
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            equipmentSlot[i].cleanUpSlot();
        }

        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                    equipmentSlot[i].UpdateSlot(item.Value);
            }
        }

        //2.������Ʒ��
        for(int i = 0; i < inventoryItemSlot.Length; i++)
        {
            shop_InventoryItemnSlot[i].cleanUpSlot();
            inventoryItemSlot[i].cleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].cleanUpSlot();
        }

        //3.����������
        for (int i = 0; i < inventory.Count; i++)
        {
            shop_InventoryItemnSlot[i].UpdateSlot(inventory[i]);
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for(int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        //4.���½�ɫ������
        for(int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    //�򱳰���������Ʒ����������Ʒ�����ͽ�һ������
    #region Add Item

    //�жϵ�ǰ���������Ƿ񻹿��Լ���װ����
    public bool canAddItem()
    {
        if(inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.Log("no space");
            return false;
        }

        return true;
    }

    public void addItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && canAddItem())
        {
            addToInventory(_item);
        }
        else if(_item.itemType == ItemType.Material)
        {
            addToStash(_item);
        }

        UpdateSlotUI();
    }

    //�����������ض���Ʒ��������UI
    private void addToInventory(ItemData _item)
    {
        //1.���ֵ����ҵ�����Ʒ����������+1�����û���ҵ��Ļ����½�һ����Ʒ
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.addStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }

        //2.����UI
        UpdateSlotUI();
    }
    private void addToStash(ItemData _item)
    {
        // ���Դ�stashDictionary�л�ȡ��_item��Ӧ��InventoryItem����
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.addStack(); // �����ȡ�ɹ�������ø�InventoryItem�����addStack()����
        }

        // �����ȡʧ�ܣ���_item��stashDictionary�в����ڣ�����һ���µ�InventoryItem���󣬲�ʹ��_item��Ϊ����
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem); // ���µ�InventoryItem������ӵ�stash�б���
            stashDictionary.Add(_item, newItem);// ͬʱ��_item��Ϊkey���´�����InventoryItem������Ϊvalue����ӵ�stashDictionary��
        }
    }
    #endregion

    //Craft??????????????????????????

    //�ӱ������Ƴ��ض���Ʒ��������UI
    public void removeItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if(value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else value.removeStack();
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else stashValue.removeStack();
        }

        UpdateSlotUI();
    }

    public List<InventoryItem> getEquipmentList() => equipment;

    public List<InventoryItem> getStashList() => stash;

    #region Data Save&Load
    //��������
    public void loadData(GameData _data)
    {
        foreach(KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach(var item in getItemDataBase())
            {
                if(item != null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach(string loadedItemId in _data.equipmentId)
        {
            foreach(var item in getItemDataBase())
            {
                if(item != null && item.itemId == loadedItemId)
                {
                    loadedEquipments.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    //��������
    public void saveData(ref GameData _data)
    {
        _data.inventory.Clear();
        _data.equipmentId.Clear();

        foreach(KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach(KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
        }
    }

    //Ѱ�����е���֪��Ʒ�����۽�ɫ�Ƿ�ӵ�У���������
    private List<ItemData> getItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();

        //Ѱ�����е�װ�������۽�ɫ�Ƿ�ӵ�У�������������
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Items" }); 
         
        foreach(string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
    #endregion

    //��ȡ�����ӵ�еĽ�Ǯ����
    public int getCoinAmount()
    {
        if (stashDictionary.TryGetValue(coin, out InventoryItem value))
        {
            return value.stackSize;
        }

        return 0;
    }

    public void decreaseCoin(int _amount)
    {
        if (stashDictionary.TryGetValue(coin, out InventoryItem value))
        {
            value.stackSize -= _amount;
        }
    }
}
