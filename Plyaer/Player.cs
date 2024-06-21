using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public bool isBusy {  get; private set; }

    [Header("Attack Detail")]
    public Vector2[] attackMovement;  //每段攻击的移动距离，使用数组存储
    public float counterAttackDuration = .2f;

    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce;

    [Header("Dash Info")]
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashSpeed = 20f;
    public float dashDuration;
    public float dashDir { get; private set; }

    #region States
    public PlayerStateMachine       stateMachine {  get; private set; }
    public PlayerIdleState          idleState { get; private set; }
    public PlayerMoveState          moveState { get; private set; }
    public PlayerJumpState          jumpState { get; private set; }
    public PlayerAirState           airState { get; private set; }
    public PlayerWallSlideState     wallSlideState { get; private set; }
    public PlayerWallJumpState      wallJumpState { get; private set; }
    public PlayerDashState          dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine    = new PlayerStateMachine();
        idleState       = new PlayerIdleState(this, stateMachine, "Idle");
        moveState       = new PlayerMoveState(this, stateMachine, "Move");
        jumpState       = new PlayerJumpState(this, stateMachine, "Jump");
        airState        = new PlayerAirState(this, stateMachine, "Jump");
        dashState       = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState  = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState   = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack   = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
    }

    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        //检查是否有Dash触发，因为Dash动作可以在地面和空中触发，所以要放在这个Update()下。
        checkForDashInput();
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        //Wait for certain seconds.
        yield return new WaitForSeconds(_seconds);

        isBusy = false; 
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    private void checkForDashInput()
    {
        if(IsWallDetected())
            return;

        dashUsageTimer -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;  //Dash冷却

            dashDir = Input.GetAxisRaw("Horizontal");

            //如果没有为Dash状态赋予方向，那么则为默认的面向方向。
            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
    }
}