using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UI_Mission : MonoBehaviour
{
    public TextMeshProUGUI text1;
    public TextMeshProUGUI text2;

    private void Update()
    {
        if(MissionManager.instance)
        {
            if(!MissionManager.instance.isTakeMission) this.gameObject.SetActive(false);

            else
            {
                text1.text = MissionManager.instance.description1 + "  *" + MissionManager.instance.num1;
                text2.text = MissionManager.instance.description2 + "  *" + MissionManager.instance.num2;
            }

            //当任务击杀怪物数量清0时，勾掉任务
            if(MissionManager.instance.num1 == 0)
            {
                text1.text = MissionManager.instance.description1;
                text1.fontStyle |= FontStyles.Strikethrough;
            }
            if (MissionManager.instance.num2 == 0)
            {
                text2.text = MissionManager.instance.description1;
                text2.fontStyle |= FontStyles.Strikethrough;
            }
        }
    }
}
