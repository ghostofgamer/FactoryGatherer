using Enums;
using UnityEngine;

namespace SOContent
{
    [CreateAssetMenu(fileName = "NewResource", menuName = "Game/Resource")]
    public class ResourceData : ScriptableObject
    {
        [SerializeField] private string _resourceName;
        [SerializeField] private Sprite _icon;
        [SerializeField]private ResourcesType _resourcesType;

        public string ResourceName => _resourceName;
        public Sprite Icon => _icon;
        public ResourcesType ResourcesType => _resourcesType;
    }
}