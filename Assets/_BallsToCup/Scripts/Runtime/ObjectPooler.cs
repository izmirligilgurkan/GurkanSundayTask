using System.Collections.Generic;
using System.Linq;
using _BallsToCup.Scripts.Runtime.Patterns;
using _BallsToCup.Scripts.Runtime.ScriptableObjects;
using UnityEngine;

namespace _BallsToCup.Scripts.Runtime
{
    public class ObjectPooler : MonoBehaviourSingleton<ObjectPooler>
    {
        private static readonly List<GameObject> pooledObjects = new List<GameObject>();
        [SerializeField] private GameObject pooledObjectPrefab;
        [SerializeField] private int poolAmount = 100;

        private void OnEnable()
        {
            for (var i = 0; i < poolAmount; i++)
            {
                var instance = Instantiate(pooledObjectPrefab);
                instance.SetActive(false);
                pooledObjects.Add(instance);
            }

            LevelManager.OnLevelLoadStart += OnLevelLoadStart;
        }

        private void OnDisable()
        {
            LevelManager.OnLevelLoadStart -= OnLevelLoadStart;
        }

        private void OnLevelLoadStart(Level level)
        {
            foreach (var pooledObject in pooledObjects) pooledObject.SetActive(false);
        }

        public static GameObject GetPooledObject()
        {
            var firstOrDefault = pooledObjects.FirstOrDefault(o => o && !o.activeInHierarchy);
            return firstOrDefault ? firstOrDefault : Instantiate(Instance.pooledObjectPrefab);
        }
    }
}