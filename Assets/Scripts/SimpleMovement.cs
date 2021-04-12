using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public void SmoothMove(Vector3 destination, ref Vector3 velocity, float smoothAmount)
    {
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothAmount);
    }
}
