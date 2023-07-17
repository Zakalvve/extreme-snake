using UnityEngine;

namespace ExtremeSnake.Core
{
    public abstract class BaseMonobehaviourState<T> : IMonobehaviourState where T: MonoBehaviour, IStateful<IMonobehaviourState>
    {
        protected T _context;
        public BaseMonobehaviourState(T context) {
            _context = context;
        }

        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void LateUpdate();
        public abstract void TransitionTo();
    }
}
