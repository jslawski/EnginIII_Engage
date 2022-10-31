using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private PlayerController player;

    private Camera thisCam;
    private Transform cameraTransform;
    private float cameraZDistance;

    [SerializeField, Range(0, 1)]
    private float upperVerticalViewportThreshold = 0.8f;
    [SerializeField, Range(0, 1)]
    private float lowerVerticalViewportThreshold = 0.4f;
    [SerializeField, Range(0, 1)]
    private float leftHorizontalViewportThreshold = 0.3f;
    [SerializeField, Range(0, 1)]
    private float rightHorizontalViewportThreshold = 0.3f;

    private Vector3 compositeShiftVector = Vector3.zero;

    private void Awake()
    {
        this.thisCam = this.gameObject.GetComponent<Camera>();
        this.cameraTransform = this.gameObject.transform;
        this.cameraZDistance = this.cameraTransform.position.z;

        this.player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private bool IsPlayerPastVerticalThreshold(float playerViewportYPosition)
    {
        return (playerViewportYPosition > this.upperVerticalViewportThreshold) || 
            (playerViewportYPosition <= this.lowerVerticalViewportThreshold);        
    }

    private bool IsPlayerPastHorizontalThreshold(float playerViewportXPosition)
    {
        return (playerViewportXPosition > this.leftHorizontalViewportThreshold) ||
            (playerViewportXPosition < this.rightHorizontalViewportThreshold);
    }

    private void FixedUpdate()
    {
        this.compositeShiftVector = Vector3.zero;

        Vector3 playerViewportPosition = this.thisCam.WorldToViewportPoint(this.player.playerRb.position);

        if (this.player != null && this.IsPlayerPastVerticalThreshold(playerViewportPosition.y))
        {
            this.UpdateCameraVerticalPosition(playerViewportPosition.y);
        }

        if (this.IsPlayerPastHorizontalThreshold(playerViewportPosition.x))
        {
            this.UpdateCameraHorizontalPosition(playerViewportPosition.x);
        }

        this.cameraTransform.position += this.compositeShiftVector;
    }

    private void UpdateCameraVerticalPosition(float playerViewportPositionY)
    {
        Vector3 worldSpaceThresholdPosition = Vector3.zero;

        if (playerViewportPositionY > 0.5f)
        {
            worldSpaceThresholdPosition = 
                this.thisCam.ViewportToWorldPoint(new Vector3(0.5f, this.upperVerticalViewportThreshold, this.player.playerRb.position.z));
        }
        else
        {
            worldSpaceThresholdPosition = 
                this.thisCam.ViewportToWorldPoint(new Vector3(0.5f, this.lowerVerticalViewportThreshold, this.player.playerRb.position.z));
        }

        this.compositeShiftVector += new Vector3(0.0f, this.player.playerRb.position.y - worldSpaceThresholdPosition.y, 0.0f);
    }

    private void UpdateCameraHorizontalPosition(float playerViewportPositionX)
    {
        Vector3 worldSpaceThresholdPosition = Vector3.zero;

        if (playerViewportPositionX > 0.5f)
        {
            worldSpaceThresholdPosition =
                this.thisCam.ViewportToWorldPoint(new Vector3(this.leftHorizontalViewportThreshold, 0.5f, this.player.playerRb.position.z));
        }
        else
        {
            worldSpaceThresholdPosition =
                this.thisCam.ViewportToWorldPoint(new Vector3(this.rightHorizontalViewportThreshold, 0.5f, this.player.playerRb.position.z));
        }

        this.compositeShiftVector += new Vector3(this.player.playerRb.position.x - worldSpaceThresholdPosition.x, 0.0f);
    }
}
