using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Настройки скорости")] [SerializeField]
    private float _moveSpeed = 10f;

    [SerializeField] private float _fastMultiplier = 2f;

    [Header("Зум (Scroll)")] [SerializeField]
    private float _zoomSpeed = 15f;

    [SerializeField] private float _minY = 5f;
    [SerializeField] private float _maxY = 60f;

    [Header("Вращение камеры")] [SerializeField]
    private float _rotationSpeed = 100f;

    [Header("Границы карты")] [SerializeField]
    private float _minX = -50f;

    [SerializeField] private float _maxX = 50f;
    [SerializeField] private float _minZ = -50f;
    [SerializeField] private float _maxZ = 50f;

    private float _yaw = 0f;
    private float _pitch = 45f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleRotation();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // A/D
        float moveZ = Input.GetAxis("Vertical"); // W/S

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 direction = forward * moveZ + right * moveX;
        float speed = Input.GetKey(KeyCode.LeftShift) ? _moveSpeed * _fastMultiplier : _moveSpeed;
        transform.position += direction * speed * Time.deltaTime;

        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, _minY, _maxY);
        pos.x = Mathf.Clamp(pos.x, _minX, _maxX);
        pos.z = Mathf.Clamp(pos.z, _minZ, _maxZ);
        transform.position = pos;
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.001f)
        {
            Vector3 pos = transform.position;
            pos.y -= scroll * _zoomSpeed;
            pos.y = Mathf.Clamp(pos.y, _minY, _maxY);
            transform.position = pos;
        }
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            _yaw += mouseX * _rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }
    }
}