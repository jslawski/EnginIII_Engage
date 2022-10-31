using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerState
{
    private float moveSpeed = 8f;
    private Vector3 moveDirection = Vector3.zero;

    public override void Enter(PlayerController controller)
    {
        base.Enter(controller);

        this.controller.audioSource.clip = Resources.Load<AudioClip>("Audio/run");
    }

    public override void Exit()
    {
        base.Exit();

        this.controller.characterAnimator.SetBool("RunRight", false);
        this.controller.characterAnimator.SetBool("RunLeft", false);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        this.moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.moveDirection += Vector3.right;

            this.controller.characterAnimator.SetBool("RunRight", true);
            this.controller.characterAnimator.SetBool("RunLeft", false);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.moveDirection += Vector3.left;

            this.controller.characterAnimator.SetBool("RunRight", false);
            this.controller.characterAnimator.SetBool("RunLeft", true);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.controller.ChangeState(new JumpState());
        }

        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow) &&
            !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow) &&
            !Input.GetKey(KeyCode.Space))
        {
            this.controller.ChangeState(new IdleState());
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (this.IsGrounded() == false)
        {
            this.controller.ChangeState(new FallState());
            return;
        }

        Vector3 moveVector = this.moveDirection * this.moveSpeed * Time.fixedDeltaTime;

        if (this.IsNearWall(moveVector) == false)
        {
            Vector3 targetPosition = this.controller.playerRb.position + moveVector;
            this.controller.playerRb.MovePosition(targetPosition);

            if (this.controller.audioSource.isPlaying == false)
            {
                this.controller.audioSource.Play();
            }
        }
    }
}