using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//À×»÷ÉËº¦

public class ThunderStrike_Controller : MonoBehaviour
{
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = PlayerManager.instance.GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            EnemyStats enemyTarget = collision.GetComponent<EnemyStats>();

            playerStats.doMagicDamage(enemyTarget);
        }
    }

}
