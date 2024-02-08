using System.Collections.Generic;
using Code.Utils.Pooling;
using TMPro;
using UnityEngine;

namespace Code.UI.Controller
{
    public class ComboController : MonoBehaviour
    {
        [SerializeField] private GameObject comboItem;

        private IObjectPoolManager _objectPoolManager;

        public void Awake()
        {
            _objectPoolManager = ObjectPoolManager.Instance;
            _objectPoolManager.CreatePool(comboItem.name, comboItem, 2);
        }

        public void ShowCombo(int comboCount)
        {
            var card = _objectPoolManager.GetObjectFromPool(comboItem.name, Vector3.zero, Quaternion.identity);
            card.transform.SetParent(transform);
            card.transform.localScale = Vector3.one;
            card.transform.localPosition = Vector3.one;
            var cItem = card.GetComponent<ComboItem>();
            cItem.SetData(comboCount);
        }
    }
}