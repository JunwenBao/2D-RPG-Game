using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
//�����˺�Ч����ʾ

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect")]
public class ItemEffect : ScriptableObject
{
    public virtual void excuteEffect(Transform _enemyPosition)
    {
        Debug.Log("eFFECT EXCUTE");
    }
}
