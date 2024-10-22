using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public bool isTakeMission;   //�Ƿ��������

    //������Ϣ
    public string name;          //����
    public string description1;  //����1
    public string description2;  //����2
    public int num1;             //��������1
    public int num2;             //��������2

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
