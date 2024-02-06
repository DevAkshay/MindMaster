using System.Collections.Generic;
using UnityEngine;

namespace Code.Utils.Pooling
{
    public class ObjectPoolManager : GenericSingleton<ObjectPoolManager>, IObjectPoolManager
    {
        private readonly Dictionary<string, SimpleObjectPool> _poolDictionary = new();

        public void CreatePool(string poolName, GameObject prefab, int initialSize)
        {
            if (!_poolDictionary.ContainsKey(poolName))
            {
                var pool = new SimpleObjectPool(prefab, initialSize, transform);
                _poolDictionary.Add(poolName, pool);
            }
            else
            {
                Debug.LogWarning("Pool with name " + poolName + " already exists.");
            }
        }

        public GameObject GetObjectFromPool(string poolName, Vector3 position, Quaternion rotation)
        {
            if (_poolDictionary.ContainsKey(poolName))
            {
                var poolObject = _poolDictionary[poolName].GetObject();
                poolObject.transform.position = position;
                poolObject.transform.rotation = rotation;
                return poolObject;
            }

            Debug.LogWarning("Pool with name " + poolName + " does not exist.");
            return null;
        }

        public void ReleaseObjectToPool(string poolName, GameObject obj)
        {
            if (_poolDictionary.ContainsKey(poolName))
                _poolDictionary[poolName].ReleaseObject(obj);
            else
                Debug.LogWarning("Pool with name " + poolName + " does not exist.");
        }

        public void ClearAllPools()
        {
            foreach (var pool in _poolDictionary.Values) pool.ClearPool();

            _poolDictionary.Clear();
        }
    }
}