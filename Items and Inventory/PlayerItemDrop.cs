using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's Drop")]
    [SerializeField] private float chanceToLooseItems;
    [SerializeField] private float chanceToLooseMaterials;

    public override void generateDrop()
    {
        Inventory inventory = Inventory.instance;

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToLose = new List<InventoryItem>();

        foreach(InventoryItem item in inventory.getEquipmentList())
        {
            if(Random.Range(0, 100) <= chanceToLooseItems)
            {
                dropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for(int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }

        foreach(InventoryItem item in inventory.getStashList())
        {
            if(Random.Range(0, 100) <= chanceToLooseMaterials)
            {
                dropItem(item.data);
                materialsToLose.Add(item);
            }
        }

        for(int i = 0; i < materialsToLose.Count; i++)
        {
            inventory.removeItem(materialsToLose[i].data);
        }
    }
}
