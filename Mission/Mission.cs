using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    //任务信息
    public string name;          //名字
    public string description1;  //描述1
    public string description2;  //描述2
    public int num1;             //描述数量1
    public int num2;             //描述数量2

    public GameObject paperOnBoard; //公告栏上的任务纸条

    //接受任务（按钮绑定）
    public void AcceptMission()
    {
        MissionManager mission = MissionManager.instance;

        mission.isTakeMission = true;
        mission.name = name;
        mission.description1 = description1;
        mission.description2 = description2;
        mission.num1 = num1;
        mission.num2 = num2;

        Destroy(paperOnBoard);
        this.gameObject.SetActive(false);
    }
}
