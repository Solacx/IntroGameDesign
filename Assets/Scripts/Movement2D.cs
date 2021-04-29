using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Based on video guide.
 * 
 * Source: https://github.com/SebLague/2DPlatformer-Tutorial
 */
public class Movement2D : RaycastController
{
    public CollisionData collisions;
    public Vector2 userInput;

    [SerializeField] private float maxSlopeAngle = 40;
    
    public override void Start() {
        base.Start();

        collisions.moveDirection = 1;
    }

    public void Move(Vector2 moveAmount, bool isGrounded) {
        Move(moveAmount, Vector2.zero, isGrounded);
    }

    public void Move(Vector2 moveAmount, Vector2 input, bool isGrounded = false) {
        UpdateCastOrigins();

        collisions.Reset();
        collisions.moveAmountOld = moveAmount;
        userInput = input;

        if (moveAmount.y < 0) {
            moveAmount = DescendSlope(moveAmount);
        }

        if (moveAmount.x != 0) {
            collisions.moveDirection = (int) Mathf.Sign(moveAmount.x);
        }

        moveAmount = HandleCollisionsX(moveAmount);
        if (moveAmount.y != 0) {
            moveAmount = HandleCollisionsY(moveAmount);
        }

        transform.Translate(moveAmount);

        if (isGrounded) {
            collisions.isBelow = true;
        }
    }

    private Vector2 HandleCollisionsX(Vector2 moveAmount) {
        float moveDirection = Mathf.Sign(moveAmount.x);
        float castRange = Mathf.Abs(moveAmount.x) + skinLength;
        Vector2 castOrigin;

        // NB: Below code seems to handle an edge case but not sure
        if (Mathf.Abs(moveAmount.x) < skinLength) {
            castRange = 2 * skinLength;
        }

        for (int i = 0; i < raysX; i++) {
            castOrigin = (moveDirection == -1) ? castOrigins.BL : castOrigins.BR;
            castOrigin += Vector2.up * spacingX * i;
            RaycastHit2D collision = Physics2D.Raycast(castOrigin, Vector2.right * moveDirection, castRange, collisionMask);

            Debug.DrawRay(castOrigin, Vector2.right * moveDirection, Color.red);

            if (collision) {
                float slopeAngle = Vector2.Angle(collision.normal, Vector2.up);
                
                if (collision.distance == 0) {
                    continue;
                }

                // Move an appropriate amount up slope
                if (i == 0 && slopeAngle <= maxSlopeAngle) {
                    float slopeDistance = 0;

                    if (collisions.isDescending) {
                        collisions.isDescending = false;
                        moveAmount = collisions.moveAmountOld;
                    }

                    // Move to slope
                    if (slopeAngle != collisions.slopeAngleOld) {
                        slopeDistance = collision.distance - skinLength;
                        moveAmount.x -= slopeDistance * moveDirection;
                    }
                    moveAmount = ClimbSlope(moveAmount, slopeAngle, collision.normal);
                    moveAmount.x += slopeDistance * moveDirection;
                }

                if (!collisions.isClimbing || slopeAngle > maxSlopeAngle) {
                    moveAmount.x = moveDirection * (collision.distance - skinLength);
                    if (collisions.isClimbing) {
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    collisions.isLeft = (moveDirection == -1);
                    collisions.isRight = (moveDirection == 1);
                    castRange = collision.distance;
                }
            }
        }

        return moveAmount;
    }

    private Vector2 HandleCollisionsY(Vector2 moveAmount) {
        float moveDirectionX = Mathf.Sign(moveAmount.x);
        float moveDirectionY = Mathf.Sign(moveAmount.y);
        float castRangeX = Mathf.Abs(moveAmount.x) + skinLength;
        float castRangeY = Mathf.Abs(moveAmount.y) + skinLength;
        Vector2 castOrigin;

        for (int i = 0; i < raysY; i++) {
            castOrigin = (moveDirectionY == -1) ? castOrigins.BL : castOrigins.TL;
            castOrigin += Vector2.right * (spacingY * i + moveAmount.x);
            RaycastHit2D collision = Physics2D.Raycast(castOrigin, Vector2.up * moveDirectionY, castRangeY, collisionMask);

            Debug.DrawRay(castOrigin, Vector2.up * moveDirectionY, Color.red);

            if (collision) {
                // NB: Source code has code here to create elements
                //     passable in one direction. It is not needed here
                //     so it has been removed for now.

                moveAmount.y = moveDirectionY * (collision.distance - skinLength);
                if (collisions.isClimbing) {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                collisions.isAbove = (moveDirectionY == 1);
                collisions.isBelow = (moveDirectionY == -1);
                castRangeY = collision.distance;
            }
        }

        // Move an appropriate amount up slope
        if (collisions.isClimbing) {
            castOrigin = (moveDirectionX == -1) ? castOrigins.BL : castOrigins.BR;
            castOrigin += Vector2.up * moveAmount.y;
            RaycastHit2D collision = Physics2D.Raycast(castOrigin, Vector2.right * moveDirectionX, castRangeX, collisionMask);

            if (collision) {
                float slopeAngle = Vector2.Angle(collision.normal, Vector2.up);

                // Slope has changed
                if (slopeAngle != collisions.slopeAngle) {
                    moveAmount.x = moveDirectionX * (collision.distance - skinLength);
                    collisions.slopeAngle = slopeAngle;
                    collisions.slopeNormal = collision.normal;
                }
            }
        }

        return moveAmount;
    }

    private Vector2 ClimbSlope(Vector2 moveAmount, float slopeAngle, Vector2 slopeNormal) {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float moveDistanceClimbing = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (moveAmount.y <= moveDistanceClimbing) {
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
            moveAmount.y = moveDistanceClimbing;

            collisions.isBelow = true;
            collisions.isClimbing = true;
            collisions.slopeAngle = slopeAngle;
            collisions.slopeNormal = slopeNormal;
        }

        return moveAmount;
    }

    private Vector2 DescendSlope(Vector2 moveAmount) {
        RaycastHit2D collisionMaxSlopeL = Physics2D.Raycast(castOrigins.BL, Vector2.down, Mathf.Abs(moveAmount.y) + skinLength, collisionMask);
        RaycastHit2D collisionMaxSlopeR = Physics2D.Raycast(castOrigins.BR, Vector2.down, Mathf.Abs(moveAmount.y) + skinLength, collisionMask);

        if (collisionMaxSlopeL ^ collisionMaxSlopeR) {
            moveAmount = SlideDownSlope(moveAmount, collisionMaxSlopeL);
            moveAmount = SlideDownSlope(moveAmount, collisionMaxSlopeR);
        }

        if (!collisions.isSliding) {
            float moveDirection = Mathf.Sign(moveAmount.x);
            Vector2 castOrigin = (moveDirection == -1) ? castOrigins.BR : castOrigins.BL;
            RaycastHit2D collision = Physics2D.Raycast(castOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

            if (collision) {
                float slopeAngle = Vector2.Angle(collision.normal, Vector2.up);
                if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle) {
                    if (Mathf.Sign(collision.normal.x) == moveDirection) {
                        if (collision.distance - skinLength <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x)) {
                            float moveDistance = Mathf.Abs(moveAmount.x);

                            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                            moveAmount.y -= Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

                            collisions.isBelow = true;
                            collisions.isDescending = true;
                            collisions.slopeAngle = slopeAngle;
                            collisions.slopeNormal = collision.normal;
                        }
                    }
                }
            }
        }
        
        return moveAmount;
    }

    private Vector2 SlideDownSlope(Vector2 moveAmount, RaycastHit2D collision) {
        if (collision) {
            float slopeAngle = Vector2.Angle(collision.normal, Vector2.up);

            if (slopeAngle > maxSlopeAngle) {
                moveAmount.x = Mathf.Sign(collision.normal.x) * (Mathf.Abs(moveAmount.y) - collision.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);

                collisions.isSliding = true;
                collisions.slopeAngle = slopeAngle;
                collisions.slopeNormal = collision.normal;
            }
        }

        return moveAmount;
    }

    /**
     * NB: Routine is outside original struct because it needs to be
     *     used in an Invoke call.
     */
    private void ResetFallingThroughPlatform() {
        collisions.isFallingThroughPlatform = false;
    }

    public struct CollisionData {
        public bool isAbove;
        public bool isBelow;
        public bool isLeft;
        public bool isRight;

        public bool isClimbing;
        public bool isDescending;
        public bool isSliding;

        public float slopeAngle;
        public float slopeAngleOld;
        public Vector2 slopeNormal;

        public Vector2 moveAmountOld;
        public int moveDirection;
        public bool isFallingThroughPlatform;

        public void Reset() {
            isAbove = false;
            isBelow = false;
            isLeft = false;
            isRight = false;

            isClimbing = false;
            isDescending = false;
            isSliding = false;

            slopeNormal = Vector2.zero;
            
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
