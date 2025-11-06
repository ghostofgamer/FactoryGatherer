using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputContent
{
    public class PlayerInput : MonoBehaviour
    {
        private const string HorizontalAxis = "Horizontal";
        private const string VerticalAxis = "Vertical";
        private const string ScrollAxis = "Mouse ScrollWheel";
        private const string MouseXAxis = "Mouse X";

        public event Action<Touch> OnTouchBegan;
        public event Action<Touch> OnTouchMoved;
        public event Action<Touch> OnTouchEnded;
        public event Action<Vector2> OnMousePan;
        public event Action<float> OnMouseRotate;
        public event Action<float> OnMouseZoom;
        public event Action<RaycastHit> OnMouseClick;
        public event Action<Vector2> OnKeyboardMove;

        private void Update()
        {
            if (!Application.isMobilePlatform)
            {
                HandleMouse();
                HandleKeyboard();
            }
            else
            {
                HandleTouch();
            }
        }

        private void HandleKeyboard()
        {
            Vector2 move = new Vector2(Input.GetAxis(HorizontalAxis), Input.GetAxis(VerticalAxis));

            if (move.sqrMagnitude > 0.001f)
                OnKeyboardMove?.Invoke(move);
        }

        private void HandleMouse()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            
            if (Mathf.Abs(Input.GetAxis(ScrollAxis)) > 0.001f)
                OnMouseZoom?.Invoke(Input.GetAxis(ScrollAxis));

            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                    OnMouseClick?.Invoke(hit);
            }

            if (Input.GetMouseButton(1))
                OnMouseRotate?.Invoke(Input.GetAxis(MouseXAxis));
        }

        private void HandleTouch()
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        OnTouchBegan?.Invoke(touch);
                        break;
                    case TouchPhase.Moved:
                        OnTouchMoved?.Invoke(touch);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        OnTouchEnded?.Invoke(touch);
                        break;
                }
            }
        }
    }
}