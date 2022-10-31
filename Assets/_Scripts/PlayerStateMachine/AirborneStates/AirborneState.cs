using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : PlayerState
{
    protected float moveSpeed = 8f;
    protected Vector3 moveDirection = Vector3.zero;

    protected float maxFallSpeed = 10f;

    public override void Enter(PlayerController controller)
    {
        base.Enter(controller);

        this.controller.playerRb.useGravity = true;
    }

    public override void Exit()
    {
        base.Exit();

        if (IsNearWall(this.moveDirection))
        {
            this.controller.playerRb.MovePosition(this.nearestWallLatchPoint);
        }
    }

    public override void UpdateState()
    {
        base.UpdateState();

        this.moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.moveDirection += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.moveDirection += Vector3.left;
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (Mathf.Abs(this.controller.playerRb.velocity.y) > this.maxFallSpeed)
        {
            this.controller.playerRb.velocity = new Vector3(this.controller.playerRb.velocity.x, -this.maxFallSpeed, 0.0f);
        }

        Vector3 moveVector = this.moveDirection * this.moveSpeed * Time.fixedDeltaTime;

        if (this.IsNearWall(moveVector) == true)
        {
            this.controller.ChangeState(new WallSlideState());            
            return;
        }

        Vector3 targetPosition = this.controller.playerRb.position + moveVector;
        this.controller.playerRb.MovePosition(targetPosition);
    }
}
