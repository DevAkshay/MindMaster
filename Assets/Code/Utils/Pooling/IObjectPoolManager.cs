using UnityEngine;

namespace Code.Utils.Pooling
{
    public interface IObjectPoolManager
    {
        public void CreatePool(string poolName, GameObject prefab, int initialSize);
        public GameObject GetObjectFromPool(string poolName);
        public void ReleaseObjectToPool(string poolName, GameObject obj);
        public void ClearAllPools();
    }
}
