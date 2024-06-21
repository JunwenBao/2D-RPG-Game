using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine _stateMachine, string animBoolName) : base(player, _stateMachine, animBoolName)
    {
    }

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

        //��ˮƽ�ƶ�ʱ������x���ϵ��ٶȡ�
        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        //�л�״̬���������û������ˮƽ�ƶ����İ��£����л���Idel״̬��
        if(xInput == 0 || player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
