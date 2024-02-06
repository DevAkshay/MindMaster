using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Utils.Pooling
{
    public interface ISimpleObjectPool
    {
        public GameObject GetObject();
        public void ReleaseObject(GameObject obj);
        public void ClearPool();
    }
}
