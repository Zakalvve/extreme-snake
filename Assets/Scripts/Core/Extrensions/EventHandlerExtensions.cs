using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeSnake.Core
{
    public class EventArgs<T> : EventArgs {
        public T Argument { get; set; }
        public EventArgs(T argument) {
            Argument = argument;
        }
    }

    public class ReadOnlyEventArgs<T> : EventArgs
    {
        public T Argument { get; private set; }

        public ReadOnlyEventArgs(T argument) {
            Argument = argument;
        }
    }

    public static class EventHandlerExtensions
    {
        public static EventArgs<T> CreateArgs<T>(
        this EventHandler<EventArgs<T>> _,
        T argument) {
            return new EventArgs<T>(argument);
        }

        public static ReadOnlyEventArgs<T> CreateArgs<T>(
            this EventHandler<ReadOnlyEventArgs<T>> _,
            T argument) {
            return new ReadOnlyEventArgs<T>(argument);
        }
    }
}
