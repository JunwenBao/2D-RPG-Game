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

        //����Ӳֱ״̬����˸:�����ú�������delayʱ�䣬�ظ�ʱ�䣩
        enemy.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);

        stateTimer = enemy.stunDuration;

        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemy.stunned = false;

        enemy.fx.Invoke("CancelColorChange", 0);   //���뿪ʱȡ�����е�invoke
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }
}
