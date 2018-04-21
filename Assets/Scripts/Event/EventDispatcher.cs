using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : IEventDispatcher
{
    private readonly Dictionary<string, List<RegisteredEvent>> _eventDictionary = new Dictionary<string, List<RegisteredEvent>>();

    public void AddEventListener(string type, Action<EventObject> action, bool autoClear = false)
    {
        if (!_eventDictionary.ContainsKey(type))
            _eventDictionary[type] = new List<RegisteredEvent>();

        _eventDictionary[type].Add(
            new RegisteredEvent
            {
                Action = action,
                AutoClear = autoClear
            }
        );
    }

    public void RemoveEventListener(string type, Action<EventObject> action)
    {
        if (!_eventDictionary.ContainsKey(type))
        {
            Debug.LogWarning("RemoveEventListener called with unregistered type!");
            return;
        }

        var foundListener = _eventDictionary[type].Find(item => item.Action == action);
        if (!_eventDictionary[type].Remove(foundListener))
            Debug.LogWarning("RemoveEventListener called with unregistered action!");
    }

    public void Dispatch(EventObject eventObject)
    {
        if (!_eventDictionary.ContainsKey(eventObject.Type))
            return;

        for (var i = _eventDictionary[eventObject.Type].Count - 1; i >= 0; i--)
        {
            var current = _eventDictionary[eventObject.Type][i];
            current.Action.Invoke(eventObject);

            if (current.AutoClear)
                _eventDictionary[eventObject.Type].Remove(current);
        }
    }
}
