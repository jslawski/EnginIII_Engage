using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlideState : AirborneState
{
    public override void Enter(PlayerCharacter character)
    {
        character.characterAnimator.SetBool("Glide", true);
        this.moveSpeed = 5f;
        this.maxFallSpeed = 0.5f;

        character.audioSource.clip = Resources.Load<AudioClip>("Audio/glide");
        character.audioSource.Play();
    }

    public override void Exit(PlayerCharacter character)
    {
        if (this.distanceToGround > 0.0f)
        {
            Vector3 correctionPosition = character.playerRb.position + (Vector3.down * this.distanceToGround);
            character.playerRb.MovePosition(correctionPosition);
        }

        character.characterAnimator.SetBool("Glide", false);

        base.Exit(character);
    }

    public override void HandleInput(PlayerCharacter character)
    {
        base.HandleInput(character);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.ChangeState(character, new FallState());
        }
    }

    public override void UpdateState(PlayerCharacter character)
    {
        if (this.IsGrounded(character) == true)
        {            
            this.ChangeState(character, new IdleState());

            character.audioSource.clip = Resources.Load<AudioClip>("Audio/land");
            character.audioSource.Play();

            return;
        }

        base.UpdateState(character);
    }
}
