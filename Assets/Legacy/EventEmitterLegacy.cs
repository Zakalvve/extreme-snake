//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.Legacy
//{

//    public abstract class BaseEventEmitterLegacy<T>
//    {
//        protected Dictionary<string,List<T>> subscribers;

//        public BaseEventEmitterLegacy() {
//            subscribers = new Dictionary<string,List<T>>();
//        }

//        public Action Subscribe(string eventName,T callback) {
//            if (!subscribers.ContainsKey(eventName)) subscribers.Add(eventName,new List<T>());
//            subscribers[eventName].Add(callback);
//            return () => {
//                subscribers[eventName].Remove(callback);
//            };
//        }
//    }

//    public class EventEmitterLegacy : BaseEventEmitterLegacy<Action>
//    {
//        public void Emit(string eventName) {
//            if (!subscribers.ContainsKey(eventName)) return;
//            foreach (var callback in subscribers[eventName]) {
//                callback();
//            }
//        }
//    }

//    public class EventEmitterLegacy<T> : BaseEventEmitterLegacy<Action<T>>
//    {
//        public void Emit(string eventName,T args) {
//            if (!subscribers.ContainsKey(eventName)) return;
//            foreach (var callback in subscribers[eventName]) {
//                callback(args);
//            }
//        }
//    }
//}
