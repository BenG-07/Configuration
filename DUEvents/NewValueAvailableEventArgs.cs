using System;

namespace DUEvents
{
    public class NewValueAvailableEventArgs<T> : EventArgs
    {
        public T Value { get; set; }

        public NewValueAvailableEventArgs(T value)
        {
            this.Value = value;
        }
    }
}
