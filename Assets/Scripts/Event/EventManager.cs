using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static MoveEvent moveEvent;
    public static AnimationEvent animationEvent;

    void Awake()
    {
        moveEvent = ScriptableObject.CreateInstance<MoveEvent>();
        animationEvent = ScriptableObject.CreateInstance<AnimationEvent>();
    }

    public static void createMoveEvent(string input, bool isCrouching)
    {
        moveEvent.SetValues(input, isCrouching);
        moveEvent.Raise();
    }

    public static void createAnimationEvent(string action)
    {
        animationEvent.SetAction(action);
        animationEvent.Raise();
    }
}
