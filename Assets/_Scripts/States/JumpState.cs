using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AirborneState
{
    private float initialJumpForce = 8f;
    private float residualJumpForce = 30f;
    private float residualJumpTime = 0.5f;

    private bool stillJumping = true;

    public override void Enter(PlayerCharacter character)
    {
        character.characterAnimator.SetBool("Jump", true);
        character.playerRb.AddForce(Vector3.up * this.initialJumpForce, ForceMode.Impulse);

        character.audioSource.clip = Resources.Load<AudioClip>("Audio/jump");
        character.audioSource.Play();

        base.Enter(character);
    }

    public override void Exit(PlayerCharacter character)
    {
        base.Exit(character);

        character.characterAnimator.SetBool("Jump", false);
    }

    public override void HandleInput(PlayerCharacter character)
    {
        base.HandleInput(character);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.ChangeState(character, new FallState());
        }
        if (Input.GetKey(KeyCode.Space) && this.residualJumpTime <= 0.0f)
        {
            this.ChangeState(character, new GlideState());
        }
    }

    public override void UpdateState(PlayerCharacter character)
    {
        if (this.stillJumping == true && this.residualJumpTime > 0.0f)
        {
            character.playerRb.AddForce(Vector3.up * this.residualJumpForce, ForceMode.Force);
            this.residualJumpTime -= Time.fixedDeltaTime;
        }

        base.UpdateState(character);
    }
}
