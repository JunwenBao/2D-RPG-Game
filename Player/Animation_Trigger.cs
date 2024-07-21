using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Animation_Trigger : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    //������������
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

    //�������
    public void ShootTrigger()
    {
        player.isPulled = false;
        player.animationShootTrigger();
    }

    public void isPulled()
    {
        player.isPulled = true;
    }

    public void isShot()
    {
        player.isShot = true;
    }

}
