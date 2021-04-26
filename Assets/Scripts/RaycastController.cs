using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Based on video guide. Adapted to consider Z-axis since project is
 * created in 3D.
 * 
 * Source: https://github.com/SebLague/2DPlatformer-Tutorial
 */
public class RaycastController : MonoBehaviour
{
    public LayerMask collisionMask;

    public const float skinLength = 0.015F;
    public const float spaceBetweenRays = 0.25F;

    [HideInInspector] public int raysX;
    [HideInInspector] public int raysY;
    [HideInInspector] public float spacingX;
    [HideInInspector] public float spacingY;

    [HideInInspector] public new BoxCollider collider;
    [HideInInspector] public CastOrigins castOrigins;

    public virtual void Awake() {
        collider = GetComponent<BoxCollider>();
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

        castOrigins.BL = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        castOrigins.BR = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        castOrigins.TL = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        castOrigins.TR = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
    }

    public struct CastOrigins {
        public Vector3 TL;
        public Vector3 TR;
        public Vector3 BL;
        public Vector3 BR;
    }
}
