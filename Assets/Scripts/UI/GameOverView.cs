using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Views
{
    public class GameOverView : View
    {
        [SerializeField] private Button _ExitMenuButton;

        public event UnityAction ExitMenuButtonClick;

        private void OnEnable()
        {
            _ExitMenuButton.onClick.AddListener(OnExitMenuButtonClick);
        }

        private void OnDisable()
        {
            _ExitMenuButton.onClick.RemoveListener(OnExitMenuButtonClick);
        }

        public void OpenScreen()
        {
            Open();
        }

        public void CloseScreen()
        {
            Close();
        }

        protected override void Open()
        {
            base.Open();
        }

        protected override void Close()
        {
            base.Close();
        }

        protected void OnExitMenuButtonClick()
        {
            ExitMenuButtonClick?.Invoke();
        }
    }
}