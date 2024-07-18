using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Trigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    //´¥·¢¹¥»÷»úÖÆ
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().DamageEffect();

                EnemyStats _target = hit.GetComponent<EnemyStats>();

                player.stats.doDamage(_target);

                //Inventory.instance.getEquipment(EquipmentType.Weapon).ExecuteItemEffect(_target.transform);
            }
        }
    }
}
