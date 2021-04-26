using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Based on video guide. Adapted to consider Z-axis since project is
 * created in 3D.
 * 
 * Source: https://github.com/SebLague/2DPlatformer-Tutorial
 */
public class PlayerInput : MonoBehaviour
{
    private Player player;

    void Start() {
        player = GetComponent<Player>();
    }

    void Update() {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        player.SetUserInput(input);

        if (Input.GetKeyDown(KeyCode.Space)) {
            player.OnJumpInputDown();
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            player.OnJumpInputUp();
        }
    }
}
