using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//作用：敌人死亡时，让物品掉落。在掉落时赋予该物品Prefab和物品的信息

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop; //可能掉落的物品列表
    private List<ItemData> dropList = new List<ItemData>(); 

    [SerializeField] private GameObject dropPerfab;

    public virtual void generateDrop()
    {
        for(int i = 0; i < possibleDrop.Length; i++)
        {
            if(Random.Range(0, 100) <= possibleDrop[i].dropChance)
                dropList.Add(possibleDrop[i]);
        }

        for(int i = 0;i < possibleItemDrop; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];

            dropList.Remove(randomItem);
            dropItem(randomItem);
        }
    }

    //当敌人死亡时，调用这个函数，使物品可以掉落
    protected void dropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPerfab, transform.position, Quaternion.identity);

        //让物品掉落的方向随机
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().setupItem(_itemData, randomVelocity);
    }
}
