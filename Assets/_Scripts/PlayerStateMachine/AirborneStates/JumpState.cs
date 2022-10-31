using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AirborneState
{
    private float initialJumpForce = 8f;
    private float residualJumpForce = 30f;
    private float residualJumpTime = 0.5f;

    private bool stillJumping = true;

    public override void Enter(PlayerController controller)
    {
        base.Enter(controller);

        this.controller.playerRb.velocity = new Vector3(this.controller.playerRb.velocity.x, 0.0f, 0.0f);

        this.controller.characterAnimator.SetBool("Jump", true);
        this.controller.playerRb.AddForce(Vector3.up * this.initialJumpForce, ForceMode.Impulse);

        this.controller.audioSource.clip = Resources.Load<AudioClip>("Audio/jump");
        this.controller.audioSource.Play();
    }

    public override void Exit()
    {
        base.Exit();

        this.controller.characterAnimator.SetBool("Jump", false);
    }

    public override void UpdateState()
    {        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.controller.ChangeState(new FallState());
        }
        if (Input.GetKey(KeyCode.Space) && this.residualJumpTime <= 0.0f)
        {
            if (this.controller.skills.HasSkill("GlideState"))
            {
                this.controller.ChangeState(new GlideState());
            }
            else
            {
                this.controller.ChangeState(new FallState());
            }
        }

        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
        if (this.stillJumping == true && this.residualJumpTime > 0.0f)
        {
            this.controller.playerRb.AddForce(Vector3.up * this.residualJumpForce, ForceMode.Force);
            this.residualJumpTime -= Time.fixedDeltaTime;
        }

        base.FixedUpdateState();
    }
}
