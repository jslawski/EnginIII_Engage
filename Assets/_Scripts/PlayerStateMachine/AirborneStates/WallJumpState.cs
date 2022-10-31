using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : PlayerState
{
    Vector3 jumpDirection;
    float jumpSpeed = 20f;
    float jumpDuration = 0.2f;

    public override void Enter(PlayerController controller)
    {
        base.Enter(controller);

        this.jumpDirection = Vector3.up * 1.5f;

        if (this.IsNearWall(Vector3.left) == true)
        {
            this.jumpDirection += Vector3.right;
            this.controller.characterAnimator.SetBool("WallJumpRight", true);
        }
        else
        {
            this.jumpDirection += Vector3.left;
            this.controller.characterAnimator.SetBool("WallJumpLeft", true);
        }

        this.controller.audioSource.clip = Resources.Load<AudioClip>("Audio/wallJump");
        this.controller.audioSource.Play();

        this.controller.playerRb.useGravity = false;
    }

    public override void Exit()
    {
        base.Exit();

        this.controller.characterAnimator.SetBool("WallJumpLeft", false);
        this.controller.characterAnimator.SetBool("WallJumpRight", false);
    }

    public override void FixedUpdateState()
    {
        Vector3 moveVector = (this.jumpDirection.normalized * this.jumpSpeed * Time.fixedDeltaTime);

        if (this.IsNearWall(new Vector3(moveVector.x, 0.0f, 0.0f)) == true)
        {
            this.controller.ChangeState(new WallSlideState());
            return;
        }

        if (jumpDuration > 0.0f)
        {
            Vector3 targetPosition = this.controller.playerRb.position + moveVector;
            this.controller.playerRb.MovePosition(targetPosition);
            this.jumpDuration -= Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            this.controller.ChangeState(new GlideState());
        }
        else
        {
            this.controller.ChangeState(new FallState());
        }
    }
}
