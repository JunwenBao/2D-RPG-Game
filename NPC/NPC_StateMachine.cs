using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_StateMachine : MonoBehaviour
{
    public NPC_State currentState { get; private set; }

    public void Initialize(NPC_State _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(NPC_State _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
