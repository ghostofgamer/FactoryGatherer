using System;
using ResourcesCounterContent;
using SaveContent;
using SOContent;
using UnityEngine;

namespace FactoryContent
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private FactoryConfig _data;
        [SerializeField] private Transform _collectPosition;

        private float _timer;
        private bool _isWork = false;

        public event Action<int, int> ValueChanged;

        public int StoredAmount { get; private set; } = 0;
        public FactoryConfig Data => _data;
        public Transform CollectPosition => _collectPosition;
        public int ID => _id;

        private void Update()
        {
            if (!_isWork)
                return;

            Produce();
        }

        public void Init()
        {
            var data = SaveSystem.GetFactory(_id);

            if (data != null)
            {
                StoredAmount = data.StoredAmount;
                ChangeValue();
            }
            else
            {
                Debug.Log("No factory found");
            }

            _isWork = true;
        }

        private void Produce()
        {
            if (StoredAmount >= _data.StorageLimit)
                return;

            _timer += Time.deltaTime;

            if (_timer >= _data.Duration)
            {
                StoredAmount += _data.ProductionPerSecond;
                StoredAmount = Mathf.Min(StoredAmount, _data.StorageLimit);
                ChangeValue();
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
                ChangeValue();
            }
            else
            {
                Debug.Log($"На фабрике {_data.FactoryName} ничего нет.");
            }
        }

        private void ChangeValue()
        {
            SaveSystem.SaveFactory(this);
            ValueChanged?.Invoke(StoredAmount, _data.StorageLimit);
        }

        public int GetStoredAmount() => StoredAmount;
    }
}