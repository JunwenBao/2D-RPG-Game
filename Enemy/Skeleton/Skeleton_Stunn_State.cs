using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Stunn_State : EnemyState
{
    protected Enemy_Skeleton enemy;

    public Skeleton_Stunn_State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.stunned = true;

        Debug.Log("STUNNNN");

        //进入硬直状态后，闪烁:（调用函数名，delay时间，重复时间）
        enemy.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);

        stateTimer = enemy.stunDuration;

        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stunned = false;

        enemy.fx.Invoke("CancelColorChange", 0);   //在离开时取消所有的invoke
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
