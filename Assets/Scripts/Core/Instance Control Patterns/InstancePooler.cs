using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Instance_Control_Patterns
{
    public class InstancePooler<T> {
        private readonly Func<T> _instanceGenerator;
        private readonly ConcurrentBag<T> _instancePool;

        public InstancePooler(Func<T> instanceGnerator) {
            _instanceGenerator = instanceGnerator;
            _instancePool = new ConcurrentBag<T>();
        }
        public T Get() {
            _instancePool.TryTake(out T item);
            if (item != null) return item;
            return _instanceGenerator(); 
        }
        public void Return(T item) => _instancePool.Add(item);
    }
}
