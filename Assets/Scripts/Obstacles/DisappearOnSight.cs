using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearOnSight : MonoBehaviour
{
    public void Disappear() {
        gameObject.SetActive(false);
    }
}
