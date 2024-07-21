using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Animator anim;
    public string checkpointID;
    public bool activationStatus;
    public GameObject bubble;

    private void Start()
    {
        anim = GetComponent<Animator>();

        bubble.SetActive(false);
    }

    [ContextMenu("Generate Checkpoint ID")]
    private void generateID()
    {
        checkpointID = System.Guid.NewGuid().ToString();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            activeCheckpoint();
        }
    }

    public void activeCheckpoint()
    {
        activationStatus = true;
        anim.SetBool("active", true);

        bubble.SetActive(true);
    }
}
