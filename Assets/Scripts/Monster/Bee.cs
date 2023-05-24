using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.UIElements;
using BeeState;

public class Bee : MonoBehaviour
{
    public float detectRange;
    public float attackRange;
    public float moveSpeed;
    public Transform[] patrolPoints;

    public Transform player;
    public Vector3 returnPosition;
    public int patrolIndex = 0;

    private StateBase<Bee>[] states;
    private State curState;

    public Vector2 dir;
    private SpriteRenderer render;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        states = new StateBase<Bee>[(int)State.Size];
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

namespace BeeState
{
    public enum State { Idle, Trace, Return, Attack, Patrol, Size }

    public class IdleState : StateBase<Bee>
    {
        private Transform player;
        private float moveSpeed;
        private float detectRange;

        private float idleTime;

        public IdleState(Bee owner) : base(owner) { }

        public override void Setup()
        {
            player = owner.player;
            moveSpeed = owner.moveSpeed;
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

            // 플레이어가 가까워졌을 때
            if (Vector2.Distance(player.position, owner.transform.position) < detectRange)
            {
                owner.ChangeState(State.Trace);
            }
        }

        public override void Exit()
        {

        }
    }

    public class TraceState : StateBase<Bee>
    {
        private Transform player;
        private float moveSpeed;
        private float detectRange;
        private float attackRange;

        public TraceState(Bee owner) : base(owner) { }

        public override void Setup()
        {
            player = owner.player;
            moveSpeed = owner.moveSpeed;
            detectRange = owner.detectRange;
            attackRange = owner.attackRange;

            Enter();
        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            owner.dir = (player.position - owner.transform.position).normalized;
            owner.transform.Translate(owner.dir * moveSpeed * Time.deltaTime);

            // 플레이어가 멀어졌을 때
            if (Vector2.Distance(player.position, owner.transform.position) > detectRange)
            {
                owner.ChangeState(State.Return);
            }
            // 공격범위 안에 있을 때
            else if (Vector2.Distance(player.position, owner.transform.position) < attackRange)
            {
                owner.ChangeState(State.Attack);
            }
        }

        public override void Exit()
        {

        }
    }

    public class ReturnState : StateBase<Bee>
    {
        private Vector3 returnPosition;
        private Transform player;
        private float moveSpeed;
        private float detectRange;

        public ReturnState(Bee owner) : base(owner) { }

        public override void Setup()
        {
            returnPosition = owner.returnPosition;
            player = owner.player;
            moveSpeed = owner.moveSpeed;
            detectRange = owner.detectRange;

            Enter();
        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            owner.dir = (returnPosition - owner.transform.position).normalized;
            owner.transform.Translate(owner.dir * moveSpeed * Time.deltaTime);

            // 원래 자리에 도착했으면
            if (Vector2.Distance(owner.transform.position, returnPosition) < 0.02f)
            {
                owner.ChangeState(State.Idle);
            }
            // 돌아가는 중에 다시 플레이어가 감지되면
            else if (Vector2.Distance(player.position, owner.transform.position) < detectRange)
            {
                owner.ChangeState(State.Trace);
            }
        }

        public override void Exit()
        {

        }
    }

    public class AttackState : StateBase<Bee>
    {
        private Transform player;
        private float attackRange;

        private float lastAttackTime;

        public AttackState(Bee owner) : base(owner) { }

        public override void Setup()
        {
            player = owner.player;
            attackRange = owner.attackRange;

            Enter();
        }

        public override void Enter()
        {
            lastAttackTime = 0;
        }

        public override void Update()
        {
            // 공격하기
            if (lastAttackTime > 1)
            {
                Debug.Log("공격");
                lastAttackTime = 0;
            }
            lastAttackTime += Time.deltaTime;

            if (Vector2.Distance(player.position, owner.transform.position) > attackRange)
            {
                owner.ChangeState(State.Trace);
            }
        }

        public override void Exit()
        {

        }
    }

    public class PatrolState : StateBase<Bee>
    {
        private Vector3 returnPosition;
        private Transform player;
        private float moveSpeed;
        private float detectRange;

        public PatrolState(Bee owner) : base(owner) { }

        public override void Setup()
        {
            returnPosition = owner.returnPosition;
            player = owner.player;
            moveSpeed = owner.moveSpeed;
            detectRange = owner.detectRange;

            Enter();
        }

        public override void Enter()
        {
            
        }

        public override void Update()
        {
            owner.dir = (owner.patrolPoints[owner.patrolIndex].position - owner.transform.position).normalized;
            owner.transform.Translate(owner.dir * moveSpeed * Time.deltaTime);

            if (Vector2.Distance(owner.transform.position, owner.patrolPoints[owner.patrolIndex].position) < 0.02f)
            {
                owner.patrolIndex = (owner.patrolIndex + 1) % owner.patrolPoints.Length;
                owner.ChangeState(State.Idle);
            }
            else if (Vector2.Distance(player.position, owner.transform.position) < detectRange)
            {
                owner.ChangeState(State.Trace);
            }
        }

        public override void Exit()
        {

        }
    }
}