using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using static UnityEditor.Progress;
//作用：背包类

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItems; //角色初始拥有的全部物品列表

    public List<InventoryItem> equipment;//角色已装备的物品列表
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;//角色未装备的物品列表
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List <InventoryItem> stash;   //角色的其他物品（金币等）列表
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform shop_InventorySlotParent; //商店的背包界面，与下面的四个不同
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_Shop_InventorySlot[] shop_InventoryItemnSlot; //商店的背包界面数组界面
    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("Database")]
    public List<InventoryItem> loadedItems; //用于存储角色身上的物品
    public List<ItemData_Equipment> loadedEquipments;

    public ItemData coin;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        //初始化组件
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

    //将角色初始拥有的装备添加到对应的装备库存栏中
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

    //添加装备
    public void equipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equipment oldEquipment = null; //旧装备：将要被移除的装备

        //如果字典中有同类型的装备，则移除再添加新装备
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

    //移除装备
    public void UnequipItem(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.removeModifiers();
        }
    }

    //更新背包的UI
    private void UpdateSlotUI()
    {
        //1.更新装备栏
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

        //2.更新物品栏
        for(int i = 0; i < inventoryItemSlot.Length; i++)
        {
            shop_InventoryItemnSlot[i].cleanUpSlot();
            inventoryItemSlot[i].cleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].cleanUpSlot();
        }

        //3.更新杂物栏
        for (int i = 0; i < inventory.Count; i++)
        {
            shop_InventoryItemnSlot[i].UpdateSlot(inventory[i]);
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for(int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        //4.更新角色属性栏
        for(int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    //向背包中增加物品，并根据物品的类型进一步划分
    #region Add Item

    //判断当前背包容量是否还可以继续装东西
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

    //背包中增加特定物品，并更新UI
    private void addToInventory(ItemData _item)
    {
        //1.在字典中找到该物品，将其数量+1，如果没有找到的话则新建一个物品
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

        //2.更新UI
        UpdateSlotUI();
    }
    private void addToStash(ItemData _item)
    {
        // 尝试从stashDictionary中获取与_item对应的InventoryItem对象
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.addStack(); // 如果获取成功，则调用该InventoryItem对象的addStack()方法
        }

        // 如果获取失败（即_item在stashDictionary中不存在，创建一个新的InventoryItem对象，并使用_item作为参数
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem); // 将新的InventoryItem对象添加到stash列表中
            stashDictionary.Add(_item, newItem);// 同时将_item作为key，新创建的InventoryItem对象作为value，添加到stashDictionary中
        }
    }
    #endregion

    //Craft??????????????????????????

    //从背包中移除特定物品，并更新UI
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
    //载入数据
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

    //保存数据
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

    //寻找所有的已知物品（无论角色是否拥有），并返回
    private List<ItemData> getItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();

        //寻找所有的装备（无论角色是否拥有），放入数组中
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

    //获取玩家所拥有的金钱数量
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
