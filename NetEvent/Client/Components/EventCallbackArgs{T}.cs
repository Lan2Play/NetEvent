namespace NetEvent.Client.Components
{
    public class EventCallbackArgs<T>
    {
        public EventCallbackArgs(T value)
        {
            Value = value;
        }

        public bool Cancel { get; set; }

        public T Value { get; }
    }
}
