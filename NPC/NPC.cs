using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Animator         anim   {  get; private set; }
    public BoxCollider2D    bc     { get; private set; }
    public NPC_StateMachine stateMachine { get; private set; }

    public Transform character;

    public bool isTalking; 

    #region Collision Detect
    [Header("Collision Info")]
    [SerializeField] public Transform    playerCheck;
    [SerializeField] public float        playerCheckDistance;
    [SerializeField] protected LayerMask whatIsPlayer;

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(transform.position, Vector2.right * -facingDir, playerCheckDistance, whatIsPlayer);

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + playerCheckDistance * -facingDir, playerCheck.position.y));
    }
    #endregion

    protected virtual void Awake() { }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        bc   = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        //If NPC detect player: Flip
        if (IsPlayerDetected() && transform.position.x < PlayerManager.instance.transform.position.x && facingRight)
        {
            Flip();
        }

        if (IsPlayerDetected() && transform.position.x < PlayerManager.instance.transform.position.x && !facingRight)
        {
            Flip();
        }
    }

    #region Flip
    public int facingDir { get; private set; } = 1; //角色朝向
    public bool facingRight = true;              //角色是否朝向右侧
    public System.Action onFlipped;
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        character.Rotate(0, 180, 0);

        if (onFlipped != null) onFlipped();
    }
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();

        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion
}
