using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected float distanceToGround = 0.0f;

    protected Vector3 nearestWallLatchPoint = Vector3.zero;

    public virtual void Enter(PlayerCharacter character) { }

    public virtual void Exit(PlayerCharacter character) { }

    public virtual void ChangeState(PlayerCharacter character, PlayerState newState)
    {
        //Debug.LogError(newState);

        this.Exit(character);
        character.currentState = newState;
        newState.Enter(character);
    }

    public virtual void HandleInput(PlayerCharacter character) { }
    public virtual void UpdateState(PlayerCharacter character) { }

    public virtual bool IsGrounded(PlayerCharacter character)
    {
        Vector3 leftOrigin = character.playerRb.position - new Vector3(character.playerCollider.bounds.extents.x * 0.9f, 0.0f, 0.0f);
        Vector3 rightOrigin = character.playerRb.position + new Vector3(character.playerCollider.bounds.extents.x * 0.9f, 0.0f, 0.0f);

        float raycastMagnitude = character.playerCollider.bounds.extents.y + (Mathf.Abs(character.playerRb.velocity.y) * Time.fixedDeltaTime);

        RaycastHit hitInfoLeft;
        RaycastHit hitInfoRight;

        bool leftHit = Physics.Raycast(leftOrigin, Vector3.down, out hitInfoLeft, raycastMagnitude, character.groundLayer);
        bool rightHit = Physics.Raycast(rightOrigin, Vector3.down, out hitInfoRight, raycastMagnitude, character.groundLayer);        

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

    public bool IsNearWall(PlayerCharacter character, Vector3 horizontalDirection)
    {
        Vector3 topOrigin = character.playerRb.position + new Vector3(0.0f, character.playerCollider.bounds.extents.y * 0.75f, 0.0f);
        Vector3 bottomOrigin = character.playerRb.position - new Vector3(0.0f, character.playerCollider.bounds.extents.y * 0.75f, 0.0f);

        float raycastMagnitude = character.playerCollider.bounds.extents.x + Mathf.Abs(horizontalDirection.x);

        RaycastHit hitInfoTop;
        RaycastHit hitInfoBottom;

        bool topHit = Physics.Raycast(topOrigin, horizontalDirection.normalized, out hitInfoTop, raycastMagnitude, character.groundLayer);
        bool bottomHit = Physics.Raycast(bottomOrigin, horizontalDirection.normalized, out hitInfoBottom, raycastMagnitude, character.groundLayer);

        float wallSeparationDistance = 0.1f + character.playerCollider.bounds.extents.x;        

        if (topHit == true || bottomHit == true)
        {
            float xLatchPosition = hitInfoTop.point.x - (wallSeparationDistance * Mathf.Sign(horizontalDirection.x));
            this.nearestWallLatchPoint = new Vector3(xLatchPosition, character.playerRb.position.y, 0.0f);
        }
        if (bottomHit == true)
        {
            float xLatchPosition = hitInfoBottom.point.x - (wallSeparationDistance * Mathf.Sign(horizontalDirection.x));
            this.nearestWallLatchPoint = new Vector3(xLatchPosition, character.playerRb.position.y, 0.0f);
        }

        return (topHit || bottomHit);
    }
}
