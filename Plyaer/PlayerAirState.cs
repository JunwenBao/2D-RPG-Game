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

        //如果在下落的过程中检测到Wall，则触发"Wall Slide"状态。
        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);

        //当角色接地后：触发状态"Idel State"。
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        if(xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);
    }
}
