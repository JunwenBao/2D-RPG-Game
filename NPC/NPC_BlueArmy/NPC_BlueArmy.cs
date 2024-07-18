using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BlueArmy : NPC
{
    public BlueArmy_Idle_State idleState { get; set; }

    public GameObject button;
    public GameObject bubble;

    protected override void Awake()
    {
        base.Awake();

        idleState = new BlueArmy_Idle_State(this, stateMachine, "Idle");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if (isTalking)
        {
            bubble.SetActive(false);
            button.SetActive(false);
        }
        else 
        { 
            bubble.SetActive(false);
            //button.SetActive(true);
        }
    }
}
