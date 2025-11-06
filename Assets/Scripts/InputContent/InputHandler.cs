using System;
using UnityEngine;

namespace InputContent
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private float _tapThreshold = 20f;
        [SerializeField] private float _rotationSensitivity = 1f;
        [SerializeField] private float _zoomSensitivity = 0.1f;

        private Vector2 _touchStartPos;
        private Vector2 _lastTouch0, _lastTouch1;
        private bool _isPanning = false;
        private bool _isRotating = false;

        public event Action<Vector2> OnPan;
        public event Action<float> OnRotate;
        public event Action<float> OnZoom;
        public event Action<RaycastHit> OnTap;
        public event Action<Vector2> OnMove;

        private void OnEnable()
        {
            _playerInput.OnTouchBegan += TouchBegan;
            _playerInput.OnTouchMoved += TouchMoved;
            _playerInput.OnTouchEnded += TouchEnded;
            _playerInput.OnMousePan += MousePan;
            _playerInput.OnMouseRotate += MouseRotate;
            _playerInput.OnMouseZoom += MouseZoom;
            _playerInput.OnMouseClick += MouseClick;
            _playerInput.OnKeyboardMove += KeyboardMove;
        }

        private void OnDisable()
        {
            _playerInput.OnTouchBegan -= TouchBegan;
            _playerInput.OnTouchMoved -= TouchMoved;
            _playerInput.OnTouchEnded -= TouchEnded;
            _playerInput.OnMousePan -= MousePan;
            _playerInput.OnMouseRotate -= MouseRotate;
            _playerInput.OnMouseZoom -= MouseZoom;
            _playerInput.OnMouseClick -= MouseClick;
            _playerInput.OnKeyboardMove -= KeyboardMove;
        }

        private void TouchBegan(Touch touch)
        {
            switch (Input.touchCount)
            {
                case 1:
                    _touchStartPos = touch.position;
                    _isPanning = false;
                    break;
                case 2:
                    _lastTouch0 = Input.GetTouch(0).position;
                    _lastTouch1 = Input.GetTouch(1).position;
                    _isRotating = true;
                    break;
            }
        }

        private void TouchMoved(Touch touch)
        {
            if (Input.touchCount == 1)
            {
                float moveDist = (touch.position - _touchStartPos).magnitude;

                if (moveDist > _tapThreshold)
                {
                    _isPanning = true;
                    OnPan?.Invoke(touch.deltaPosition);
                }
            }
            else if (Input.touchCount == 2)
            {
                Vector2 prevDir = _lastTouch1 - _lastTouch0;
                Vector2 currDir = Input.GetTouch(1).position - Input.GetTouch(0).position;
                OnRotate?.Invoke(Vector2.SignedAngle(prevDir, currDir) * _rotationSensitivity);
                _lastTouch0 = Input.GetTouch(0).position;
                _lastTouch1 = Input.GetTouch(1).position;
            }
        }

        private void TouchEnded(Touch touch)
        {
            if (Input.touchCount == 1 && !_isPanning && !_isRotating)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                    OnTap?.Invoke(hit);
            }

            if (Input.touchCount <= 2)
            {
                _isRotating = false;
                _isPanning = false;
            }
        }

        #region Mouse Handlers

        private void MousePan(Vector2 delta) => OnPan?.Invoke(delta);

        private void MouseRotate(float deltaX) => OnRotate?.Invoke(deltaX * _rotationSensitivity);

        private void MouseZoom(float delta) => OnZoom?.Invoke(delta * _zoomSensitivity);

        private void MouseClick(RaycastHit hit) => OnTap?.Invoke(hit);

        #endregion

        #region Keyboard Handler

        private void KeyboardMove(Vector2 direction) => OnMove?.Invoke(direction);

        #endregion
    }
}