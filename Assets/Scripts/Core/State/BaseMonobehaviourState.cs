using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnake.Core
{
    public abstract class BaseMonobehaviourState<T> : IMonobehaviourState where T: MonoBehaviour, IStateful<IMonobehaviourState>
    {
        protected T _context;
        protected List<Action> Subscriptions = new List<Action>();
        public BaseMonobehaviourState(T context) {
            _context = context;
        }

        protected void UnsubscribeFromAll() {
            foreach (Action unsub in Subscriptions) {
                unsub();
            }
        }
        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void LateUpdate();
        public abstract void TransitionTo();
    }
}
