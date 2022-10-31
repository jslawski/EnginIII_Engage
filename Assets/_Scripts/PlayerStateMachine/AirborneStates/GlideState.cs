using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideState : AirborneState
{
    public override void Enter(PlayerController controller)
    {
        base.Enter(controller);

        this.controller.characterAnimator.SetBool("Glide", true);
        this.moveSpeed = 5f;
        this.maxFallSpeed = 0.5f;

        this.controller.audioSource.clip = Resources.Load<AudioClip>("Audio/glide");
        this.controller.audioSource.Play();
    }

    public override void Exit()
    {
        if (this.distanceToGround > 0.0f)
        {
            Vector3 correctionPosition = this.controller.playerRb.position + (Vector3.down * this.distanceToGround);
            this.controller.playerRb.MovePosition(correctionPosition);
        }

        this.controller.characterAnimator.SetBool("Glide", false);

        base.Exit();
    }

    public override void UpdateState()
    {       
        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.controller.ChangeState(new FallState());
        }

        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
        if (this.IsGrounded() == true)
        {
            this.controller.ChangeState(new IdleState());

            this.controller.audioSource.clip = Resources.Load<AudioClip>("Audio/land");
            this.controller.audioSource.Play();

            return;
        }

        base.FixedUpdateState();
    }
}
