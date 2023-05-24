using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] LayerMask groundMask;

    private Rigidbody2D rb;
    private Animator anim;

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
        if (!IsGroundExist())
        {
            Turn();
        }
    }

    public void Move()
    {
        rb.velocity = new Vector2(transform.right.x * -moveSpeed, rb.velocity.y);
    }

    public void Turn()
    {
        transform.Rotate(Vector3.up, 180);
    }

    private bool IsGroundExist()
    {
        Debug.DrawRay(groundCheckPoint.position, Vector2.down, Color.red);

        return Physics2D.Raycast(groundCheckPoint.position, Vector2.down, 1f, groundMask);
    }
}
