using UnityEngine;

namespace UI.Buttons
{
    public class CloseScreenButton : AbstractButton
    {
        [SerializeField] private AbstractScreen _screen;

        public override void OnClick()
        {
            if (_screen != null)
                _screen.Close();
        }
    }
}