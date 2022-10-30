using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerState
{
    public override void Enter(PlayerCharacter character)
    {        
        character.characterAnimator.SetBool("Idle", true);
        
        character.playerRb.useGravity = false;
        character.playerRb.velocity = Vector3.zero;        
    }

    public override void Exit(PlayerCharacter character)
    {
        character.characterAnimator.SetBool("Idle", false);
    }

    public override void HandleInput(PlayerCharacter character)
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.ChangeState(character, new RunState());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.ChangeState(character, new JumpState());
        }
    }
}
