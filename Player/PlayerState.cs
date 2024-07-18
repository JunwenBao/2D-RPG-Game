using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    //1.Components:
    #region Components
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D rb;
    #endregion

    //2.Control Information
    #region Information
    protected float xInput; //x������
    protected float yInput; //y������

    private string animBoolName;    //��¼ÿ��״̬��ת����������
    protected float stateTimer;     //��ʱ��
    protected bool triggerCalled;   //����������������
    #endregion

    //3.Constructor:Initialize components
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);    //����ɫ��animator������Ϊtrue����������

        rb = player.rb;

        triggerCalled = false;
    }

    public virtual void Update()
    {
        //��ʱ��ʼ
        stateTimer -= Time.deltaTime;

        //���϶�ȡ�Ƿ��������ˮƽ+��ֱ������ƶ������¡�
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        //���϶�ȡ���Դ�ֱ����y�����ϵ�ֵ��
        //player.anim.SetFloat("yInput", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);   //�رն�������Enter()�Ĳ������
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
