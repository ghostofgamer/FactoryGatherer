using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Настройки скорости")] public float moveSpeed = 10f;
    public float fastMultiplier = 2f;

    [Header("Зум (Scroll)")]
    public float zoomSpeed = 15f;
    public float minY = 5f;
    public float maxY = 60f;
    
    [Header("Вращение камеры")]
    public float rotationSpeed = 100f;
    public float minPitch = 20f;  // минимальный угол наклона (X)
    public float maxPitch = 80f;  // максимальный угол наклона (X)

    private float yaw = 0f;   // вращение вокруг Y
    private float pitch = 45f;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleRotation();
    }

    void HandleMovement()
    {
        // Ввод
        float moveX = Input.GetAxis("Horizontal"); // A/D
        float moveZ = Input.GetAxis("Vertical");   // W/S

        // Направление относительно камеры
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Игнорируем вертикаль
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 direction = forward * moveZ + right * moveX;

        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * fastMultiplier : moveSpeed;
        transform.position += direction * speed * Time.deltaTime;

        // Ограничение по высоте
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.001f)
        {
            Vector3 pos = transform.position;
            pos.y -= scroll * zoomSpeed;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            transform.position = pos;
        }
    }

    void HandleRotation()
    {
        // Вращение при зажатой ПКМ (Right Mouse Button)
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * rotationSpeed * Time.deltaTime;
            pitch -= mouseY * rotationSpeed * Time.deltaTime;

            // Ограничиваем наклон по X
             pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}