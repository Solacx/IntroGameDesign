using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private bool isCrouching;

    void Update()
    {
        processMovementInputs();

        // Crouching is not considered as movement
        if (Input.GetKey(KeyCode.DownArrow))
        {
            EventManager.createAnimationEvent("CROUCH");
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            EventManager.createAnimationEvent("STAND");
        }
    }

    private void processMovementInputs()
    {
        isCrouching = Input.GetKey(KeyCode.DownArrow);

        if (Input.GetKey(KeyCode.RightArrow))
        {
            EventManager.createMoveEvent("RIGHT", isCrouching);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            EventManager.createMoveEvent("LEFT", isCrouching);
        }
    }
}
