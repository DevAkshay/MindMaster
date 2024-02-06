using System.Collections.Generic;
using UnityEngine;

namespace Code.Utils.Pooling
{
    public class ObjectPoolManager : GenericSingleton<ObjectPoolManager>, IObjectPoolManager
    {
        private Dictionary<string, SimpleObjectPool> _poolDictionary = new Dictionary<string, SimpleObjectPool>();

        public void CreatePool(string poolName, GameObject prefab, int initialSize)
        {
            if (!_poolDictionary.ContainsKey(poolName))
            {
                SimpleObjectPool pool = new SimpleObjectPool(prefab, initialSize);
                _poolDictionary.Add(poolName, pool);
            }
            else
            {
                Debug.LogWarning("Pool with name " + poolName + " already exists.");
            }
        }

        public GameObject GetObjectFromPool(string poolName)
        {
            if (_poolDictionary.ContainsKey(poolName))
            {
                return _poolDictionary[poolName].GetObject();
            }
            else
            {
                Debug.LogWarning("Pool with name " + poolName + " does not exist.");
                return null;
            }
        }

        public void ReleaseObjectToPool(string poolName, GameObject obj)
        {
            if (_poolDictionary.ContainsKey(poolName))
            {
                _poolDictionary[poolName].ReleaseObject(obj);
            }
            else
            {
                Debug.LogWarning("Pool with name " + poolName + " does not exist.");
            }
        }

        public void ClearAllPools()
        {
            foreach (var pool in _poolDictionary.Values)
            {
                pool.ClearPool();
            }

            _poolDictionary.Clear();
        }
    }
}
