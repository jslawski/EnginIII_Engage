using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public override void Enter(PlayerController controller)
    {
        base.Enter(controller);

        this.controller.characterAnimator.SetBool("Idle", true);

        this.controller.playerRb.useGravity = false;
        this.controller.playerRb.velocity = Vector3.zero;
    }

    public override void Exit()
    {
        base.Exit();

        this.controller.characterAnimator.SetBool("Idle", false);
    }

    public override void UpdateState()
    {        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.controller.ChangeState(new MoveState());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.controller.ChangeState(new JumpState());
        }

        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }
}
