using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_State : MonoBehaviour
{
    protected NPC_StateMachine stateMachine;
    protected NPC NPCBase;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public NPC_State(NPC _NPCBase, NPC_StateMachine _stateMachine, string _animBoolName)
    {
        this.NPCBase = _NPCBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.time;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        NPCBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        NPCBase.anim.SetBool(animBoolName, false);
        //NPCBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
