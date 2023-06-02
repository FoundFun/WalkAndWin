using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Views
{
    public class StartView : View
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _createPlayerButton;

        public event UnityAction StartButtonClick;
        public event UnityAction CreateNameButtonClick;

        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartButtonClick);
            _createPlayerButton.onClick.AddListener(OnCreatePlayer);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClick);
            _createPlayerButton.onClick.RemoveListener(OnCreatePlayer);
        }

        public void OpenScreen()
        {
            Open();
        }

        public void CloseScreen()
        {
            Close();
        }

        public void EnableButtonCreatePlayer()
        {
            _createPlayerButton.gameObject.SetActive(true);
            _createPlayerButton.interactable = true;
        }

        public void DisableButtonCreatePlayer()
        {
            _createPlayerButton.gameObject.SetActive(false);
            _createPlayerButton.interactable = false;
        }

        protected override void Open()
        {
            base.Open();
        }

        protected override void Close()
        {
            base.Close();
        }

        private void OnStartButtonClick()
        {
            StartButtonClick?.Invoke();
        }

        private void OnCreatePlayer()
        {
            CreateNameButtonClick?.Invoke();
        }
    }
}