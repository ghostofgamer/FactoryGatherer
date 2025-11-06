using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Screens
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("UI Elements")] [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField] private Image _fillImage;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _fillSmoothSpeed = 3f;

        private float _targetFill = 0f;
        private bool _isWork = false;
        private float _elapsedTime;

        private void Awake()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            _fillImage.fillAmount = 0f;
        }

        private void Update()
        {
            if (!_isWork)
                return;

            _fillImage.fillAmount = Mathf.Lerp(_fillImage.fillAmount, _targetFill, _fillSmoothSpeed * Time.deltaTime);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            _isWork = true;
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            _fillImage.fillAmount = 0f;
        }

        public void SetProgress(float value)
        {
            _targetFill = Mathf.Clamp01(value);
        }

        public IEnumerator FadeOut()
        {
            _elapsedTime = 0f;

            while (_elapsedTime < _fadeDuration)
            {
                _elapsedTime += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(1f, 0f, _elapsedTime / _fadeDuration);
                yield return null;
            }

            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            _isWork = false;
            gameObject.SetActive(false);
        }
    }
}