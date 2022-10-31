using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterState : PlayerState
{
    private float moveSpeed = 5f;
    private Vector3 moveDirection = Vector3.zero;

    private float underwaterDrag = 3.0f;
    private float underwaterAngularDrag = 1.0f;
    private float airDrag = 0.0f;
    private float airAngularDrag = 0.05f;

    public override void Enter(PlayerController controller)
    {
        base.Enter(controller);

        this.controller.playerRb.drag = this.underwaterDrag;
        this.controller.playerRb.angularDrag = this.underwaterAngularDrag;        
    }

    public override void Exit()
    {
        base.Exit();

        this.controller.playerRb.drag = this.airDrag;
        this.controller.playerRb.angularDrag = this.airAngularDrag;
    }

    public override void UpdateState()
    {        
        this.moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.moveDirection += Vector3.right;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.moveDirection += Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.controller.ChangeState(new JumpState());
        }

        if (this.controller.waterCollider == null)
        {
            this.controller.ChangeState(new FallState());
        }

        base.UpdateState();
    }

    public override void FixedUpdateState()
    {
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

        this.ApplyBuoyancyForce();

        base.FixedUpdateState();
    }

    private float GetWaterHeight()
    {
        if (this.controller.waterCollider != null)
        {
            return this.controller.waterCollider.bounds.max.y;
        }

        return this.controller.playerRb.position.y;
        
    }

    private void ApplyBuoyancyForce()
    {
        float yDiff = this.controller.playerRb.position.y - this.GetWaterHeight();

        if (yDiff < 0.0f)
        {
            this.controller.playerRb.AddForce(Vector3.up * this.controller.floatMagnitude * Mathf.Abs(yDiff), ForceMode.Force);
        }
    }
}