using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerState
{
    private float moveSpeed = 8f;
    private Vector3 moveDirection = Vector3.zero;

    public override void Enter(PlayerCharacter character)
    {
        Debug.LogError("EnterRun");
        character.audioSource.clip = Resources.Load<AudioClip>("Audio/run");
    }

    public override void Exit(PlayerCharacter character)
    {
        character.characterAnimator.SetBool("RunRight", false);
        character.characterAnimator.SetBool("RunLeft", false);
    }

    public override void HandleInput(PlayerCharacter character)
    {
        this.moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.moveDirection += Vector3.right;

            character.characterAnimator.SetBool("RunRight", true);
            character.characterAnimator.SetBool("RunLeft", false);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.moveDirection += Vector3.left;

            character.characterAnimator.SetBool("RunRight", false);
            character.characterAnimator.SetBool("RunLeft", true);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.ChangeState(character, new JumpState());
        }

        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.RightArrow) &&
            !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.LeftArrow) &&
            !Input.GetKey(KeyCode.Space))
        {
            this.ChangeState(character, new IdleState());
        }
    }

    public override void UpdateState(PlayerCharacter character)
    {
        if (this.IsGrounded(character) == false)
        {
            this.ChangeState(character, new FallState());           
            return;
        }

        Vector3 moveVector = this.moveDirection * this.moveSpeed * Time.fixedDeltaTime;

        if (this.IsNearWall(character, moveVector) == false)
        {
            Vector3 targetPosition = character.playerRb.position + moveVector;
            character.playerRb.MovePosition(targetPosition);

            if (character.audioSource.isPlaying == false)
            {
                character.audioSource.Play();
            }
        }
    }
}
