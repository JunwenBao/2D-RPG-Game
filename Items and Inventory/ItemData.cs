using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
//作用：保存物品的数据

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public ItemType itemType;       //物品类型
    public string   itemName;       //物品名字
    public Sprite   itemIcon;       //物品透明图标
    public Sprite   backgroundIcon; //物品背景图标
    public string   itemId;         //物品ID

    public int      buyPrice;       //购买价格
    public int      sellPrice;      //出售价格

    [Range(0, 100)]
    public float dropChance;        //物品掉落概率

    protected StringBuilder sb = new StringBuilder();

    private void OnValidate()
    {
        //为每一个物品分配ID
        #if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
        #endif
    }

    //生成描述：用于ToolTips
    public virtual string getDiscription()
    {
        return "";
    }
}
