using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour
{
    //������Ϣ
    public string name;          //����
    public string description1;  //����1
    public string description2;  //����2
    public int num1;             //��������1
    public int num2;             //��������2

    public GameObject paperOnBoard; //�������ϵ�����ֽ��

    //�������񣨰�ť�󶨣�
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
