using InputContent;
using UnityEngine;

public class CameraController : MonoBehaviour
{
// @formatter:off
    [SerializeField] private InputHandler _inputHandler;

    [Header("Настройки скорости")] 
    [SerializeField] private float _moveSpeedPC = 10f;
    [SerializeField] private float _moveSpeedMobile = 10f;

    [Header("Зум (Scroll)")] 
    [SerializeField] private float _zoomSpeed = 15f;
    [SerializeField] private float _minY = 5f;
    [SerializeField] private float _maxY = 60f;

    [Header("Вращение камеры")] 
    [SerializeField] private  float _rotationSensitivity = 3;
    
    [Header("Границы карты")]
    [SerializeField] private float _minX = -50f;
    [SerializeField] private float _maxX = 50f;
    [SerializeField] private float _minZ = -50f;
    [SerializeField] private float _maxZ = 50f;
// @formatter:on

    private float _targetSpeed;

    private void OnEnable()
    {
        _inputHandler.OnPan += OnPanInput; // свайп одним пальцем
        _inputHandler.OnRotate += OnRotateInput;
        _inputHandler.OnZoom += HandleZoom;
        _inputHandler.OnMove += HandleKeyboardMove;
    }

    private void OnDisable()
    {
        _inputHandler.OnPan -= OnPanInput;
        _inputHandler.OnRotate -= OnRotateInput;
        _inputHandler.OnZoom -= HandleZoom;
        _inputHandler.OnMove -= HandleKeyboardMove;
    }

    private void Start()
    {
        _targetSpeed = Application.isMobilePlatform ? _moveSpeedMobile : _moveSpeedPC;
    }

    private void HandleKeyboardMove(Vector2 dir)
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * dir.y + right * dir.x;
        float speed = _targetSpeed;
        transform.position += moveDir * speed * Time.deltaTime;

        ClampPosition();
    }

    void HandleZoom(float value)
    {
        if (Mathf.Abs(value) > 0.001f)
        {
            Vector3 pos = transform.position;
            pos.y -= value * _zoomSpeed;
            pos.y = Mathf.Clamp(pos.y, _minY, _maxY);
            transform.position = pos;
        }
    }
    

    private void OnPanInput(Vector2 delta)
    {
        Vector3 move = new Vector3(-delta.x, 0, -delta.y) * _targetSpeed;
        move = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * move;
        transform.position += move;
        ClampPosition();
    }


    private void OnRotateInput(float angleDelta)
    {
        Vector3 euler = transform.rotation.eulerAngles;
        euler.y += angleDelta * _rotationSensitivity;
        transform.rotation = Quaternion.Euler(euler);
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, _minX, _maxX);
        pos.z = Mathf.Clamp(pos.z, _minZ, _maxZ);
        pos.y = Mathf.Clamp(pos.y, _minY, _maxY);
        transform.position = pos;
    }
}