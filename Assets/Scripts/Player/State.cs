using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    public static Vector3 velocity;

    void Awake()
    {
        velocity = new Vector3(0, 0, 0);
    }
}
