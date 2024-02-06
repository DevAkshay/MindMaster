using System.Collections.Generic;
using UnityEngine;

namespace Code.Utils.Pooling
{
    public class SimpleObjectPool : ISimpleObjectPool
    {
        private GameObject _prefab;
        private List<GameObject> _poolList;

        public SimpleObjectPool(GameObject prefab, int initialSize)
        {
            this._prefab = prefab;
            this._poolList = new List<GameObject>(initialSize);

            InitializePool(initialSize);
        }

        private void InitializePool(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
            {
                CreateObject();
            }
        }

        private GameObject CreateObject()
        {
            GameObject obj = Object.Instantiate(_prefab);
            obj.SetActive(false);
            _poolList.Add(obj);
            return obj;
        }

        public GameObject GetObject()
        {
            GameObject obj = null;

            // Check if there's an inactive object in the pool
            for (int i = 0; i < _poolList.Count; i++)
            {
                if (!_poolList[i].activeInHierarchy)
                {
                    obj = _poolList[i];
                    break;
                }
            }

            // If no inactive object found, create a new one
            if (obj == null)
            {
                obj = CreateObject();
            }

            obj.SetActive(true);
            return obj;
        }

        public void ReleaseObject(GameObject obj)
        {
            obj.SetActive(false);
        }

        public void ClearPool()
        {
            for (int i = 0; i < _poolList.Count; i++)
            {
                Object.Destroy(_poolList[i]);
            }

            _poolList.Clear();
        }
    }
}
