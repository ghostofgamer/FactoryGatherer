using UnityEngine;

namespace SOContent
{
    [CreateAssetMenu(fileName = "NewFactory", menuName = "Game/Factory")]
    public class FactoryConfig : ScriptableObject
    {
        [SerializeField] private string _factoryName;
        [SerializeField] private ResourceData _produces;
        [SerializeField] private int _productionPerSecond = 1;
        [SerializeField] private int _storageLimit = 50;
        [SerializeField] private float _duration;

        public string FactoryName => _factoryName;
        public ResourceData Produces => _produces;
        public int ProductionPerSecond => _productionPerSecond;
        public int StorageLimit => _storageLimit;
        public float Duration => _duration;
    }
}