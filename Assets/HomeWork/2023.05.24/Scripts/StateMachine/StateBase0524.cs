using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase0524<TOwner> where TOwner : MonoBehaviour
{
    protected TOwner owner;

    public StateBase0524(TOwner owner)
    {
        this.owner = owner;
    }

    public abstract void Setup();

    public abstract void Enter();

    public abstract void Update();

    public abstract void Exit();
}
