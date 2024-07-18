using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string   itemName;
    public Sprite   itemIcon;
    public Sprite   backgroundIcon;
    public string   itemId;

    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    private void OnValidate()
    {
    //Ϊÿһ����Ʒ����ID
    #if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
    #endif
    }

    //��������������ToolTips
    public virtual string getDiscription()
    {
        return "";
    }
}
