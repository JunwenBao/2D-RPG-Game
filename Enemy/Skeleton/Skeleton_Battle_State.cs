using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_Battle_State : EnemyState
{
    private Transform player;
    private Enemy_Skeleton enemy;
    private int moveDir;

    private bool moveLock = false;  //Lock the movement if the skeleton is too close to the player

    public Skeleton_Battle_State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        //Use PlayerManager,instead of:
        //player = GameObject.Find("Player").transform;
        player = PlayerManager.instance.player.transform;

        Debug.Log("battle!");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //Control the move lock 
        if (Vector2.Distance(player.transform.position, enemy.transform.position) <= 1.5) moveLock = true;
        else moveLock = false;

        if (moveLock) enemy.SetZeroVelocity();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance && canAttack())
            {
                    stateMachine.ChangeState(enemy.attackState);
                    Debug.Log("change to attack state");
            }
        }
        else
        {
            //��������ʧ��IsPlayerDetected() �� �������룬���˻���stateTimerʱ���ת����Idle״̬��
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
                stateMachine.ChangeState(enemy.idleState);
        }

        //Change the direction according to the position of PLAYER
        if (player.position.x > enemy.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moveDir = -1;

        if(!moveLock)
            enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }

    //Judge whether Enemy can attack
    private bool canAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
