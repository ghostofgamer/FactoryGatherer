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
        private int _storedAmount = 0;

        public event Action<int, int> ChangeValue;

        public FactoryConfig Data => _data;
        public Transform CollectPosition => _collectPosition;

        private void Update()
        {
            Produce();
        }

        private void Produce()
        {
            if (_storedAmount >= _data.StorageLimit)
                return;

            _timer += Time.deltaTime;

            if (_timer >= 1f)
            {
                _storedAmount += _data.ProductionPerSecond;
                _storedAmount = Mathf.Min(_storedAmount, _data.StorageLimit);
                ChangeValue?.Invoke(_storedAmount, _data.StorageLimit);
                _timer = 0f;
            }
        }

        [ContextMenu("Collect")]
        public void Collect()
        {
            if (_storedAmount > 0)
            {
                ResourcesCounter.Instance.AddResource(_data.Produces, _storedAmount);
                Debug.Log($"Собрано {_storedAmount} {_data.Produces.ResourceName} с фабрики {_data.FactoryName}");
                _storedAmount = 0;
                ChangeValue?.Invoke(_storedAmount, _data.StorageLimit);
            }
            else
            {
                Debug.Log($"На фабрике {_data.FactoryName} ничего нет.");
            }
        }

        public int GetStoredAmount() => _storedAmount;
    }
}