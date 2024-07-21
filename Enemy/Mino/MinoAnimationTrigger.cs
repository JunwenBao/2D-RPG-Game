using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinoAnimationTrigger : MonoBehaviour
{
    private Enemy_Mino enemy => GetComponentInParent<Enemy_Mino>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();
                enemy.stats.doDamage(target);

                hit.GetComponent<Player>().DamageEffect();
            }
        }
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
