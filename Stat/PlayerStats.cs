using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    private Player player;
    public Slider slider;

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

    //¼ÓÑª
    public void ModifyHealth(int num)
    {
        currentHealth += num;
        //slider.value = currentHealth / 100;
    }
}
