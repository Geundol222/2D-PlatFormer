using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeState0524;
using System.Buffers;
using UnityEngine.Assertions.Must;

public class Bee0524 : MonoBehaviour
{
    StateMachine0524<State, Bee0524> stateMachine;

    [SerializeField] public FireBall fireBall;

    public Transform[] patrolPoints;
    public int patrolIndex = 0;
    public Vector2 dir;
    public SpriteRenderer render;

    public float detectRange;
    public float attackRange;
    public float moveSpeed;
    public Transform player;
    public Vector3 returnPosition;

    public State curState;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();

        stateMachine = new StateMachine0524<State, Bee0524>(this);
        stateMachine.AddState(State.Idle, new IdleState(this, stateMachine));
        stateMachine.AddState(State.Runaway, new RunawayState(this, stateMachine));
        stateMachine.AddState(State.Return, new ReturnState(this, stateMachine));
        stateMachine.AddState(State.Attack, new AttackState(this, stateMachine));
        stateMachine.AddState(State.Patrol, new PatrolState(this, stateMachine));
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
        stateMachine.Setup(State.Idle);
    }

    private void Update()
    {
        stateMachine.Update();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

namespace BeeState0524
{
    public enum State { Idle, Runaway, Return, Attack, Patrol, Size }

    public abstract class BeeState0524 : StateBase0524<State, Bee0524>
    {
        protected GameObject gameObject => owner.gameObject;
        protected FireBall fireBall => owner.fireBall;
        protected Transform transform => owner.transform;
        protected float detectRange => owner.detectRange;
        protected float attackRange => owner.attackRange;
        protected float moveSpeed => owner.moveSpeed;
        protected Transform player => owner.player;
        protected Vector3 returnPosition => owner.returnPosition;
        protected Vector2 dir
        {
            get
            {
                return owner.dir;
            }
            set
            {
                owner.dir = value;
            }
        }
        protected Transform[] patrolPoints
        {
            get
            {
                return owner.patrolPoints;
            }
            set
            {
                owner.patrolPoints = value;
            }
        }
        protected int patrolIndex
        {
            get
            {
                return owner.patrolIndex;
            }
            set
            {
                owner.patrolIndex = value;
            }
        }

        public BeeState0524(Bee0524 owner, StateMachine0524<State, Bee0524> stateMachine) : base(owner, stateMachine) { }
    }

    public class IdleState : BeeState0524
    {
        float idleTime;

        public IdleState(Bee0524 owner, StateMachine0524<State, Bee0524> stateMachine) : base(owner, stateMachine) { }

        public override void Setup()
        {
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
                stateMachine.ChangeState(State.Patrol);
            }

            if (Vector2.Distance(player.position, transform.position) < attackRange
                && Vector2.Distance(player.position, transform.position) > detectRange)
            {
                stateMachine.ChangeState(State.Attack);
            }
        }

        public override void Exit()
        {
            
        }
    }

    public class RunawayState : BeeState0524
    {
        public RunawayState(Bee0524 owner, StateMachine0524<State, Bee0524> stateMachine) : base(owner, stateMachine) { }

        public override void Setup()
        {
            
        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            dir = (player.position - transform.position).normalized;
            transform.Translate(-dir * moveSpeed * Time.deltaTime);

            if (Vector2.Distance(player.position, transform.position) > detectRange)
            {
                stateMachine.ChangeState(State.Return);
            }
            else if (Vector2.Distance(player.position, transform.position) < attackRange
                && Vector2.Distance(player.position, transform.position) > detectRange)
            {
                stateMachine.ChangeState(State.Attack);
            }
        }

        public override void Exit()
        {

        }
    }

    public class ReturnState : BeeState0524
    {
        public ReturnState(Bee0524 owner, StateMachine0524<State, Bee0524> stateMachine) : base(owner, stateMachine) { }

        public override void Setup()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            dir = (returnPosition - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime);

            if (Vector2.Distance(returnPosition, transform.position) < 0.02f)
            {
                stateMachine.ChangeState(State.Idle);
            }
            else if (Vector2.Distance(player.position, transform.position) < attackRange
                && Vector2.Distance(player.position, transform.position) > detectRange)
            {
                stateMachine.ChangeState(State.Attack);
            }
        }

        public override void Exit()
        {

        }
    }

    public class AttackState : BeeState0524
    {
        float lastAttackTime;

        public AttackState(Bee0524 owner, StateMachine0524<State, Bee0524> stateMachine) : base(owner, stateMachine) { }

        public override void Setup()
        {

        }

        public override void Enter()
        {
            lastAttackTime = 0;
        }

        public override void Update()
        {
            dir = (player.position - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime);

            if (lastAttackTime > 1.5f)
            {
                FireBall.Instantiate(fireBall, transform.position, transform.rotation);
                lastAttackTime = 0;
            }
            lastAttackTime += Time.deltaTime;

            if (Vector2.Distance(player.position, transform.position) > attackRange)
            {
                stateMachine.ChangeState(State.Return);
            }
            else if (Vector2.Distance(player.position, transform.position) < detectRange)
            {
                stateMachine.ChangeState(State.Runaway);
            }
        }

        public override void Exit()
        {

        }
    }

    public class PatrolState : BeeState0524
{
        public PatrolState(Bee0524 owner, StateMachine0524<State, Bee0524> stateMachine) : base(owner, stateMachine) { }

        public override void Setup()
        {

        }

        public override void Enter()
        {

        }

        public override void Update()
        {
            dir = (patrolPoints[patrolIndex].position - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime);

            if (Vector2.Distance(patrolPoints[patrolIndex].position, transform.position) < 0.02f)
            {
                stateMachine.ChangeState(State.Idle);
            }
            else if (Vector2.Distance(player.position, transform.position) < attackRange
                && Vector2.Distance(player.position, transform.position) > detectRange)
            {
                stateMachine.ChangeState(State.Attack);
            }
        }

        public override void Exit()
        {

        }
    }
}
