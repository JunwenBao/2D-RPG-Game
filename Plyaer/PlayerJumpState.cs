using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerStateMachine _stateMachine, string animBoolName) : base(player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //������ʱ������ǽ���򴥷�Wall Slide״̬
        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);

        //�������Ĺ��̵��п���ˮƽ�ƶ�
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);

        //���½�ʱ������Air״̬
        if (rb.velocity.y < 0 )
            stateMachine.ChangeState(player.airState);
    }
}
