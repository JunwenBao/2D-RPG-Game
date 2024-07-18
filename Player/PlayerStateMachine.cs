using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }

    //Initialize state: Enter the state
    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }
    //Change state: Exit current state -> Enter another state
    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
