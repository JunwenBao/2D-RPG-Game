using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BlueArmy : NPC
{
    public BlueArmy_Idle_State idleState { get; set; }

    public GameObject button;
    public GameObject bubble;

    public GameObject talkUI;
    public Sprite avatar;         //头像
    public TextAsset textFile;    //文本文件

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

        if (button.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            DialogManager.instance.avatar = avatar;
            DialogManager.instance.textFile = textFile;
            talkUI.SetActive(true);
        }
    }
}
