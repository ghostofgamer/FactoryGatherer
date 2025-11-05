using UnityEngine;

namespace UI
{
    public abstract class AbstractScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _screen;

        public void Open()
        {
            _screen.SetActive(true);
        }

        public void Close()
        {
            _screen.SetActive(false);
        }
    }
}