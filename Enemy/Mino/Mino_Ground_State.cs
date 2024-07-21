using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino_Ground_State : EnemyState
{
    protected Enemy_Mino enemy;
    protected Transform player;

    public Mino_Ground_State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Mino _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //Detect the enemy
        if (enemy.IsPlayerDetected() && !enemy.stunned)// || Vector2.Distance(enemy.transform.position, player.transform.position) < 2)
            stateMachine.ChangeState(enemy.battleState);
    }
}
