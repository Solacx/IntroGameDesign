using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : GameEvent
{
    public string action;

    public void SetAction(string action)
    {
        this.action = action;
    }
}
