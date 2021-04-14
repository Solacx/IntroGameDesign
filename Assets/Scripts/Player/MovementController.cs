﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private SimpleMovement movementController;
    [SerializeField] public float moveSpeed = 5.00F;
    [SerializeField] public float smoothAmount = 0.80F;
    [SerializeField] [Range(0F, 1.0F)] private float crouchRatio = 0.50F;
    [SerializeField] public float upForce = 7.50F;
    [SerializeField] public float downForce = 1.50F;
    [SerializeField] private UnityEngine.UI.Text temp;

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

    void Update()
    {
        // CLEAN UP THIS SECTION LATER
        Rigidbody temp = GetComponent<Rigidbody>();

        if (temp.velocity.y < 0)
        {
            temp.velocity += Vector3.up * Physics.gravity.y * downForce * Time.deltaTime;
        }
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

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Temp"))
        {
            int score = int.Parse(temp.text);
            score++;
            temp.text = score.ToString();
            transform.position = new Vector3(-5, 2, 0);
        }
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void OnCollisionExit()
    {
        isGrounded = false;
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
