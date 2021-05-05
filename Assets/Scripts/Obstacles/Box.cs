using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
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
