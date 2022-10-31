using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : PlayerState
{
    private float timeToRespawn = 2.0f;

    private Vector3 enterVelocity = Vector3.zero;
    private float deathMagnitude = 3.0f;

    public override void Enter(PlayerController controller)
    {
        base.Enter(controller);

        this.enterVelocity = this.controller.playerRb.velocity.normalized;
        this.controller.characterAnimator.SetBool("Die", true);

        this.controller.playerRb.velocity = Vector3.zero;
        this.controller.playerRb.useGravity = false;
    }

    public override void Exit()
    {
        base.Exit();

        this.controller.characterAnimator.SetBool("Die", false);
    }

    public override void UpdateState()
    {
        //Nothing should happen here (not even base functionality)
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (this.timeToRespawn > 0.0f)
        {
            this.timeToRespawn -= Time.fixedDeltaTime;

            Vector3 moveVector = this.enterVelocity * this.deathMagnitude * Time.fixedDeltaTime;
            Vector3 targetDest = this.controller.playerRb.position + moveVector;
            this.controller.playerRb.MovePosition(targetDest);
        }
        else
        {
            this.controller.Respawn();
        }
    }
}
