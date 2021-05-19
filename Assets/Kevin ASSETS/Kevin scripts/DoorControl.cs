using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    private static DoorControl instance;
    public static DoorControl Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.Find("Door").GetComponent<DoorControl>();
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public bool isOne=false;
    public bool isTwo= false;
    public bool isThree = false;
    public Transform[] door1s;
    public Transform[] door2s;
    public Transform[] door3s;
    public Transform winDoor;

    public bool isWinDoor = false;
    private Vector2 openTarget;
    private Vector2 closeTarget;
    public void OpenWinDoor(bool open)
    {
        isWinDoor = open;
    }
    public void Open(int index)
    {
        switch (index)
        {
            case 1: isOne = true;break;
            case 2: isTwo = true; break;
            case 3: isThree = true; break;
        }
    }
    public void Close(int index)
    {
        switch (index)
        {
            case 1: isOne = false; break;
            case 2: isTwo = false; break;
            case 3: isThree = false; break;
        }
    }

    void Start()
    {
        openTarget = winDoor.position + Vector3.up*3;
        closeTarget = winDoor.position;
    }

    void Update()
    {
        if(isWinDoor)
        {
            winDoor.position = Vector2.Lerp(winDoor.position, openTarget, 0.3f);
        }
        else
        {
            winDoor.position = Vector2.Lerp(winDoor.position, closeTarget, 0.3f);
        }

        if(isOne)
        {
            for (int i = 0; i < door1s.Length; i++)
            {
                door1s[i].rotation = Quaternion.Lerp(door1s[i].rotation, Quaternion.Euler(0, 0, 180), 0.2f);
            }
        }
        else
        {
            for (int i = 0; i < door1s.Length; i++)
            {
                door1s[i].rotation = Quaternion.Lerp(door1s[i].rotation, Quaternion.Euler(0, 0, 90), 0.2f);
            }
        }
        if(isTwo)
        {
            for (int i = 0; i < door2s.Length; i++)
            {
                door2s[i].rotation = Quaternion.Lerp(door2s[i].rotation, Quaternion.Euler(0, 0, 180), 0.2f);
            }
        }
        else
        {
            for (int i = 0; i < door2s.Length; i++)
            {
                door2s[i].rotation = Quaternion.Lerp(door2s[i].rotation, Quaternion.Euler(0, 0, 90), 0.2f);
            }
        }
        if (isThree)
        {
            for (int i = 0; i < door1s.Length; i++)
            {
                door3s[i].rotation = Quaternion.Lerp(door3s[i].rotation, Quaternion.Euler(0, 0, 180), 0.2f);
            }
        }
        else
        {
            for (int i = 0; i < door3s.Length; i++)
            {
                door3s[i].rotation = Quaternion.Lerp(door3s[i].rotation, Quaternion.Euler(0, 0, 90), 0.2f);
            }
        }
    }
}
