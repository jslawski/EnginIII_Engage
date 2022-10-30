using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideState : PlayerState
{
    private enum WallSide { Left, Right, None }
    private WallSide latchedWall = WallSide.None;

    private float maxFallSpeed = 2f;

    public override void Enter(PlayerCharacter character)
    {
        if (this.IsWallInDirection(character, Vector3.left) == true)
        {
            this.latchedWall = WallSide.Left;
            character.characterAnimator.SetBool("SlideLeft", true);
        }
        else
        {
            this.latchedWall = WallSide.Right;
            character.characterAnimator.SetBool("SlideRight", true);
        }

        character.audioSource.clip = Resources.Load<AudioClip>("Audio/wallSlide");
        character.audioSource.Play();
    }

    public override void Exit(PlayerCharacter character)
    {
        if (this.distanceToGround > 0.0f)
        {
            Vector3 correctionPosition = character.playerRb.position + (Vector3.down * this.distanceToGround);
            character.playerRb.MovePosition(correctionPosition);
        }

        character.characterAnimator.SetBool("SlideRight", false);
        character.characterAnimator.SetBool("SlideLeft", false);
    }

    public override void HandleInput(PlayerCharacter character)
    {
        if (this.latchedWall == WallSide.Left)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                this.ChangeState(character, new FallState());                
            }
        }
        else if (this.latchedWall == WallSide.Right)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                this.ChangeState(character, new FallState());                
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.ChangeState(character, new WallJumpState());
        }
    }

    public override void UpdateState(PlayerCharacter character)
    {
        if (Mathf.Abs(character.playerRb.velocity.y) > this.maxFallSpeed)
        {
            character.playerRb.velocity = new Vector3(character.playerRb.velocity.x, -this.maxFallSpeed, 0.0f);
        }

        if (this.IsGrounded(character) == true)
        {
            this.ChangeState(character, new IdleState());

            character.audioSource.clip = Resources.Load<AudioClip>("Audio/land");
            character.audioSource.Play();

            return;
        }
    }

    private bool IsWallInDirection(PlayerCharacter character, Vector3 direction)
    {
        float raycastMagnitude = character.playerCollider.bounds.extents.x + 0.2f;
        return Physics.Raycast(character.playerRb.position, direction, raycastMagnitude, character.groundLayer);
    }
}
