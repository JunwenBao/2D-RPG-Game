using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public Player player;
    public PlayerStats stats;

    public int currency; //��Ǯ����

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    //�洢�ͼ�������
    public void loadData(GameDataScriptable _data)
    {
        this.currency = _data.currency;
    }

    public void saveData(ref GameDataScriptable _data)
    {
        _data.currency = this.currency;
    }
}
