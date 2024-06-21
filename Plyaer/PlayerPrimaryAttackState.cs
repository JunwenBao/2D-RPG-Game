using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttack;
    private float comboWindow = 2;

    public PlayerPrimaryAttackState(Player player, PlayerStateMachine _stateMachine, string animBoolName) : base(player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        xInput = 0; //fix bug on attack direction

        //如果：超过可连招数量 或 超过可连上的时间， 则：从头开始打连招
        if(comboCounter > 1 || Time.time >= lastTimeAttack + comboWindow)
            comboCounter = 0;
        player.anim.SetInteger("ComboCounter", comboCounter);

        //改变攻击速度
        //player.anim.speed = 3;

        #region Choose Attack Direction

        float attackDir = player.facingDir;
        if(xInput != 0)
            attackDir = xInput;

        #endregion

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].x);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .1f);

        //重置攻击速度
        //player.anim.speed = 1;

        comboCounter++;
        lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();

        //攻击时不可移动
        if (stateTimer < 0)
            player.SetZeroVelocity();

        if(triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
