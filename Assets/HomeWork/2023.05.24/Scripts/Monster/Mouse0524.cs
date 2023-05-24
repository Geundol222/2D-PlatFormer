
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse0524 : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform groundChecker;
    [SerializeField] LayerMask groundMask;

    Rigidbody2D rb;
    Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        if (!IsGround())
            Turn();
    }

    public void Move()
    {
        rb.velocity = new Vector2(transform.right.x * -moveSpeed, rb.velocity.y);
    }

    public void Turn()
    {
        transform.Rotate(Vector3.up, 180);
    }

    private bool IsGround()
    {
        Debug.DrawRay(groundChecker.position, Vector2.down, Color.red);
        return Physics2D.Raycast(groundChecker.position, Vector2.down, 1f, groundMask);
    }
}
