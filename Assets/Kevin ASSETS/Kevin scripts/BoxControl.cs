using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxControl : MonoBehaviour
{
    public bool canOpenDoor = false;
    public bool canWin = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if(canOpenDoor)
        {
            if(other.name=="1"|| other.name == "2"|| other.name == "3")
            {
                DoorControl.Instance.Open(int.Parse(other.name));
            }
        }
        if(canWin)
        {
            if (other.name == "winDoor")
            {
                DoorControl.Instance.OpenWinDoor(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (canOpenDoor)
        {
            if (other.name == "1" || other.name == "2" || other.name == "3")
            {
                DoorControl.Instance.Close(int.Parse(other.name));
            }
        }
        if (canWin)
        {
            if (other.name == "winDoor")
            {
                DoorControl.Instance.OpenWinDoor(false);
            }
        }

    }

        void Update()
    {
        
    }
}
