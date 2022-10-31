using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideState : PlayerState
{
    protected enum WallSide { Left, Right, None }
    protected WallSide latchedWall = WallSide.None;

    protected float maxFallSpeed = 2.0f;

    public override void Enter(PlayerController controller)
    {
        base.Enter(controller);

        if (this.IsNearWall(Vector3.left) == true)
        {
            this.latchedWall = WallSide.Left;
            this.controller.characterAnimator.SetBool("SlideLeft", true);
        }
        else
        {
            this.latchedWall = WallSide.Right;
            this.controller.characterAnimator.SetBool("SlideRight", true);
        }

        this.controller.audioSource.clip = Resources.Load<AudioClip>("Audio/wallSlide");
        this.controller.audioSource.Play();
    }

    public override void Exit()
    {
        base.Exit();

        if (this.distanceToGround > 0.0f)
        {
            Vector3 correctionPosition = this.controller.playerRb.position + (Vector3.down * this.distanceToGround);
            this.controller.playerRb.MovePosition(correctionPosition);
        }

        this.controller.characterAnimator.SetBool("SlideRight", false);
        this.controller.characterAnimator.SetBool("SlideLeft", false);
    }

    public override void UpdateState()
    {        
        if (this.latchedWall == WallSide.Left)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                this.controller.ChangeState(new FallState());
            }
        }
        else if (this.latchedWall == WallSide.Right)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                this.controller.ChangeState(new FallState());
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.controller.ChangeState(new WallJumpState());
        }

        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (Mathf.Abs(this.controller.playerRb.velocity.y) > this.maxFallSpeed)
        {
            this.controller.playerRb.velocity = new Vector3(this.controller.playerRb.velocity.x, -this.maxFallSpeed, 0.0f);
        }

        if (this.IsGrounded() == true)
        {
            this.controller.ChangeState(new IdleState());

            this.controller.audioSource.clip = Resources.Load<AudioClip>("Audio/land");
            this.controller.audioSource.Play();

            return;
        }
    }
}