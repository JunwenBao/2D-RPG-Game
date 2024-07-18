using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//创造雷击效果

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item Effect/Thunder Strike")]
public class ThunderStrikEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;

    public override void excuteEffect(Transform _enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);

    }
}
