using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    //1.角色的信息
    #region Information
    [Header("Attack Detail")]
    public Vector2[] attackMovement;            //每段攻击的移动距离，使用数组存储
    public float counterAttackDuration = .2f;

    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact; //接到剑后玩家的后坐力大小
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    [Header("Dash Info")]
    public float dashSpeed = 20f;
    public float dashDuration;
    private float defaultDashSpeed;
    public float dashDir { get; private set; }
    public bool isBusy { get; private set; }

    [Header("Communication")]
    public bool isTalking;
    public GameObject bubble;

    [Header("Arrow")]
    public bool isPulled; //角色是否拉弓
    public bool isShot;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private int arrowDamage;

    public bool isPointing; //判断角色是否在点击鼠标

    private CharacterStats myStats;

    #endregion

    //2.角色的状态机：需要在Awake()中初始化
    #region States
    public PlayerStateMachine  stateMachine  { get; private set; }
    public Player_Idle_State   idleState     { get; private set; }
    public Player_Move_State   moveState     { get; private set; }
    public Player_Jump_State   jumpState     { get; private set; }
    public Player_Air_State    airState      { get; private set; }
    public Player_Squat_State  squatState    { get; private set; }
    public Player_Attack_State attackState   { get; private set; }
    public Player_Dead_State   deadState     { get; private set; }
    public Player_Shot_State   shotState     { get; private set; }
    public Player_ShotRelease_State release_State { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine    ();
        idleState    = new Player_Idle_State     (this, stateMachine, "Idle");
        moveState    = new Player_Move_State     (this, stateMachine, "Move");
        jumpState    = new Player_Jump_State     (this, stateMachine, "Jump");
        airState     = new Player_Air_State      (this, stateMachine, "Fall");
        squatState   = new Player_Squat_State    (this, stateMachine, "Squat");
        attackState  = new Player_Attack_State   (this, stateMachine, "Attack");
        deadState    = new Player_Dead_State     (this, stateMachine, "Die");
        shotState    = new Player_Shot_State     (this, stateMachine, "Shot");
        release_State = new Player_ShotRelease_State(this, stateMachine, "Release");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
        
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        //Communication: show the dialog bubble
        if (isTalking) bubble.SetActive(true);
        else bubble.SetActive(false);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        //Wait for certain seconds.
        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }

    //Set the "triggerCalled" to TRUE
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    public override void slowEntityBy(float _slowPercent, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercent);
        jumpForce = jumpForce * (1 - _slowPercent);
        dashSpeed = dashSpeed * (1 - _slowPercent);
        anim.speed = anim.speed * (1 - _slowPercent);

        Invoke("returnDefaultSpeed", _slowDuration);

    }

    protected override void returnDefaultSpeed()
    {
        base.returnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void animationShootTrigger()
    {
        GameObject newArrow = Instantiate(arrowPrefab, attackCheck.position, Quaternion.identity);

        newArrow.GetComponent<Arrow_Controller>().setupArrow(arrowSpeed * facingDir, stats);
    }
}
