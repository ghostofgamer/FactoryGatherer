using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [Header("Настройки скорости")] [SerializeField]
    private float _moveSpeedPC = 10f;

    [SerializeField] private float _moveSpeedMobile = 10f;

    [SerializeField] private float _fastMultiplier = 2f;

    [Header("Зум (Scroll)")] [SerializeField]
    private float _zoomSpeed = 15f;

    [SerializeField] private float _minY = 5f;
    [SerializeField] private float _maxY = 60f;

    [Header("Вращение камеры")] [SerializeField]
    private float _rotationSpeed = 100f;

    private float _rotationSpeedMobile = 10f;

    [Header("Границы карты")] [SerializeField]
    private float _minX = -50f;

    [SerializeField] private float _maxX = 50f;
    [SerializeField] private float _minZ = -50f;
    [SerializeField] private float _maxZ = 50f;

    private float _yaw = 0f;
    private float _pitch = 45f;

    private Vector2 lastTouchPos;
    private bool isDragging = false;

    private float _targetSpeed;

    private Vector3 _panVelocity = Vector3.zero;
    private bool _isPinching = false;

    public float RotationSpeed
    {
        get => _rotationSpeed;
        set => _rotationSpeed = value;
    }

    void Start()
    {
        _targetSpeed = Application.isMobilePlatform ? _moveSpeedMobile : _moveSpeedPC;

        Vector3 angles = transform.eulerAngles;
        _yaw = angles.y;
        _pitch = angles.x;
    }

    void Update()
    {
        _targetSpeed = Application.isMobilePlatform ? _moveSpeedMobile : _moveSpeedPC;
        
        HandleMovement();
        HandleZoom();
        HandleRotation();
        HandleTouchCamera();
        HandleRotationWithTwoFingers();
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
        float speed = Input.GetKey(KeyCode.LeftShift) ? _targetSpeed * _fastMultiplier : _targetSpeed;
        transform.position += direction * speed * Time.deltaTime;

        ClampPosition();
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


    private Vector2 _prevTouch0, _prevTouch1;
    private float _rotationVelocity = 0f;
    private Vector2 _pinchStart0, _pinchStart1; // позиции при первом кадре двух пальцев
    private float _pinchMoveDistance;
    [SerializeField] private TMP_Text pinchDistanceText;
    public float porog = 10f;

    private Vector2 prevTouch0, prevTouch1; // позиции пальцев в предыдущем кадре
    private bool isPinching = false; // активен ли двухпальцевый жест
    [SerializeField] private float rotationSpeed = 0.2f; // чувствительность вращения
    private float yaw = 0f; // угол поворота по Y
    private float pitch = 45f;

    [SerializeField, Tooltip("Слой, который используется как поверхность земли для тача.")]
    private LayerMask groundLayerMask;

    private Vector3 _lastTouchPos;

    public float sensitivity = 2f;

    private Vector2 pinchStart0;
    private Vector2 pinchStart1;
    private float startYaw; // yaw камеры на момент начала пинча
    private float startPitch;
    private Vector2 lastTouch0;
    private Vector2 lastTouch1;

    public float rotationSensitivity = 3;
    private bool rotationActive = false;
    private float minFingerMove = 5f;
    private bool fingersMoved = false;
    
    [SerializeField] private float rotationSmoothTime = 0.1f; // плавность
    [SerializeField] private float rotationDamping = 8f;  
    [SerializeField] private float rotationLerpSpeed = 10f;
    
    private float targetYaw; // куда хотим повернуть
    private float currentYaw;
    
    void HandleRotationWithTwoFingers()
    {
        /*if (Input.touchCount < 2)
        {
            isPinching = false;
            fingersMoved = false;
            return;
        }

        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        if (!isPinching)
        {
            // первый кадр двух пальцев — фиксируем стартовые позиции
            lastTouch0 = touch0.position;
            lastTouch1 = touch1.position;
            isPinching = true;
            fingersMoved = false;
            Debug.Log($"Two fingers touched. Waiting for movement. Positions: {lastTouch0}, {lastTouch1}");
            return; // не крутим пока пальцы не двигаются
        }

        // Проверяем, двигаются ли пальцы
        if (!fingersMoved)
        {
            float move0 = (touch0.position - lastTouch0).magnitude;
            float move1 = (touch1.position - lastTouch1).magnitude;

            if (move0 > 1f || move1 > 1f) // минимальное движение для старта вращения
            {
                fingersMoved = true;
                Debug.Log("Fingers started moving, rotation enabled.");
            }
            else
            {
                // ещё не начали двигаться, ничего не делаем
                lastTouch0 = touch0.position;
                lastTouch1 = touch1.position;
                return;
            }
        }

        // теперь уже считаем угол между текущим и предыдущим кадром
        Vector2 prevDir = lastTouch1 - lastTouch0;
        Vector2 currDir = touch1.position - touch0.position;

        float angle = Vector2.SignedAngle(prevDir, currDir);

        if (Mathf.Abs(angle) > 0.01f)
        {
            // применяем вращение только по Y
            Vector3 euler = transform.rotation.eulerAngles;
            euler.y += angle * rotationSensitivity;
            transform.rotation = Quaternion.Euler(euler.x, euler.y, euler.z);

            Debug.Log($"Rotation applied. Angle delta: {angle}, New Y: {euler.y}");
        }

        // обновляем позиции пальцев для следующего кадра
        lastTouch0 = touch0.position;
        lastTouch1 = touch1.position;*/
        
        
        if (Input.touchCount < 2)
        {
            isPinching = false;
            fingersMoved = false;
            return;
        }

        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        // Первый кадр с двумя пальцами
        if (!isPinching)
        {
            lastTouch0 = touch0.position;
            lastTouch1 = touch1.position;
            isPinching = true;
            fingersMoved = false;

            // фиксируем стартовые значения
            currentYaw = transform.eulerAngles.y;
            targetYaw = currentYaw;

            // Debug.Log("Two fingers touched. Waiting for movement...");
            return;
        }

        // Проверяем, началось ли движение пальцев
        if (!fingersMoved)
        {
            float move0 = (touch0.position - lastTouch0).magnitude;
            float move1 = (touch1.position - lastTouch1).magnitude;

            if (move0 > 5f || move1 > 5f)
            {
                fingersMoved = true;
                // Debug.Log("Fingers started moving, rotation enabled.");
            }
            else
            {
                lastTouch0 = touch0.position;
                lastTouch1 = touch1.position;
                return;
            }
        }

        // Считаем изменение угла
        Vector2 prevDir = lastTouch1 - lastTouch0;
        Vector2 currDir = touch1.position - touch0.position;
        float angle = Vector2.SignedAngle(prevDir, currDir);

        if (Mathf.Abs(angle) > 0.01f)
        {
            targetYaw += angle * rotationSensitivity;
            // Debug.Log($"Target yaw updated: {targetYaw:F2} (Δ {angle:F2})");
        }

        // Плавное приближение через LerpAngle
        currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, Time.deltaTime * rotationLerpSpeed);

        // Применяем
        Vector3 euler = transform.eulerAngles;
        euler.y = currentYaw;
        transform.rotation = Quaternion.Euler(euler);

        // Обновляем позиции
        lastTouch0 = touch0.position;
        lastTouch1 = touch1.position;
    }

    public float panSensitivity = 0.1f;

    void HandleTouchCamera()
    {
        if (Input.touchCount == 2)
        {
            // HandleRotationWithTwoFingers();
            isDragging = false;
            return;
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                // Берём первый объект, на который попал луч
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    // Проверяем слой
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        isDragging = true; // разрешаем движение
                    }
                    else
                    {
                        isDragging = false; // первый объект не земля — движение не начинаем
                    }
                }
                else
                {
                    isDragging = false; // вообще ничего не попало
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector3 move = new Vector3(-touch.deltaPosition.x, 0, -touch.deltaPosition.y) * panSensitivity;
                move = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * move;
                transform.position += move;
                ClampPosition();
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
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