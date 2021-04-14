using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temp : MonoBehaviour
{
    [SerializeField] private MovementController playerMovementScript;

    [SerializeField] private UnityEngine.UI.Button button;

    void Awake()
    {
        button.onClick.AddListener(ChangeSettings);
    }

    private void ChangeSettings()
    {
        string setting = button.GetComponentInChildren<UnityEngine.UI.Text>().text;
        switch (setting)
        {
            case "#1":
                playerMovementScript.moveSpeed = 5.00F;
                break;

            case "#2":
                playerMovementScript.moveSpeed = 4.00F;
                break;

            case "#3":
                playerMovementScript.smoothAmount = 0.80F;
                break;

            case "#4":
                playerMovementScript.smoothAmount = 1.20F;
                break;

            case "#5":
                playerMovementScript.smoothAmount = 0.40F;
                break;

            case "#6":
                playerMovementScript.downForce = 1.50F;
                break;

            case "#7":
                playerMovementScript.downForce = 2.00F;
                break;

            case "#100":
                GameObject player = GameObject.Find("Player");
                player.GetComponent<Rigidbody>().freezeRotation = false;
                break;
        }
    }
}
