using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : PlayerState
{
    protected float moveSpeed = 8f;
    protected Vector3 moveDirection = Vector3.zero;

    protected float maxFallSpeed = 10f;

    public override void Enter(PlayerCharacter character)
    {        
        character.playerRb.useGravity = true;
    }

    public override void Exit(PlayerCharacter character)
    {
        if (this.nearestWallLatchPoint != Vector3.zero)
        {
            character.playerRb.MovePosition(this.nearestWallLatchPoint);
        }
    }

    public override void HandleInput(PlayerCharacter character)
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
    }

    public override void UpdateState(PlayerCharacter character)
    {
        if (Mathf.Abs(character.playerRb.velocity.y) > this.maxFallSpeed)
        {
            character.playerRb.velocity = new Vector3(character.playerRb.velocity.x, -this.maxFallSpeed, 0.0f);
        }

        Vector3 moveVector = this.moveDirection * this.moveSpeed * Time.fixedDeltaTime;

        if (this.IsNearWall(character, moveVector) == true)
        {
            this.ChangeState(character, new WallSlideState());
            return;
        }

        Vector3 targetPosition = character.playerRb.position + moveVector;
        character.playerRb.MovePosition(targetPosition);          
    }
}
