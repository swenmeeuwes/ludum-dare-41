using System;

public interface IEventDispatcher
{
    void AddEventListener(string type, Action<EventObject> action, bool autoClear);
    void RemoveEventListener(string type, Action<EventObject> action);
    void Dispatch(EventObject eventObject);
}
