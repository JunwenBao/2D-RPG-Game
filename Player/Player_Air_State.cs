using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Air_State : PlayerState
{
    public Player_Air_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) { }

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

        //If detect the ground, change state to IDLE STATE  
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        //When the character on air, still can move on x-axis, but slower than ground
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);
    }
}
