using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
//���ã�������Ʒ������

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public ItemType itemType;       //��Ʒ����
    public string   itemName;       //��Ʒ����
    public Sprite   itemIcon;       //��Ʒ͸��ͼ��
    public Sprite   backgroundIcon; //��Ʒ����ͼ��
    public string   itemId;         //��ƷID

    public int      buyPrice;       //����۸�
    public int      sellPrice;      //���ۼ۸�

    [Range(0, 100)]
    public float dropChance;        //��Ʒ�������

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
