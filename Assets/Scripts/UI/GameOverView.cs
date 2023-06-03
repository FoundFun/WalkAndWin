using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Views
{
    public class GameOverView : View
    {
        [SerializeField] private Button _exitMenuButton;
        [SerializeField] private GameStatistic _gameStatistic;

        public event UnityAction ExitMenuButtonClick;

        private void OnEnable()
        {
            _exitMenuButton.onClick.AddListener(OnExitMenuButtonClick);
        }

        private void OnDisable()
        {
            _exitMenuButton.onClick.RemoveListener(OnExitMenuButtonClick);
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
            float buttonPositionY = 415;

            base.Open();

            _gameStatistic.gameObject.transform.position =
                   new Vector2(_gameStatistic.transform.position.x, -Screen.height);
            _exitMenuButton.gameObject.transform.position =
                new Vector2(_exitMenuButton.transform.position.x, -Screen.height);

            _gameStatistic.gameObject.LeanScale(Vector3.one, 1);
            _gameStatistic.gameObject.LeanMoveLocalY(0, 2)
                .setEaseInOutExpo().setEaseInOutBack();
            _exitMenuButton.gameObject.LeanMoveLocalY(-buttonPositionY, 2.4f)
                .setEaseInOutExpo().setEaseInOutBack();
        }

        protected override void Close()
        {
            base.Close();

            _gameStatistic.gameObject.LeanScale(Vector3.zero, 1);
            _gameStatistic.gameObject.LeanMoveLocalY(-Screen.height, 2)
                .setEaseInOutExpo().setEaseInOutBack();
            _exitMenuButton.gameObject.LeanMoveLocalY(-Screen.height, 2.4f)
                .setEaseInOutExpo().setEaseInOutBack();
        }

        protected void OnExitMenuButtonClick()
        {
            ExitMenuButtonClick?.Invoke();
        }
    }
}