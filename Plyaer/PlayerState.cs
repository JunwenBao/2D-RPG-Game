using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;

    //��¼ÿ��״̬��ת����������
    private string animBoolName;

    //��¼Dash�Ŀ���ʱ��
    protected float stateTimer;

    //����������������
    protected bool triggerCalled;

    public PlayerState(Player player, PlayerStateMachine _stateMachine, string animBoolName)
    {
        this.player = player;
        this.stateMachine = _stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;

        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        //���϶�ȡ�Ƿ��������ˮƽ+��ֱ������ƶ������¡�
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        //���϶�ȡ���Դ�ֱ����y�����ϵ�ֵ��
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
