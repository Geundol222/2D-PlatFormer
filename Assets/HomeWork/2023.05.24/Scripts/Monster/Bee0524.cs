using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeState0524;
using System.Buffers;

public class Bee0524 : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int patrolIndex = 0;
    public Vector2 dir;
    public SpriteRenderer render;

    public float detectRange;
    public float attackRange;
    public float moveSpeed;
    public Transform player;
    public Vector3 returnPosition;

    public StateBase0524<Bee0524>[] states;
    public State curState;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        states = new StateBase0524<Bee0524>[(int)State.Size];
        states[(int)State.Idle] = new IdleState(this);
        states[(int)State.Trace] = new TraceState(this);
        states[(int)State.Return] = new ReturnState(this);
        states[(int)State.Attack] = new AttackState(this);
        states[(int)State.Patrol] = new PatrolState(this);
    }

    private void Start()
    {
        curState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
        states[(int)curState].Setup();
    }

    private void Update()
    {
        states[(int)curState].Update();
    }

    private void FixedUpdate()
    {
        Turn();
    }

    private void Turn()
    {
        if (dir.x > 0)
            render.flipX = true;
        else if (dir.x < 0)
            render.flipX = false;
    }

    public void ChangeState(State state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Setup();
    }


}

namespace BeeState0524
{
    public enum State { Idle, Trace, Return, Attack, Patrol, Size }

    public class IdleState : StateBase0524<Bee0524>
    {
        float detectRange;
        Transform player;

        float idleTime;

        public IdleState(Bee0524 owner) : base(owner) { }

        public override void Setup()
        {
            player = owner.player;
            detectRange = owner.detectRange;

            Enter();
        }

        public override void Enter()
        {
            idleTime = 0;
        }

        public override void Update()
        {
            idleTime += Time.deltaTime;
            if (idleTime > 2)
            {
                owner.ChangeState(State.Patrol);
            }

            if (Vector2.Distance(player.position, owner.transform.position) < detectRange)
            {
                owner.ChangeState(State.Trace);
            }
        }

        public override void Exit()
        {
            
        }
    }

    public class TraceState : StateBase0524<Bee0524>
    {
        public TraceState(Bee0524 owner) : base(owner) { }

        public override void Setup()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {

        }

        public override void Exit()
        {

        }
    }

    public class ReturnState : StateBase0524<Bee0524>
    {
        public ReturnState(Bee0524 owner) : base(owner) { }

        public override void Setup()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {

        }

        public override void Exit()
        {

        }
    }

    public class AttackState : StateBase0524<Bee0524>
    {
        public AttackState(Bee0524 owner) : base(owner) { }

        public override void Setup()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {

        }

        public override void Exit()
        {

        }
    }

    public class PatrolState : StateBase0524<Bee0524>
    {
        public PatrolState(Bee0524 owner) : base(owner) { }

        public override void Setup()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {

        }

        public override void Exit()
        {

        }
    }
}
