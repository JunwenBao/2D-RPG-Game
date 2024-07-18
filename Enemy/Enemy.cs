using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    //Enemy Basic Information
    #region Information
    [Header("Stunned Info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;

    [SerializeField] protected LayerMask whatIsPlayer;   //用于检测玩家
    #endregion

    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();

        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 100, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

    //Freeze the Time
    #region Freeze
    public virtual void FreezeTime(bool _timerFrozen)
    {
        if (_timerFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }
    #endregion

    //Control Counter Window
    #region Counter Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    //Control the enemy dead result
    #region Dead
    public string lastAnimBoolName { get; private set; }

    public virtual void AssignLastAnimName(string _name) => lastAnimBoolName = _name;
    #endregion 

    public override void slowEntityBy(float _slowPercent, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 -  _slowPercent);
        anim.speed = anim.speed * (1 - _slowPercent);

        Invoke("returnDefaultSpeed", _slowDuration); //在规定时间后，触发指定函数
    }

    protected override void returnDefaultSpeed()
    {
        base.returnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
    }
}
