using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public Player player;

    public int currency; //��Ǯ����

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    //�洢�ͼ�������
    public void loadData(GameData _data)
    {
        this.currency = _data.currency;
    }

    public void saveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }

}
