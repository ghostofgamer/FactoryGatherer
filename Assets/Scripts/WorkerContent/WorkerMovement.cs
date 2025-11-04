using UnityEngine;
using UnityEngine.AI;

namespace WorkerContent
{
    public class WorkerMovement : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundMask;
        [SerializeField]private Animator _animator;

        private NavMeshAgent _agent;

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            // ПК-клик (для теста в редакторе)
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Click");
                MoveToMouseClick();
            }


            // // На телефоне (тач)
            // if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            // {
            //     MoveToTouch(Input.GetTouch(0).position);
            // }
            
            HandleWorkerMovementAnimation();
        }

        void MoveToMouseClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Рисуем луч для отладки в Scene View (1000 единиц)
            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 2f);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.TryGetComponent(out Ground ground))
                {
                    _agent.SetDestination(hit.point);
                    Debug.Log($"Попал в зкемлю: {hit.collider.name} at {hit.point}");
                }
                    
                Debug.Log($"Ray hit: {hit.collider.name} at {hit.point}");
            }
            else
            {
                Debug.Log("Raycast did NOT hit anything");
            }
        }

        void MoveToTouch(Vector2 touchPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, _groundMask))
            {
                _agent.SetDestination(hit.point);
                Debug.Log($"Moving to: {hit.point}");
            }
        }
        
        void HandleWorkerMovementAnimation()
        {
            if (_agent == null || _animator == null)
                return;
            
            float speed = 0f;

            if (_agent.pathPending == false && _agent.remainingDistance > _agent.stoppingDistance)
            { 
                speed = 1f; 
            }
            else
            {
                // Агент достиг цели
                speed = 0f;
            }

            _animator.SetFloat("Speed", speed,0.1f, Time.deltaTime);
        }
    }
}