using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public bool isTakeMission;   //是否接了任务

    //任务信息
    public string name;          //名字
    public string description1;  //描述1
    public string description2;  //描述2
    public int num1;             //描述数量1
    public int num2;             //描述数量2

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
