using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack_State : PlayerState
{
    private int   comboCounter;     //Record the number of attack
    private float lastTimeAttack;   //The last time you attack
    private float comboWindow = 2;  //The combo window

    public Player_Attack_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        xInput = 0; //fix bug on attack direction
        stateTimer = .1f;

        //Control the combo number
        if (comboCounter > 1 || Time.time >= lastTimeAttack + comboWindow)
            comboCounter = 0;
        player.anim.SetInteger("ComboCount", comboCounter);

        //Control the attack direction
        float attackDir = player.facingDir;
        if(xInput != 0) attackDir = xInput;
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].x);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .1f);

        comboCounter++;
        lastTimeAttack = Time.time; 
    }

    public override void Update()
    {
        base.Update();

        //When you attack, you can not move
        if (stateTimer < 0)
            player.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
