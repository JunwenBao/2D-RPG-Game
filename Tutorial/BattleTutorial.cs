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

    // �������ĺ���
    private void ShowBattleTutorial()
    {
        Debug.Log("���Ǵ��������壬�¼���������");
    }
}
