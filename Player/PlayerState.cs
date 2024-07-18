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
    protected float xInput; //x轴输入
    protected float yInput; //y轴输入

    private string animBoolName;    //记录每个状态的转换条件名字
    protected float stateTimer;     //计时器
    protected bool triggerCalled;   //连续攻击触发开关
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
        player.anim.SetBool(animBoolName, true);    //将角色的animator条件设为true，触发动画

        rb = player.rb;

        triggerCalled = false;
    }

    public virtual void Update()
    {
        //计时开始
        stateTimer -= Time.deltaTime;

        //不断读取是否存在来自水平+垂直方向的移动键按下。
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        //不断读取来自垂直方向（y方向）上的值。
        //player.anim.SetFloat("yInput", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);   //关闭动画，于Enter()的操作相对
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
