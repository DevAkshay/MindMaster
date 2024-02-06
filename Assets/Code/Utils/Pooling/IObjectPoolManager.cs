using UnityEngine;

namespace Code.Utils.Pooling
{
    public interface IObjectPoolManager
    {
        public void CreatePool(string poolName, GameObject prefab, int initialSize);
        public GameObject GetObjectFromPool(string poolName, Vector3 position, Quaternion rotation);
        public void ReleaseObjectToPool(string poolName, GameObject obj);
        public void ClearAllPools();
    }
}
