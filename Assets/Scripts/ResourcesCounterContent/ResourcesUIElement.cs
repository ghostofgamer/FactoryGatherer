using SOContent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ResourcesCounterContent
{
    public class ResourcesUIElement : MonoBehaviour
    {
        private ResourceData _resource;
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private Image _icon;

        public void Init(ResourceData resource)
        {
            _resource = resource;
            _icon.sprite = _resource.Icon;
            ResourcesCounter.Instance.OnResourceChanged += UpdateUI;
            UpdateUI(_resource, ResourcesCounter.Instance.GetAmount(_resource));
        }

        private void UpdateUI(ResourceData res, int amount)
        {
            if (res == _resource)
                _amountText.text = $"{amount}";
        }

        private void OnDestroy()
        {
            if (ResourcesCounter.Instance != null)
                ResourcesCounter.Instance.OnResourceChanged -= UpdateUI;
        }
    }
}