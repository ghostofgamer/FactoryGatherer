using UnityEngine;

namespace WorkerContent
{
    public class WorkerAnimation : MonoBehaviour
    {
        private const string Collect = "Collect";
        private const string Speed = "Speed";

        [SerializeField] private Animator _animator;


        public void PlayCollect(bool value)
        {
            _animator.SetBool(Collect, value);
        }

        public void PlayMove(float value)
        {
            float current = _animator.GetFloat(Speed);
            
            if (!Mathf.Approximately(current, value))
                _animator.SetFloat(Speed, value);
        }

        public float GetSpeed()
        {
            return _animator.GetFloat(Speed);
        }
    }
}