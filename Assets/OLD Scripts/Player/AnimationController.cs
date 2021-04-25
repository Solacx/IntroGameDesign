using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Vector3 crouchScale = new Vector3(1F, 0.80F, 1F);

    private GameEventListener moveEventListener;
    private GameEventListener animationEventListener;

    void Awake()
    {
        moveEventListener = gameObject.AddComponent<GameEventListener>();
        animationEventListener = gameObject.AddComponent<GameEventListener>();
    }
    
    void Start()
    {
        MoveEvent moveEvent = EventManager.moveEvent;
        moveEventListener.SetEvent(moveEvent);
        moveEventListener.AddResponse(HandleMoveEvent);
        moveEvent.AddListener(moveEventListener);

        AnimationEvent animationEvent = EventManager.animationEvent;
        animationEventListener.SetEvent(animationEvent);
        animationEventListener.AddResponse(HandleAnimationEvent);
        animationEvent.AddListener(animationEventListener);
    }

    private void HandleMoveEvent()
    {
        string input = EventManager.moveEvent.input;

        FaceDirection(input);
    }

    private void HandleAnimationEvent()
    {
        string action = EventManager.animationEvent.action;

        switch (action)
        {
            case "STAND": Stand(); break;
            case "CROUCH": Crouch(); break;
        }
    }

    private void FaceDirection(string direction)
    {
        transform.rotation = Quaternion.identity;

        if (direction == "LEFT")
        {
            transform.Rotate(0, 180, 0);
        }
    }

    private void Stand()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }

    private void Crouch()
    {
        transform.localScale = crouchScale;
    }
}
