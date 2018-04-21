public class EventObject
{
    public object Sender { get; set; }
    public string Type { get; set; }
    public object Data { get; set; }

    public override string ToString()
    {
        return string.Format("[EventObject(Sender={0}; Type={1}; Data={2};)]", Sender.ToString(), Type, Data.ToString());
    }
}