using System;
using ResourcesCounterContent;
using SOContent;
using UnityEngine;

namespace FactoryContent
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private FactoryConfig _data;
        [SerializeField] private Transform _collectPosition;

        private float _timer;
        
        public event Action<int, int> ChangeValue;

        public int StoredAmount { get; private set; } = 0;
        public FactoryConfig Data => _data;
        public Transform CollectPosition => _collectPosition;

        private void Update()
        {
            Produce();
        }

        private void Produce()
        {
            if (StoredAmount >= _data.StorageLimit)
                return;

            _timer += Time.deltaTime;

            if (_timer >= 1f)
            {
                StoredAmount += _data.ProductionPerSecond;
                StoredAmount = Mathf.Min(StoredAmount, _data.StorageLimit);
                ChangeValue?.Invoke(StoredAmount, _data.StorageLimit);
                _timer = 0f;
            }
        }

        [ContextMenu("Collect")]
        public void Collect()
        {
            if (StoredAmount > 0)
            {
                ResourcesCounter.Instance.AddResource(_data.Produces, StoredAmount);
                Debug.Log($"Собрано {StoredAmount} {_data.Produces.ResourceName} с фабрики {_data.FactoryName}");
                StoredAmount = 0;
                ChangeValue?.Invoke(StoredAmount, _data.StorageLimit);
            }
            else
            {
                Debug.Log($"На фабрике {_data.FactoryName} ничего нет.");
            }
        }

        public int GetStoredAmount() => StoredAmount;
    }
}