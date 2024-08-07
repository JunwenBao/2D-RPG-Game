using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void takeDamage(int _damage)
    {
        base.takeDamage(_damage);

        player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();

        player.Die();

        isDead = true;

        GetComponent<PlayerItemDrop>().generateDrop();
    }
}
