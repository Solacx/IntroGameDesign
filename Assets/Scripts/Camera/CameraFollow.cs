using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Based on video guide.
 * 
 * Source: https://github.com/SebLague/2DPlatformer-Tutorial
 */
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Movement2D controller;

    private FocusArea cameraFocusArea;
    public Vector2 cameraFocusAreaSize;

    //
    public float verticalOffset;
	public float lookAheadDstX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;

    float currentLookAheadX;
	float targetLookAheadX;
	float lookAheadDirX;
	float smoothLookVelocityX;
	float smoothVelocityY;

	bool lookAheadStopped;
    //

    void Start() {
        cameraFocusArea = new FocusArea(controller.collider.bounds, cameraFocusAreaSize);
    }

    void LateUpdate() {
        cameraFocusArea.Update(controller.collider.bounds);

        Vector2 newFocusArea = cameraFocusArea.center + Vector2.up * verticalOffset;
        if (cameraFocusArea.velocity.x != 0) {
            lookAheadDirX = Mathf.Sign(cameraFocusArea.velocity.x);

            if (Mathf.Sign(controller.userInput.x) == Mathf.Sign(cameraFocusArea.velocity.x) && controller.userInput.x != 0) {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * lookAheadDstX;
            } else {
                if (!lookAheadStopped) {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4F;
                }
            }
        }

        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
        newFocusArea.y = Mathf.SmoothDamp(transform.position.y, newFocusArea.y, ref smoothVelocityY, verticalSmoothTime);
        newFocusArea += Vector2.right * currentLookAheadX;
        transform.position = (Vector3) newFocusArea + Vector3.forward * -10;
    }

    void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 0.50F);
        Gizmos.DrawCube(cameraFocusArea.center, cameraFocusAreaSize);
    }

    public struct FocusArea {
        public Vector2 center;
        public Vector2 velocity;
        
        float left;
        float right;
        float top;
        float bottom;

        public FocusArea(Bounds newBounds, Vector2 size) {
            left = newBounds.center.x - (size.x / 2);
            right = newBounds.center.x + (size.x / 2);
            bottom = newBounds.min.y;
            top = newBounds.min.y + size.y;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = Vector2.zero;
        }

        public void Update(Bounds newBounds) {
            float shiftX = 0;
            if (newBounds.min.x < left) {
                shiftX = newBounds.min.x - left;
            } else if (newBounds.max.x > right) {
                shiftX = newBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (newBounds.min.y < bottom) {
                shiftY = newBounds.min.y - bottom;
            } else if (newBounds.max.y > top) {
                shiftY = newBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
