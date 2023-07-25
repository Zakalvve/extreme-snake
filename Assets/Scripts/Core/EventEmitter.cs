using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ExtremeSnake.Core
{
    /// <summary>
    /// This class is used to map strings with event handlers to provide a convenient way to register and invoke events.
    /// </summary>
    public class EventEmitter
    {

        /// <summary>
        /// Stores event names a strings. Each event has an event handler which contains all the callbacks for subscribers to that event.
        /// </summary>
        private Dictionary<string,EventHandler<EventArgs>> _events = new Dictionary<string,EventHandler<EventArgs>>();

        /// <summary>
        /// Reference to all the unsub callbacks created as a result of subscriptions to this EventEmitter.
        /// </summary>
        private Stack<Action> _unsubCallbacks = new Stack<Action>();

        /// <summary>
        /// Forcefully unsubscribes all subscribers from this EventEmitter
        /// </summary>
        public void UnsubscribeFromAll() {
            while(_unsubCallbacks.Count > 0) {
                _unsubCallbacks.Pop()();
            }
        }
        
        /// <summary>
        /// Emits the event with the given name without passing any arguments. Executes all subscribed callbacks passing the object that initiated the Emit to all subscribers.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="sender"></param>
        public void Emit(string eventName, object sender) {
            if (!_events.ContainsKey(eventName)) return;
            EventHandler<EventArgs> handler = _events[eventName];
            if (_debug) Debug.Log($"Emiting event {eventName} from {sender}");
            handler?.Invoke(sender,EventArgs.Empty);
        }

        /// <summary>
        /// Subscribes a callback with only the sender object as arguments to the event with the given name.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public Action Subscribe(string eventName,Action<object> callback) {
            EventHandler<EventArgs> handler = (sender,args) => {
                callback(sender);
            };

            if (_debug) Debug.Log($"Subscribing {callback} to event {eventName}");
            if (!_events.ContainsKey(eventName))
                _events.Add(eventName,handler);
            else
                _events[eventName] += handler;

            _unsubCallbacks.Push(() => { _events[eventName] -= handler; });
            return _unsubCallbacks.Peek();
        }

        /// <summary>
        /// Emits the event with the given name. Executes all subscribed callbacks passing the object that initiated the Emit and the given T args to all subscribers.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void Emit<T>(string eventName,object sender, T args) where T : EventArgs {
            if (!_events.ContainsKey(eventName)) return;
            EventHandler<EventArgs> handler = _events[eventName];
            if (_debug) Debug.Log($"Emiting event {eventName} from {sender} with arguments {args}");
            handler?.Invoke(sender,args);
        }
        /// <summary>
        /// Subscribes a callback with only the sender object and arguments of type T as arguments to the event with the given name.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="callback"></param>
        public Action Subscribe<T>(string eventName,Action<object,T> callback) where T : EventArgs {
            EventHandler<EventArgs> handler = (sender,args) => {
                callback(sender,(T)args);
            };

            if (_debug) Debug.Log($"Subscribing {callback} to event {eventName}");
            if (!_events.ContainsKey(eventName))
                _events.Add(eventName,handler);
            else
                _events[eventName] += handler;

            _unsubCallbacks.Push(() => { _events[eventName] -= handler; });
            return _unsubCallbacks.Peek();
        }


        /// <summary>
        /// When true event information will be logged to the console
        /// </summary>
        public static bool ToggleDebug() {
            _debug = _debug == true ? false : true;
            return _debug;
        }
        private static bool _debug = false;
    }
}