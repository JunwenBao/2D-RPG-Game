using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Idle_State : Player_Ground_State
{
    public Player_Idle_State(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();//角色在进入站立状态下，速度应当设置为0
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //Change State: Idle -> Move
        if (xInput != 0 && !player.isBusy)
            stateMachine.ChangeState(player.moveState);
    }
}
