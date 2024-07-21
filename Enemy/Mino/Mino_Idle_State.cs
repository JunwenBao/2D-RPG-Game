using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino_Idle_State : Mino_Ground_State
{
    public Mino_Idle_State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Mino _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
}
