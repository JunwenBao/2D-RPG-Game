using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//这是一个角色的实体类，任何一个角色（主角，敌人）都会继承自它。
public class Entity : MonoBehaviour
{
    //1.组件定义：在Start()中为他们赋值
    #region Components
    public Animator          anim  { get; private set; }
    public Rigidbody2D       rb    { get; private set; }
    public EntityFX          fx    { get; private set; }
    public SpriteRenderer    sr    { get; private set; }
    public CharacterStats    stats { get; private set; }
    public CapsuleCollider2D cd    { get; private set; }
    #endregion

    //2.角色的基本信息：击退信息 + 碰撞信息
    #region Information
    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackDirection;  //击退方向
    [SerializeField] protected float knockbackDuration;     //击退持续时间
    protected bool isKnocked;
    public bool stunned = false;   //Judge whether the character is stunned

    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius; //攻击范围检测（圆）

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    [SerializeField] protected LayerMask whatIsGround;    //分配图层
    #endregion

    public System.Action onFlipped;

    //3.初始化三函数--------------------------------------------------------------
    protected virtual void Awake()
    {

    }

    //Start()：在加载角色的时候，读取该角色中或它的子类中的套件
    protected virtual void Start()
    {
        anim  = GetComponentInChildren<Animator>();
        rb    = GetComponent<Rigidbody2D>();
        fx    = GetComponent<EntityFX>();
        sr    = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponent<CharacterStats>();
        cd    = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }
    //------------------------------------------------------------------------

    //4.控制角色速度
    #region Veleocity
    //将角色速度设置为0
    public void SetZeroVelocity()
    {
        if (isKnocked) return;

        rb.velocity = new Vector2(0, 0);
    }

    //设置角色速度
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked) return; //被击中，则什么也不做

        rb.velocity = new Vector2(_xVelocity, _yVelocity);

        FlipController(_xVelocity);
    }
    #endregion

    //5.控制角色朝向 + 翻转
    #region Flip
    public int facingDir { get; private set; } = 1; //角色朝向
    protected bool facingRight = true;              //角色是否朝向右侧
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if(onFlipped != null) onFlipped();
    }
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();

        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion

    //6.角色隐形函数:如有需要，直接调用即可
    public void MakeTransprent(bool _transprenty)
    {
        if (_transprenty)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    //7.Collision Check
    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected()   => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius); //攻击范围检测
    }
    #endregion

    //8.Damage Control
    #region Damage
    public virtual void DamageEffect()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback");
        Debug.Log(gameObject.name + "I dame");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false; //为什么要再次设为false? 因为在下面的SetVelocity()中，如果角色被攻击，则不进行下面的代码。
    }
    #endregion

    public virtual void Die()
    {

    }

    public virtual void slowEntityBy(float _slowPercent, float _slowDuration)
    {

    }

    protected virtual void returnDefaultSpeed()
    {
        anim.speed = 1;
    }
}
