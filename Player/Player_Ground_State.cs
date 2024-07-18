using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Ground_State : PlayerState
{
    public Player_Ground_State(Player _player, PlayerStateMachine _stateMachine, string animBoolName) : base(_player, _stateMachine, animBoolName) { }

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

        //Jump
        if(Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.jumpState);

        //Squat + Stop Squat
        if(Input.GetKey(KeyCode.S))
            stateMachine.ChangeState(player.squatState);
        if(Input.GetKeyUp(KeyCode.S))
            stateMachine.ChangeState(player.idleState);

        //Attack
        if (Input.GetKey(KeyCode.Mouse0))
            stateMachine.ChangeState(player.attackState);

        //Shot
        if (Input.GetKey(KeyCode.Mouse1))
            stateMachine.ChangeState(player.shotState);
    }
}
