using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    public void Pickup(Player player) {
        // There's a better loop using iterators on GetChild() -> DO IT LATER
        foreach (Transform child in player.transform) {
            foreach (Transform innerChild in child) {
                if (innerChild.gameObject.name == "Eyes") {
                    // Match to EYES is gonna be hella issue later
                    innerChild.gameObject.SetActive(true);
                }
            }
        }
    }
}
