using System;
using System.Collections;
using FactoryContent;
using InputContent;
using UnityEngine;
using UnityEngine.AI;

namespace WorkerContent
{
    public class WorkerMovement : MonoBehaviour
    {
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private WorkerAnimation _workerAnimation;
        [SerializeField] private float _speedSensiv = 5f;

        private NavMeshAgent _agent;
        private Coroutine _movementCoroutine;
        private Coroutine _collectCoroutine;
        private float _currentSpeed;
        private float _animatorSpeed = 0f;
        private bool _isCollecting = false;
        private WaitForSeconds _collectWait = new WaitForSeconds(5f);

        public event Action<Factory, int> ResourcesCollected;

        private void OnEnable()
        {
            _inputHandler.OnTap += HandleTap;
        }

        private void OnDisable()
        {
            _inputHandler.OnTap -= HandleTap;
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (_isCollecting)
                return;

            _animatorSpeed = Mathf.MoveTowards(_animatorSpeed, _currentSpeed, Time.deltaTime * _speedSensiv);

            if (Mathf.Abs(_animatorSpeed - _workerAnimation.GetSpeed()) > 0.01f)
                _workerAnimation.PlayMove(_animatorSpeed);
        }

        private void HandleTap(RaycastHit hit)
        {
            if (_isCollecting)
                return;

            if (hit.collider.TryGetComponent(out Ground ground))
                MoveWorkerTo(hit.point, () => Debug.Log("Добрался до точки"));

            if (hit.collider.TryGetComponent(out Factory factory))
            {
                MoveWorkerTo(factory.CollectPosition.position, () =>
                {
                    if (_collectCoroutine != null)
                        StopCoroutine(_collectCoroutine);

                    _collectCoroutine = StartCoroutine(Collect(factory));
                });
            }
        }

        private void MoveWorkerTo(Vector3 targetPoint, Action onFinish = null)
        {
            if (_agent == null)
                return;

            if (_movementCoroutine != null)
                StopCoroutine(_movementCoroutine);

            _agent.SetDestination(targetPoint);
            _movementCoroutine = StartCoroutine(WorkerMovementCoroutine(onFinish));
        }

        private IEnumerator WorkerMovementCoroutine(Action onFinish)
        {
            _currentSpeed = 1;

            while (_agent.pathPending || _agent.remainingDistance > _agent.stoppingDistance)
            {
                yield return null;
            }

            _currentSpeed = 0;
            onFinish?.Invoke();
            _movementCoroutine = null;
        }

        private IEnumerator Collect(Factory factory)
        {
            _isCollecting = true;
            _workerAnimation.PlayCollect(true);
            yield return _collectWait;
            _workerAnimation.PlayCollect(false);
            ResourcesCollected?.Invoke(factory, factory.StoredAmount);
            factory.Collect();
            _isCollecting = false;
        }
    }
}