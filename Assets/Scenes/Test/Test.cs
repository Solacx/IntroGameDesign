using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Temp something;

    void Start() {
        something.SetValue(1);

        Debug.Log(something.value);
    }
    
    public struct Temp {
        public int value;

        public void SetValue(int x) {
            value = x;
        }
    }
}
