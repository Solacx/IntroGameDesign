using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEvent : GameEvent
{
    public string input;
    public bool isCrouching;

    public void SetValues(string input, bool isCrouching)
    {
        this.input = input;
        this.isCrouching = isCrouching;
    }
}
