using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino_Die_State : EnemyState
{
    Enemy_Mino enemy;

    public Mino_Die_State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Mino _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        if (MissionManager.instance.num2 > 0)
            MissionManager.instance.num2--;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
