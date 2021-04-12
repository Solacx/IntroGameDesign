﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : ScriptableObject
{
    private List<GameEventListener> eventListeners = new List<GameEventListener>();

    public void Raise()
    {
        eventListeners.ForEach(i => i.OnEventRaised());
    }

    public void AddListener(GameEventListener x)
    {
        eventListeners.Add(x);
    }

    public void RemoveListener(GameEventListener x)
    {
        eventListeners.Remove(x);
    }
}
