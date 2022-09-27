using System;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime.Patterns
{
    /// <summary>
    /// Generic lazy MonoBehaviour singleton thread-safe.
    /// </summary>
    /// <typeparam name="T">Singleton type</typeparam>
    public abstract class MonoBehaviourSingleton<T> : BaseMonoBehaviour where T : BaseMonoBehaviour
    {
        /// <summary>Instance.</summary>
        public static T Instance => lazy.Value;

        /// <summary>Instance created?</summary>
        public static bool IsCreated => lazy.IsValueCreated;

        private static readonly Lazy<T> lazy = new Lazy<T>(() =>
        {
            T instance = FindObjectOfType<T>(true);
            if (instance == null)
            {
                GameObject ownerObject = new GameObject(typeof(T).Name);
                instance = ownerObject.AddComponent<T>();
            }

            return instance;
        });
    }
}