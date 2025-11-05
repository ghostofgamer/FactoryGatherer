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
        }

        private void OnDisable()
        {
            _factory.ValueChanged -= ShowInfo;
        }

        private void Start()
        {
            _icon.sprite = _factory.Data.Produces.Icon;
            ShowInfo(_factory.GetStoredAmount(), _factory.Data.StorageLimit);
        }

        private void ShowInfo(int currentAmount, int totalAmount)
        {
            _storedText.text = $"{_factory.Data.Produces.ResourceName}: {currentAmount} / {totalAmount}";
        }
    }
}