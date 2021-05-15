using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooFollow : MonoBehaviour
{
    [SerializeField] private GameObject objectToFollow;

    [SerializeField] private float speed;

    void Update() {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, objectToFollow.transform.position, step);
    }
}
