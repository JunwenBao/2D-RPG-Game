using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region States
    public Skeleton_Idle_State      idleState    { get; set; }
    public Skeleton_Move_State      moveState    { get; set; }
    public Skeleton_Battle_State    battleState  { get; set; }
    public Skeleton_Attack_State    attackState  { get; set; }
    public Skeleton_Stunn_State     stunnState   { get; set; }
    public Skeleton_Dead_State      deadState    { get; set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState   = new Skeleton_Idle_State     (this, stateMachine, "Idle", this);
        moveState   = new Skeleton_Move_State     (this, stateMachine, "Move", this);
        battleState = new Skeleton_Battle_State   (this, stateMachine, "Move", this);
        attackState = new Skeleton_Attack_State   (this, stateMachine, "Attack", this);
        stunnState  = new Skeleton_Stunn_State    (this, stateMachine, "Stunn", this);
        deadState   = new Skeleton_Dead_State     (this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void DamageEffect()
    {
        base.DamageEffect();

        stateMachine.ChangeState(stunnState);
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    /*
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            //stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
    */
}
