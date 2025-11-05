using System.Collections.Generic;
using SOContent;
using UnityEngine;

namespace ResourcesCounterContent
{
    public class ResourceHUD : MonoBehaviour
    {
        [Header("UI контейнер и префаб")]
        [SerializeField] private Transform _container;       
        [SerializeField] private ResourcesUIElement _resourcesUIElementPrefab;

        public void Init()
        {
            ResourcesCounter.Instance.Init();
            List<ResourceData> resources = ResourcesCounter.Instance.GetAllResources();

            foreach (var resource in resources)
            {
                ResourcesUIElement item = Instantiate(_resourcesUIElementPrefab, _container);
                item.Init(resource);
            }
        }
    }
}