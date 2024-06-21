using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine _stateMachine, string animBoolName) : base(player, _stateMachine, animBoolName)
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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if(xInput != 0 && player.facingDir != xInput)
            stateMachine.ChangeState(player.idleState);

        if(yInput < 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
        else//Wall Slide 时速度变慢
            rb.velocity = new Vector2(0, rb.velocity.y * .1f);

        //Wall Slide地面检测，接地后转为idel状态。
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
