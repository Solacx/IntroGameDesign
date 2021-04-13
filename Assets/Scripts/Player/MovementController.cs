using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private SimpleMovement movementController;
    [SerializeField] private float moveSpeed = 5.00F;
    [SerializeField] private float smoothAmount = 0.80F;
    [SerializeField] [Range(0F, 1.0F)] private float crouchRatio = 0.50F;
    [SerializeField] private float upForce = 7.50F;

    private GameEventListener moveEventListener;
    private bool isGrounded;

    void Awake()
    {
        moveEventListener = gameObject.AddComponent<GameEventListener>();
    }
    
    void Start()
    {
        MoveEvent moveEvent = EventManager.moveEvent;
        moveEventListener.SetEvent(moveEvent);
        moveEventListener.AddResponse(HandleMoveEvent);
        moveEvent.AddListener(moveEventListener);
    }

    void LateUpdate()
    {
        Vector3 currentPosition = transform.position;
        Vector3 destination = currentPosition + (State.velocity * 0.3F);

        if (currentPosition != destination)
        {
            // Ensure smooth movement is complete.
            movementController.SmoothMove(destination, ref State.velocity, smoothAmount);
        }
    }

    void OnCollisionEnter()
    {
        isGrounded = true;
    }

    void OnCollisionExit()
    {
        isGrounded = false;
    }

    void Update()
    {
        Debug.Log("isGrounded: " + isGrounded);
    }

    private void HandleMoveEvent()
    {
        string input = EventManager.moveEvent.input;
        bool isCrouching = EventManager.moveEvent.isCrouching;

        Vector3 destination = transform.position;

        switch (input)
        {
            case "UP":
                // THIS SECTION IS NOT EFFICIENT AND NEEDS TO BE
                // CLEANED UP LATER
                if (isGrounded)
                {
                    GetComponent<Rigidbody>().AddForce(new Vector3(0, upForce, 0), ForceMode.Impulse);
                }
                return;

            case "RIGHT":
                destination += new Vector3(moveSpeed, 0, 0) * ((isCrouching) ? crouchRatio : 1.0F);
                break;

            case "LEFT":
                destination -= new Vector3(moveSpeed, 0, 0) * ((isCrouching) ? crouchRatio : 1.0F);
                break;
        }
        
        movementController.SmoothMove(destination, ref State.velocity, smoothAmount);
    }
}
