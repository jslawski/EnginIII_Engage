using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : AirborneState
{
    public override void Enter(PlayerCharacter character)
    {
        base.Enter(character);
        character.playerRb.velocity = Vector3.zero;
        character.characterAnimator.SetBool("Fall", true);

        character.audioSource.clip = Resources.Load<AudioClip>("Audio/fall");
        character.audioSource.Play();
    }

    public override void Exit(PlayerCharacter character)
    {
        if (this.distanceToGround > 0.0f)
        {
            Vector3 correctionPosition = character.playerRb.position + (Vector3.down * this.distanceToGround);
            character.playerRb.MovePosition(correctionPosition);
        }

        character.characterAnimator.SetBool("Fall", false);

        base.Exit(character);
    }

    public override void HandleInput(PlayerCharacter character)
    {
        base.HandleInput(character);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.ChangeState(character, new GlideState());
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
