using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [SerializeField] private float detectRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform[] patrolPoints;

    public enum State { Idle, Trace, Return, Attack, Patrol }

    private State curState;
    private Transform player;
    private Vector3 returnPosition;
    private int patrolIndex = 0;
    private Vector2 dir;
    private SpriteRenderer render;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        curState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        returnPosition = transform.position;
    }

    private void Update()
    {
        switch(curState)
        {
            case State.Idle:
                IdleUpdate();
                break;
            case State.Trace:
                TraceUpdate();
                break;
            case State.Return:
                ReturnUpdate();
                break;
            case State.Attack:
                AttackUpdate();
                break;
            case State.Patrol:
                PatrolUpdate();
                break;
        }
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

    float idleTime = 0;

    private void IdleUpdate()
    {
        // �ƹ��͵� ���ϱ�
        if (idleTime > 2)
        {
            idleTime = 0;
            curState = State.Patrol;
        }
        idleTime += Time.deltaTime;


        // �÷��̾ ��������� ��
        if (Vector2.Distance(player.position, transform.position) < detectRange)
        {
            curState = State.Trace;
        }
    }

    private void TraceUpdate()
    {
        // �÷��̾� �i�ư���
        dir = (player.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        // �÷��̾ �־����� ��
        if (Vector2.Distance(player.position, transform.position) > detectRange)
        {
            curState = State.Return;
        }
        // ���ݹ��� �ȿ� ���� ��
        else if (Vector2.Distance(player.position, transform.position) < attackRange)
        {
            curState = State.Attack;
        }
    }

    private void ReturnUpdate()
    {
        // ���� �ڸ��� ���ư���
        dir = (returnPosition - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        // ���� �ڸ��� ����������
        if (Vector2.Distance(transform.position, returnPosition) < 0.02f)
        {
            curState = State.Idle;
        }
        // ���ư��� �߿� �ٽ� �÷��̾ �����Ǹ�
        else if (Vector2.Distance(player.position, transform.position) < detectRange)
        {
            curState = State.Trace;
        }
    }

    float lastAttackTime = 0;

    private void AttackUpdate()
    {
        // �����ϱ�
        if (lastAttackTime > 1)
        {
            Debug.Log("����");
            lastAttackTime = 0;
        }  
        lastAttackTime += Time.deltaTime;

        if (Vector2.Distance(player.position, transform.position) > attackRange)
        {
            curState = State.Trace;
        }
    }

    private void PatrolUpdate()
    {
        // ���� ����
        dir = (patrolPoints[patrolIndex].position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, patrolPoints[patrolIndex].position) < 0.02f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            curState = State.Idle;
        }
        else if(Vector2.Distance(player.position, transform.position) < detectRange)
        {
            curState = State.Trace;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
