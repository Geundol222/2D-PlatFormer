using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase0524<TState, TOwner> where TOwner : MonoBehaviour
{
    protected TOwner owner;
    protected StateMachine0524<TState, TOwner> stateMachine;

    public StateBase0524(TOwner owner, StateMachine0524<TState, TOwner> stateMachine)
    {
        this.owner = owner;
        this.stateMachine = stateMachine;
    }

    public abstract void Setup();

    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();
}
