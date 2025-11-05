using DG.Tweening;
using FactoryContent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WorkerContent;

public class FloatingPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMesh;
    [SerializeField] private Image _icon;
    [SerializeField] private float _moveUpDistance = 1.5f;
    [SerializeField] private float _duration = 1.3f;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private WorkerMovement _workerMovement;
    [SerializeField] private GameObject _popup;

    private Vector3 _startLocalPos;

    private void Awake()
    {
        if (_popup != null)
            _startLocalPos = _popup.transform.localPosition;
    }

    private void OnEnable()
    {
        _workerMovement.ResourcesCollected += ShowCollect;
    }

    private void OnDisable()
    {
        _workerMovement.ResourcesCollected -= ShowCollect;
    }

    private void ShowCollect(Factory factory, int amount)
    {
        _textMesh.text = $"+{amount}";
        _icon.sprite = factory.Data.Produces.Icon;
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        _popup.SetActive(true);
        _popup.transform.localPosition = _startLocalPos;
        _canvasGroup.alpha = 1f;

        Vector3 endLocalPos = _startLocalPos + Vector3.up * _moveUpDistance;

        Sequence seq = DOTween.Sequence();
        seq.Append(_popup.transform.DOLocalMove(endLocalPos, _duration).SetEase(Ease.OutQuad));
        seq.Join(_canvasGroup.DOFade(0, _duration).SetEase(Ease.InQuad));
        seq.OnComplete(() =>
        {
            _popup.SetActive(false);
            _popup.transform.localPosition = _startLocalPos;
        });
    }
}