using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FactoryContent
{
    public class FactoryViewer : MonoBehaviour
    {
        [SerializeField] private Factory _factory;
        [SerializeField] private TMP_Text _storedText;
        [SerializeField] private Image _icon;

        private void OnEnable()
        {
            _factory.ValueChanged += ShowInfo;
            _factory.Initialized += Init;
        }

        private void OnDisable()
        {
            _factory.ValueChanged -= ShowInfo;
            _factory.Initialized -= Init;
        }

        private void Init()
        {
            _icon.sprite = _factory.Data.Produces.Icon;
        }

        private void ShowInfo(int currentAmount, int totalAmount)
        {
            _storedText.text = $"{_factory.Data.Produces.ResourceName}: {currentAmount} / {totalAmount}";
        }
    }
}