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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
        {
            this.waterCollider = other;
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
