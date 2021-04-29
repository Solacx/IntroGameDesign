using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Based on video guide.
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
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetUserInput(input);

        if (Input.GetKeyDown(KeyCode.Space)) {
            player.OnJumpInputDown();
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            player.OnJumpInputUp();
        }
    }
}
