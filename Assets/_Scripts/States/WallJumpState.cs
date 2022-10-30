using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : PlayerState
{
    private enum WallSide { Left, Right, None }

    Vector3 jumpDirection;
    float jumpSpeed = 20f;
    float jumpDuration = 0.2f;

    public override void Enter(PlayerCharacter character)
    {
        this.jumpDirection = Vector3.up * 1.5f;

        if (this.IsWallInDirection(character, Vector3.left) == true)
        {
            this.jumpDirection += Vector3.right;
            character.characterAnimator.SetBool("WallJumpRight", true);
        }
        else
        {
            this.jumpDirection += Vector3.left;
            character.characterAnimator.SetBool("WallJumpLeft", true);
        }

        character.audioSource.clip = Resources.Load<AudioClip>("Audio/wallJump");
        character.audioSource.Play();

        character.playerRb.useGravity = false;
    }

    public override void Exit(PlayerCharacter character)
    {
        if (this.nearestWallLatchPoint != Vector3.zero)
        {
            character.playerRb.MovePosition(this.nearestWallLatchPoint);
        }

        character.characterAnimator.SetBool("WallJumpLeft", false);
        character.characterAnimator.SetBool("WallJumpRight", false);
    }

    public override void UpdateState(PlayerCharacter character)
    {
        Vector3 moveVector = (this.jumpDirection.normalized * this.jumpSpeed * Time.fixedDeltaTime);

        if (this.IsNearWall(character, new Vector3(moveVector.x, 0.0f, 0.0f)) == true)
        {
            this.ChangeState(character, new WallSlideState());
            return;
        }

        if (jumpDuration > 0.0f)
        {
            Vector3 targetPosition = character.playerRb.position + moveVector;
            character.playerRb.MovePosition(targetPosition);
            this.jumpDuration -= Time.fixedDeltaTime;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            this.ChangeState(character, new GlideState());
        }
        else
        {
            this.ChangeState(character, new FallState());
        }
    }

    private bool IsWallInDirection(PlayerCharacter character, Vector3 direction)
    {
        float raycastMagnitude = character.playerCollider.bounds.extents.x + 0.2f;
        return Physics.Raycast(character.playerRb.position, direction, raycastMagnitude, character.groundLayer);
    }
}
