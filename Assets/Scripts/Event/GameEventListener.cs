using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private UnityEvent response;

    void Awake()
    {
        response = new UnityEvent();
    }

    public void SetEvent(GameEvent gameEvent)
    {
        this.gameEvent = gameEvent;
    }

    public void AddResponse(UnityAction response)
    {
        this.response.AddListener(response);
    }

    public void RemoveResponse(UnityAction response)
    {
        this.response.RemoveListener(response);
    }

    public void OnEventRaised()
    {
        response.Invoke();
    }

    private void OnEnable()
    {
        if (gameEvent != null)
        {
            gameEvent.AddListener(this);
        }
    }

    private void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }
}
