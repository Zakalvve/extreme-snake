﻿using UnityEngine;

namespace ExtremeSnake.Core
{
    public abstract class Singleton<T>: MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake() {
            if (Instance != null && Instance != this as T) {
                Destroy(this);
                return;
            }
            Instance = this as T;
            DontDestroyOnLoad(this);
        }
    }
}
