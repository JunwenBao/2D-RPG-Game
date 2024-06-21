using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine _stateMachine, string animBoolName) : base(player, _stateMachine, animBoolName)
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

        //在水平移动时，赋予x轴上的速度。
        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        //切换状态条件：如果没有来自水平移动键的按下，则切换回Idel状态。
        if(xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
