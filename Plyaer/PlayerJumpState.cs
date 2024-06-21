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

        //当起跳时触碰到墙，则触发Wall Slide状态
        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);

        //在起跳的过程当中可以水平移动
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);

        //当下降时，触发Air状态
        if (rb.velocity.y < 0 )
            stateMachine.ChangeState(player.airState);
    }
}
