using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Jump_State : PlayerState
{
    public Player_Jump_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce); //jump distance
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //在起跳的过程当中可以水平移动
        if (xInput != 0)
            player.SetVelocity(player.moveSpeed * .8f * xInput, rb.velocity.y);

        //当下降时，触发Air状态
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);
    }
}
