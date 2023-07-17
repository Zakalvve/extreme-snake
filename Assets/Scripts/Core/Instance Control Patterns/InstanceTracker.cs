using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class InstanceTracker<T> : MonoBehaviour where T: InstanceTracker<T>
    {
        [ContextMenu("Number of Instances")]
        public static void NumInstances() {
            Debug.Log(Instances.Count);
        }
        public static List<T> Instances = new List<T>();
        protected virtual void Awake() {
            Instances.Add(this as T);
        }
        protected virtual void OnDestroy() {
            Instances.Remove(this as T);
        }
    }
}
