using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Based on video guide.
 * 
 * Source: https://github.com/SebLague/2DPlatformer-Tutorial
 */
public class Player : MonoBehaviour
{
    [SerializeField] private Movement2D controller;

    [SerializeField] private float moveSpeed = 6.00F;
    [SerializeField] private float accelerationTimeGrounded = 0.10F;
    [SerializeField] private float accelerationTimeAirborne = 0.20F;

    private float maxJumpHeight = 4.00F;
    private float minJumpHeight = 1.00F;
    private float timeToJumpApex = 0.40F;

    private Vector2 velocity;
    private float smoothVelocity;
    private float maxJumpVelocity;
    private float minJumpVelocity;
    private float gravity;

    private Vector2 userInput;

    void Start() {
        controller = GetComponent<Movement2D>();
        
        CalculateJump();
    }

    void Update() {
        CalculateVelocity();

        controller.Move(velocity * Time.deltaTime, userInput);

        if (controller.collisions.isAbove || controller.collisions.isBelow) {
            // Spikes
            if (controller.collisions.isHazard) {
                OnDeath();
            }

            // Slope
            if (controller.collisions.isSliding) {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            } else {
                velocity.y = 0;
            }
        }
    }

    public void OnDeath() {
        // For now leave it as reset position
        transform.position = Vector3.zero;
    }

    public void SetUserInput(Vector2 input) {
        userInput = input;
    }

    public void OnJumpInputDown() {
        // NB: Source code to enable sliding on walls has been removed
        //     since it is not needed.

        if (controller.collisions.isBelow) {
            if (controller.collisions.isSliding) {
                if (userInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) {
                    // Move direction is into wall
                    velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
                    velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            } else {
                velocity.y = maxJumpVelocity;
            }
        }
    }

    public void OnJumpInputUp() {
        if (velocity.y > minJumpVelocity) {
            velocity.y = minJumpVelocity;
        }
    }

    private void CalculateVelocity() {
        float targetVelocity = userInput.x * moveSpeed;

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocity, ref smoothVelocity, (controller.collisions.isBelow) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }

    private void CalculateJump() {
        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }
}
