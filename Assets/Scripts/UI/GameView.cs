using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Views
{
    public class GameView : View
    {
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private Button _nextPlayerButton;

        public event UnityAction ExitGameButtonClick;
        public event UnityAction NextPlayerButtonClick;

        private void OnEnable()
        {
            _exitGameButton.onClick.AddListener(OnExitGameButtonClick);
            _nextPlayerButton.onClick.AddListener(OnNextPlayerButtonClick);
        }

        private void OnDisable()
        {
            _exitGameButton.onClick.RemoveListener(OnExitGameButtonClick);
            _nextPlayerButton.onClick.RemoveListener(OnNextPlayerButtonClick);
        }

        public void EnableButtonNextPlayer()
        {
            _nextPlayerButton.gameObject.SetActive(true);
            _nextPlayerButton.interactable = true;
        }

        public void DisableButtonNextPlayer()
        {
            _nextPlayerButton.gameObject.SetActive(false);
            _nextPlayerButton.interactable = false;
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

        protected void OnExitGameButtonClick()
        {
            ExitGameButtonClick?.Invoke();
        }

        private void OnNextPlayerButtonClick()
        {
            NextPlayerButtonClick?.Invoke();
        }
    }
}