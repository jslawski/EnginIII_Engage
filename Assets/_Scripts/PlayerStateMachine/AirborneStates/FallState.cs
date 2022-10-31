using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : AirborneState
{
    public override void Enter(PlayerController controller)
    {
        base.Enter(controller);

        this.controller.playerRb.velocity = Vector3.zero;
        this.controller.characterAnimator.SetBool("Fall", true);

        this.controller.audioSource.clip = Resources.Load<AudioClip>("Audio/fall");
        this.controller.audioSource.Play();
    }

    public override void Exit()
    {
        if (this.distanceToGround > 0.0f)
        {
            Vector3 correctionPosition = this.controller.playerRb.position + (Vector3.down * this.distanceToGround);
            this.controller.playerRb.MovePosition(correctionPosition);
        }

        this.controller.characterAnimator.SetBool("Fall", false);

        base.Exit();        
    }

    public override void UpdateState()
    {        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.controller.ChangeState(new GlideState());
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