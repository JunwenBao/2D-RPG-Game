using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTutorial : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowBattleTutorial();
        }
    }

    // 被触发的函数
    private void ShowBattleTutorial()
    {
        Debug.Log("主角触碰到物体，事件被触发！");
    }
}
