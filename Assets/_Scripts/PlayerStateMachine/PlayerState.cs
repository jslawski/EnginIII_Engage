using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerController controller;

    protected float distanceToGround = 0.0f;
    protected Vector3 nearestWallLatchPoint = Vector3.zero;

    public virtual void Enter(PlayerController controller)
    {
        this.controller = controller;
    }

    public virtual void Exit() { }

    public virtual void UpdateState()
    {
        if (this.controller.waterCollider != null)
        {
            if (this.controller.skills.HasSkill("UnderwaterState") == true)
            {
                if (this.controller.currentState.GetType() != typeof(UnderwaterState) &&
                this.controller.currentState.GetType() != typeof(JumpState))
                {
                    this.controller.ChangeState(new UnderwaterState());
                }
            }
            else
            {
                this.controller.ChangeState(new DieState());
            }            
        }

        
    }

    public virtual void FixedUpdateState() { }

    protected bool IsGrounded()
    {
        Vector3 leftOrigin = this.controller.playerRb.position - new Vector3(this.controller.playerCollider.bounds.extents.x * 0.8f, 0.0f, 0.0f);
        Vector3 rightOrigin = this.controller.playerRb.position + new Vector3(this.controller.playerCollider.bounds.extents.x * 0.8f, 0.0f, 0.0f);

        float raycastMagnitude = this.controller.playerCollider.bounds.extents.y + 0.01f;//(Mathf.Abs(this.controller.playerRb.velocity.y) * Time.fixedDeltaTime);

        RaycastHit hitInfoLeft;
        RaycastHit hitInfoRight;

        bool leftHit = Physics.Raycast(leftOrigin, Vector3.down, out hitInfoLeft, raycastMagnitude, this.controller.groundLayer);
        bool rightHit = Physics.Raycast(rightOrigin, Vector3.down, out hitInfoRight, raycastMagnitude, this.controller.groundLayer);

        if (leftHit == true)
        {
            this.distanceToGround = hitInfoLeft.distance;
        }
        else if (rightHit == true)
        {
            this.distanceToGround = hitInfoRight.distance;
        }

        return (leftHit || rightHit);
    }

    public bool IsNearWall(Vector3 horizontalDirection)
    {
        Vector3 topOrigin = this.controller.playerRb.position + new Vector3(0.0f, this.controller.playerCollider.bounds.extents.y * 0.75f, 0.0f);
        Vector3 bottomOrigin = this.controller.playerRb.position - new Vector3(0.0f, this.controller.playerCollider.bounds.extents.y * 0.75f, 0.0f);

        float raycastMagnitude = this.controller.playerCollider.bounds.extents.x + Mathf.Abs(horizontalDirection.x);

        RaycastHit hitInfoTop;
        RaycastHit hitInfoBottom;

        bool topHit = Physics.Raycast(topOrigin, horizontalDirection.normalized, out hitInfoTop, raycastMagnitude, this.controller.groundLayer);
        bool bottomHit = Physics.Raycast(bottomOrigin, horizontalDirection.normalized, out hitInfoBottom, raycastMagnitude, this.controller.groundLayer);

        float wallSeparationDistance = 0.1f + this.controller.playerCollider.bounds.extents.x;

        if (topHit == true || bottomHit == true)
        {
            float xLatchPosition = hitInfoTop.point.x - (wallSeparationDistance * Mathf.Sign(horizontalDirection.x));
            this.nearestWallLatchPoint = new Vector3(xLatchPosition, this.controller.playerRb.position.y, 0.0f);
        }
        if (bottomHit == true)
        {
            float xLatchPosition = hitInfoBottom.point.x - (wallSeparationDistance * Mathf.Sign(horizontalDirection.x));
            this.nearestWallLatchPoint = new Vector3(xLatchPosition, this.controller.playerRb.position.y, 0.0f);
        }

        return (topHit || bottomHit);
    }
}
