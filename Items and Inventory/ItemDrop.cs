using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//���ã���������ʱ������Ʒ���䡣�ڵ���ʱ�������ƷPrefab����Ʒ����Ϣ

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop; //���ܵ������Ʒ�б�
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

    //����������ʱ���������������ʹ��Ʒ���Ե���
    protected void dropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPerfab, transform.position, Quaternion.identity);

        //����Ʒ����ķ������
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().setupItem(_itemData, randomVelocity);
    }
}
