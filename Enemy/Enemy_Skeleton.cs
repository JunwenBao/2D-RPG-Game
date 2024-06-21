using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    #region States
    public SkeletonIdelState idleState {  get; set; }
    public SkeletonMoveState moveState { get; set; }
    public SkeletonBattleState battleState { get; set; }
    public SkeletonAttackState attackState { get; set; }  
    public SkeletonStunnedState stunnedState { get; set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState       = new SkeletonIdelState(this, stateMachine, "Idel", this);
        moveState       = new SkeletonMoveState(this, stateMachine, "Move", this);
        battleState     = new SkeletonBattleState(this, stateMachine, "Move", this);
        attackState     = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState    = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
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

    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }
}