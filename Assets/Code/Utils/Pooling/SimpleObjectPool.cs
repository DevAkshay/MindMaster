using System.Collections.Generic;
using UnityEngine;

namespace Code.Utils.Pooling
{
    public class SimpleObjectPool : ISimpleObjectPool
    {
        private readonly GameObject _prefab;
        private readonly List<GameObject> _poolList;
        private readonly Transform _parent;

        public SimpleObjectPool(GameObject prefab, int initialSize, Transform parent)
        {
            _prefab = prefab;
            _poolList = new List<GameObject>(initialSize);
            _parent = parent;

            InitializePool(initialSize);
        }

        private void InitializePool(int initialSize)
        {
            for (var i = 0; i < initialSize; i++)
            {
                CreateObject();
            }
        }

        private GameObject CreateObject()
        {
            var obj = Object.Instantiate(_prefab, _parent);
            var poolObject = obj.GetComponent<IPoolObject>();
            if (poolObject != null)
            {
                poolObject.Pool = this;
            }

            obj.SetActive(false);
            _poolList.Add(obj);
            return obj;
        }

        public GameObject GetObject()
        {
            GameObject obj = null;

            // Check if there's an inactive object in the pool
            for (var i = 0; i < _poolList.Count; i++)
                if (!_poolList[i].activeInHierarchy)
                {
                    obj = _poolList[i];
                    break;
                }

            // If no inactive object found, create a new one
            if (obj == null) obj = CreateObject();

            var poolObject = obj.GetComponent<IPoolObject>();
            if (poolObject != null)
            {
                poolObject.OnObjectInit();
            }
            obj.SetActive(true);
            return obj;
        }

        public void ReleaseObject(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(_parent);
        }

        public void ClearPool()
        {
            for (var i = 0; i < _poolList.Count; i++) Object.Destroy(_poolList[i]);

            _poolList.Clear();
        }
    }
}