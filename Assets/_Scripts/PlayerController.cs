using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private string debugStateName;

    public PlayerState currentState;

    [HideInInspector]
    public Rigidbody playerRb;
    [HideInInspector]
    public BoxCollider playerCollider;
    [HideInInspector]
    public Animator characterAnimator;
    [HideInInspector]
    public AudioSource audioSource;

    public LayerMask groundLayer;

    public Collider waterCollider;
    
    public float floatMagnitude = 50.0f;

    public PlayerSkills skills;

    public Vector3 latestCheckpointPosition = Vector3.zero;

    private void Start()
    {
        this.playerRb = GetComponent<Rigidbody>();
        this.playerCollider = GetComponent<BoxCollider>();
        this.characterAnimator = GetComponentInChildren<Animator>();
        this.audioSource = GetComponent<AudioSource>();
        this.skills = new PlayerSkills();
        this.skills.SetupDict();

        this.ChangeState(new FallState());
    }
    
    private void Update()
    {
        this.currentState.UpdateState();

        if (Input.GetKeyUp(KeyCode.R))
        {
            this.Respawn();
        }
    }

    private void FixedUpdate()
    {
        this.currentState.FixedUpdateState();
    }

    public void ChangeState(PlayerState newState)
    {
        //Don't change states unless the player has the skill
        if (!this.skills.HasSkill(newState.GetType().ToString()))
        {
            return;
        }

        if (this.currentState != null)
        {
            this.currentState.Exit();
        }
        
        this.currentState = newState;
        this.currentState.Enter(this);

        this.debugStateName = this.currentState.GetType().ToString();
    }

    public void Respawn()
    {
        this.playerRb.position = this.latestCheckpointPosition;
        this.ChangeState(new FallState());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            this.waterCollider = other;
        }

        if (other.tag == "Skill")
        {
            SkillPickup newSkill = other.gameObject.GetComponent<SkillPickup>();
            this.skills.AddSkill(newSkill.skillStateName);
            Destroy(other.gameObject);
        }

        if (other.tag == "Checkpoint")
        {
            this.latestCheckpointPosition = other.bounds.center;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            this.waterCollider = null;
        }        
    }
}
