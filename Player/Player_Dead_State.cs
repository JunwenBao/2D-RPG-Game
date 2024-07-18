using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Dead_State : PlayerState
{
    public Player_Dead_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) { }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        GameObject.Find("Canvas").GetComponent<UI>().switchOnEndScreen();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();
    }
}
