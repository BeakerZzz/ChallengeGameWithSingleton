using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("移动参数")]
    public float moveSpeed = 8f;
    public float crouchSpeedDivisor = 3f;

    [Header("跳跃参数")]
    public float jumpForce = 6.3f;
    public float jumpHoldForce = 1.9f;
    public float jumpHoldDuration = 0.1f;
    public float crouchJumpBoost = 2.5f;
    public float hangingJumpForce = 15f;


    private  float jumpTime;

    [Header("状态")]
    public bool isCrouch;
    public bool isOnGround;
    public bool isJump;
    public bool isHeadBlocked;
    public bool isHanging;

    [Header("环境检测")]
    //检测地面
    public LayerMask groundLayer;
    //跳跃用检测
    public float footOffset = 0.4f;
    public float groundDistance = 0.2f;
    //起立用检测
    public float headClearance = 0.5f;
    //悬挂用检测
    private float playerHeight;
    public float eyeHeight = 1.5f;
    public float grabDistance = 0.4f;
    public float reachOffset = 0.7f;

    public float xVelocity;

    //按键设置
    private bool jumpPressed;
    private bool jumpHeld;
    private bool crouchPressed;
    private bool crouchHeld;

    //碰撞体尺寸
    Vector2 colliderStandSize;
    Vector2 colliderStandOffset;
    Vector2 colliderCrouchSize;
    Vector2 colliderCrouchOffset;


    //生命周期
    InputLifecycleManager inputLifecycleManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        playerHeight = coll.size.y;
        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        colliderCrouchSize = new Vector2(coll.size.x, coll.size.y / 2f);
        colliderCrouchOffset = new Vector2(coll.offset.x, coll.offset.y / 2f);

        inputLifecycleManager = new InputLifecycleManager();
        inputLifecycleManager.AddInputLifecycle("Jump");
        inputLifecycleManager.AddInputLifecycle("Crouch");
    }

    private void Update()
    {
        jumpPressed = inputLifecycleManager.GetPressed("Jump");
        crouchPressed = inputLifecycleManager.GetPressed("Crouch");
        jumpHeld = Input.GetButton("Jump");
        crouchHeld = Input.GetButton("Crouch");
        
    }

    private void FixedUpdate()
    {
        PhysicsCheck();
        GroundMovement();
        MidAirMovement();
    }

    private void PhysicsCheck()
    {

        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);

        if (leftCheck || rightCheck) 
            isOnGround = true;
        else
            isOnGround = false;

        RaycastHit2D headCheck = Raycast(new Vector2(0f,coll.size.y), Vector2.up, headClearance, groundLayer);

        if(headCheck)
            isHeadBlocked = true;
        else
            isHeadBlocked = false;

        float direction = transform.localScale.x;
        Vector2 grabDir = new Vector2(direction, 0f);
        RaycastHit2D blockedCheck = Raycast(new Vector2(footOffset * direction, playerHeight), grabDir, grabDistance, groundLayer);
        RaycastHit2D wallCheck = Raycast(new Vector2(footOffset * direction, eyeHeight), grabDir, grabDistance, groundLayer);
        RaycastHit2D ledgeChack = Raycast(new Vector2(reachOffset * direction, playerHeight), Vector2.down, grabDistance, groundLayer);

        if(!isOnGround && rb.velocity.y < 0f && ledgeChack && wallCheck && !blockedCheck)
        {
            Vector2 pos = transform.position;
            pos.x += (wallCheck.distance -0.05f)* direction;
            pos.y -= ledgeChack.distance;
            transform.position = pos;
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }


    private void GroundMovement()
    {
        if (isHanging)
            return;

        if (crouchHeld && !isCrouch && isOnGround)
        {
            Crouch();
        }
        else if (!crouchHeld && isCrouch && !isHeadBlocked)
        {
            StandUp();
        }
        else if (!isOnGround && isCrouch && !isHeadBlocked)
        {
            StandUp();
        }

        xVelocity = Input.GetAxis("Horizontal") * moveSpeed;

        if (isCrouch)
        {
            xVelocity /= crouchSpeedDivisor;
        }

        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
        FilpDirection();
    }

    private void MidAirMovement()
    {
        

        if (jumpPressed && isOnGround && !isJump && !isHanging) 
        {
            if(isCrouch && isOnGround && !isHeadBlocked)
            {
                StandUp();
                rb.AddForce(new Vector2(0f, crouchJumpBoost), ForceMode2D.Impulse);
            }
            isOnGround = false;
            isJump = true;

            jumpTime = Time.time + jumpHoldDuration;

            rb.AddForce(new Vector2(0f,jumpForce), ForceMode2D.Impulse);

            AudioManager.PlayJumpAudio();
        }
        else if (isJump && !isHanging)
        {
            if(jumpHeld)
            {
                rb.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Impulse);
            }
            if(jumpTime < Time.time)
            {
                isJump = false;
            }
        }
        
        if (isHanging)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.AddForce(new Vector2(0f, hangingJumpForce), ForceMode2D.Impulse);
                isHanging = false;

                AudioManager.PlayJumpAudio();
            }
            if (crouchPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
            }

        }
    }

    //private bool JumpPressedLifeTime(String ButtonName)//按下按键持续时间
    //{

    //    if (Input.GetButtonDown(ButtonName))
    //        jumpRequestLifetime = 0.2f;
    //    else if (Input.GetButtonUp(ButtonName))
    //        jumpRequestLifetime = 0f;
    //    else
    //        jumpRequestLifetime = Mathf.Max(0f, jumpRequestLifetime - Time.deltaTime);
    //    return jumpRequestLifetime > 0;
    //}
    //private bool CrouchPressedLifeTime(String ButtonName)//按下按键持续时间
    //{

    //    if (Input.GetButtonDown(ButtonName))
    //        crouchRequestLifetime = 0.2f;
    //    else if (Input.GetButtonUp(ButtonName))
    //        crouchRequestLifetime = 0f;
    //    else
    //        crouchRequestLifetime = Mathf.Max(0f, crouchRequestLifetime - Time.deltaTime);
    //    return crouchRequestLifetime > 0;
    //}


    private void FilpDirection()//移动时左右翻转
    {
        if (xVelocity < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (xVelocity > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void Crouch()
    {
        isCrouch = true;
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }

    private void StandUp()
    {
        isCrouch = false;
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * length, color);
        return hit;
    }
}
