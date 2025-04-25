using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerMovement playerMovement;
    Rigidbody2D rb;

    private int speedHash;
    private int isCrouchingHash;
    private int isHangingHash;
    private int isJumpingHash;
    private int isOnGroundHash;
    private int verticalVelocityHash;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement  = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();

        speedHash = Animator.StringToHash("speed");
        isCrouchingHash = Animator.StringToHash("isCrouching");
        isHangingHash = Animator.StringToHash("isHanging");
        isJumpingHash = Animator.StringToHash("isJumping");
        isOnGroundHash = Animator.StringToHash("isOnGround");
        verticalVelocityHash = Animator.StringToHash("verticalVelocity");
    }

    private void Update()
    {
        anim.SetFloat(speedHash, Mathf.Abs(playerMovement.xVelocity));
        anim.SetBool(isCrouchingHash, playerMovement.isCrouch);
        anim.SetBool(isHangingHash, playerMovement.isHanging);
        anim.SetBool(isJumpingHash, playerMovement.isJump);
        anim.SetBool(isOnGroundHash, playerMovement.isOnGround);
        anim.SetFloat(verticalVelocityHash, rb.velocity.y);
    }

    public void StepAudio()
    {
        AudioManager.PlayFootstepAudio();
    }

    public void CrouchStepAudio()
    {
        AudioManager.PlayCrouchFootstepAudio();
    }

    //放在这里会有重复播放的问题，已移至PlayerMovement.cs
    //public void JumpAudio()
    //{
    //    AudioManager.PlayJumpAudio();
    //}
}
