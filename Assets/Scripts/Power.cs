using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
{
    void OnCollisionEnter(Collision collision) {
        Debug.Log("Got new power!");

        foreach (Transform child in collision.transform) {
            if (child.gameObject.name == "Back") {
                child.gameObject.SetActive(true);
            }
        }
    }
}
