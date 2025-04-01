using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("ÒÆ¶¯²ÎÊý")]
    public float moveSpeed = 8f;
    public float crouchSpeedDivisor = 3f;

    private float xVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        GroundMovement();
        FilpDirection();
    }

    private void GroundMovement()
    {
        xVelocity = Input.GetAxis("Horizontal") * moveSpeed;
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    private void FilpDirection()
    {
        if (xVelocity < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (xVelocity > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
    }
}
