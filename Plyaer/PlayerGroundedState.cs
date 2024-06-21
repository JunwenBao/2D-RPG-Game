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

        //攻击状态:使用GetKey()而不使用GetKeyDown()的原因？可以按住鼠标，实现连击。
        if(Input.GetKey(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);

        //如果没检测到地面，则进入Air状态
        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        //当按下空格键时：触发状态"JumpState" && 角色接地。
        //从"Ground State"状态触发"Jump State"的一个好处：防止无限跳跃的出现。
        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }
}
