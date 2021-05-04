using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Based on video guide.
 * 
 * Source: https://github.com/SebLague/2DPlatformer-Tutorial
 */
public class RaycastController : MonoBehaviour
{
    public LayerMask collisionMask;
    public LayerMask interactionMask;

    public const float skinLength = 0.015F;
    public const float spaceBetweenRays = 0.25F;

    [HideInInspector] public int raysX;
    [HideInInspector] public int raysY;
    [HideInInspector] public float spacingX;
    [HideInInspector] public float spacingY;

    [HideInInspector] public new BoxCollider2D collider;
    [HideInInspector] public CastOrigins castOrigins;

    public virtual void Awake() {
        collider = GetComponent<BoxCollider2D>();
    }

    public virtual void Start() {
        CalculateRaySpacing();
    }

    public void CalculateRaySpacing() {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinLength * -2);

        raysX = Mathf.RoundToInt(bounds.size.y / spaceBetweenRays);
        raysY = Mathf.RoundToInt(bounds.size.x / spaceBetweenRays);

        spacingX = bounds.size.y / (raysX - 1);
        spacingY = bounds.size.x / (raysY - 1);
    }

    public void UpdateCastOrigins() {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinLength * -2);

        castOrigins.BL = new Vector2(bounds.min.x, bounds.min.y);
        castOrigins.BR = new Vector2(bounds.max.x, bounds.min.y);
        castOrigins.TL = new Vector2(bounds.min.x, bounds.max.y);
        castOrigins.TR = new Vector2(bounds.max.x, bounds.max.y);
    }

    public struct CastOrigins {
        public Vector2 TL;
        public Vector2 TR;
        public Vector2 BL;
        public Vector2 BR;
    }
}
