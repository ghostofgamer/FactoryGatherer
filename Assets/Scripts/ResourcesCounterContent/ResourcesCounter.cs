using System;
using System.Collections.Generic;
using SOContent;
using UnityEngine;

namespace ResourcesCounterContent
{
    public class ResourcesCounter : MonoBehaviour
    {
        public static ResourcesCounter Instance { get; private set; }

        [SerializeField] private ResourceData[] allResources;
        
        private Dictionary<ResourceData, int> _resources = new();

        public event Action<ResourceData, int> OnResourceChanged;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            Init();
        }
        
        public void Init()
        {
            foreach (var res in allResources)
            {
                if (!_resources.ContainsKey(res))
                    _resources[res] = 0;
            }
        }
    
        public void AddResource(ResourceData resource, int amount)
        {
            if (!_resources.ContainsKey(resource))
                _resources[resource] = 0;

            _resources[resource] += amount;
            OnResourceChanged?.Invoke(resource, _resources[resource]);
            Debug.Log($"Добавлено {amount} {resource.ResourceName}. Всего: {_resources[resource]}");
        }
    
        public bool SpendResource(ResourceData resource, int amount)
        {
            if (!_resources.ContainsKey(resource) || _resources[resource] < amount)
                return false;

            _resources[resource] -= amount;
            OnResourceChanged?.Invoke(resource, _resources[resource]);
            Debug.Log($"Потрачено {amount} {resource.ResourceName}. Осталось: {_resources[resource]}");
            return true;
        }
    
        public int GetAmount(ResourceData resource)
        {
            return _resources.ContainsKey(resource) ? _resources[resource] : 0;
        }
        
        public List<ResourceData> GetAllResources()
        {
            return new List<ResourceData>(_resources.Keys);
        }
    }
}