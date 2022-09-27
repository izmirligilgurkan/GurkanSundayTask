using System;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime.ScriptableObjects
{
    public abstract class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        /// <summary>Instance.</summary>
        public static T Instance => lazy.Value;

        [NonSerialized]
        private static readonly Lazy<T> lazy = new Lazy<T>(() =>
        {
            string name = typeof(T).Name;
      
            T instance = Resources.Load<T>(name);
            if (instance == null)
                Debug.Log($"No instance of '{name}' found in the Resources folder. Create one inside the Resources folder, and name the file '{name}'");
      
            return instance;
        });
    }
}