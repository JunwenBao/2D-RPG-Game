using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����һ����ɫ��ʵ���࣬�κ�һ����ɫ�����ǣ����ˣ�����̳�������
public class Entity : MonoBehaviour
{
    //1.������壺��Start()��Ϊ���Ǹ�ֵ
    #region Components
    public Animator          anim  { get; private set; }
    public Rigidbody2D       rb    { get; private set; }
    public EntityFX          fx    { get; private set; }
    public SpriteRenderer    sr    { get; private set; }
    public CharacterStats    stats { get; private set; }
    public CapsuleCollider2D cd    { get; private set; }
    #endregion

    //2.��ɫ�Ļ�����Ϣ��������Ϣ + ��ײ��Ϣ
    #region Information
    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackDirection;  //���˷���
    [SerializeField] protected float knockbackDuration;     //���˳���ʱ��
    protected bool isKnocked;
    public bool stunned = false;   //Judge whether the character is stunned

    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius; //������Χ��⣨Բ��

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    [SerializeField] protected LayerMask whatIsGround;    //����ͼ��
    #endregion

    public System.Action onFlipped;

    //3.��ʼ��������--------------------------------------------------------------
    protected virtual void Awake()
    {

    }

    //Start()���ڼ��ؽ�ɫ��ʱ�򣬶�ȡ�ý�ɫ�л����������е��׼�
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

    //4.���ƽ�ɫ�ٶ�
    #region Veleocity
    //����ɫ�ٶ�����Ϊ0
    public void SetZeroVelocity()
    {
        if (isKnocked) return;

        rb.velocity = new Vector2(0, 0);
    }

    //���ý�ɫ�ٶ�
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked) return; //�����У���ʲôҲ����

        rb.velocity = new Vector2(_xVelocity, _yVelocity);

        FlipController(_xVelocity);
    }
    #endregion

    //5.���ƽ�ɫ���� + ��ת
    #region Flip
    public int facingDir { get; private set; } = 1; //��ɫ����
    protected bool facingRight = true;              //��ɫ�Ƿ����Ҳ�
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

    //6.��ɫ���κ���:������Ҫ��ֱ�ӵ��ü���
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
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius); //������Χ���
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
        isKnocked = false; //ΪʲôҪ�ٴ���Ϊfalse? ��Ϊ�������SetVelocity()�У������ɫ���������򲻽�������Ĵ��롣
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
