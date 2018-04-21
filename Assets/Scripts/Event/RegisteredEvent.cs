using System;

[Serializable]
public class RegisteredEvent
{
    public Action<EventObject> Action { get; set; }
    public bool AutoClear { get; set; }
}
