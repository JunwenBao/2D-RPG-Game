using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    //Judge whether player can use skill according to cooldown time.
    public virtual bool canUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            useSkill();
            //Use Skill
            cooldownTimer = cooldown;
            return true;
        }

        Debug.Log("Skill is on cooldown");
        return false;
    }

    public virtual void useSkill()
    {
        //Do some skill specific thing
    }
}
