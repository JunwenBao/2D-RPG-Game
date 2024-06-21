using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine _stateMachine, string animBoolName) : base(player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //���������Ĺ����м�⵽Wall���򴥷�"Wall Slide"״̬��
        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);

        //����ɫ�ӵغ󣺴���״̬"Idel State"��
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        if(xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);
    }
}