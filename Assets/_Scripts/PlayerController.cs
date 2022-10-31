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

    private void Start()
    {
        this.playerRb = GetComponent<Rigidbody>();
        this.playerCollider = GetComponent<BoxCollider>();
        this.characterAnimator = GetComponentInChildren<Animator>();
        this.audioSource = GetComponent<AudioSource>();

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
        if (this.currentState != null)
        {
            this.currentState.Exit();
        }
        
        this.currentState = newState;
        this.currentState.Enter(this);

        this.debugStateName = this.currentState.GetType().ToString();
    }
}
