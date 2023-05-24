using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine0524<TState, TOwner> where TOwner : MonoBehaviour
{
    private TOwner owner;
    private Dictionary<TState, StateBase0524<TState, TOwner>> states;
    private StateBase0524<TState, TOwner> curState;

    public StateMachine0524(TOwner owner)
    {
        this.owner = owner;
        this.states = new Dictionary<TState, StateBase0524<TState, TOwner>>();
    }

    public void AddState(TState state, StateBase0524<TState, TOwner> stateBase)
    {
        states.Add(state, stateBase);
    }

    public void Setup(TState startState)
    {
        foreach(StateBase0524<TState, TOwner> state in states.Values)
        {
            state.Setup();
        }

        curState = states[startState];
        curState.Enter();
    }

    public void Update()
    {
        curState.Update();
    }

    public void ChangeState(TState newState)
    {
        curState.Exit();
        curState = states[newState];
        curState.Enter();
    }

    
}
