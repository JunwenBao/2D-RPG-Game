using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine _stateMachine, string animBoolName) : base(player, _stateMachine, animBoolName)
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

        if(Input.GetKeyDown(KeyCode.Q)) 
            stateMachine.ChangeState(player.counterAttack);

        //����״̬:ʹ��GetKey()����ʹ��GetKeyDown()��ԭ�򣿿��԰�ס��꣬ʵ��������
        if(Input.GetKey(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);

        //���û��⵽���棬�����Air״̬
        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        //�����¿ո��ʱ������״̬"JumpState" && ��ɫ�ӵء�
        //��"Ground State"״̬����"Jump State"��һ���ô�����ֹ������Ծ�ĳ��֡�
        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }
}
