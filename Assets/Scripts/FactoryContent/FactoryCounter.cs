using Initialization;
using UnityEngine;

namespace FactoryContent
{
    public class FactoryCounter : MonoBehaviour
    {
        [SerializeField] private Factory[] _factories;
        [SerializeField]private Bootstrap _bootstrap;

        private void OnEnable()
        {
            _bootstrap.InitCompleted += Init;
        }

        private void OnDisable()
        {
            _bootstrap.InitCompleted -= Init;
        }

        private void Init()
        {
            foreach (var factory in _factories)
                factory.Init();
        }
    }
}