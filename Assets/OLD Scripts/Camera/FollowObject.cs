using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private GameObject temp;   // Use a better name

    void LateUpdate()
    {
        transform.position = new Vector3(
            temp.transform.position.x,
            temp.transform.position.y,
            gameObject.transform.position.z
        );
    }
}
