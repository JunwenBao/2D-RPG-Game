using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Controller : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float xVelocity;

    [SerializeField] private string targetLayerName = "Enemy";
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private bool canMove;
    [SerializeField] private bool fliped;

    private CharacterStats myStats;
    private void Update()
    {
        if(canMove)
            rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    public void setupArrow(float _speed, CharacterStats _myStats)
    {
        xVelocity = _speed;
        myStats = _myStats;

        if (PlayerManager.instance.player.facingDir == -1) transform.Rotate(0, 180, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(targetLayerName))
        {
            myStats.doDamage(collision.GetComponent<CharacterStats>());

            stuckInto(collision);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            stuckInto(collision);
    }

    private void stuckInto(Collider2D collision)
    {
        Debug.Log("STUCKKKKK");
        //GetComponentInChildren<ParticleSystem>().Stop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        canMove = false;
        xVelocity = 0;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = collision.transform;

        Destroy(gameObject, Random.Range(5, 7));
    }

    public void flipArrow()
    {
        if(fliped) return;

        xVelocity = xVelocity * -1;
        fliped = true;
        transform.Rotate(0, 180, 0);
        //targetLayerName = "Player"; ��ת�˺�
    }
}