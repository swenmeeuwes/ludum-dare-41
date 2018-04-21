using System;
using UnityEngine;

public class MonoEventDispatcher : MonoBehaviour, IEventDispatcher
{
    private EventDispatcher _eventDispatcher;

    protected virtual void Awake()
    {
        _eventDispatcher = new EventDispatcher();
    }

    public void AddEventListener(string type, Action<EventObject> action, bool autoClear = false)
    {
        _eventDispatcher.AddEventListener(type, action, autoClear);
    }

    public void RemoveEventListener(string type, Action<EventObject> action)
    {
        _eventDispatcher.RemoveEventListener(type, action);
    }

    public void Dispatch(EventObject eventObject)
    {
        _eventDispatcher.Dispatch(eventObject);
    }
}