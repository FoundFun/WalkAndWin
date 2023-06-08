using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Views
{
    public class GameView : View
    {
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private Button _nextPlayerButton;
        [SerializeField] private AudioSource _gameAudio;

        private Coroutine _coroutine;

        public event UnityAction ExitGameButtonClick;
        public event UnityAction NextPlayerButtonClick;

        private void Awake()
        {
            _gameAudio.volume = 0;
        }

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

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(PlayMusic());
        }

        protected override void Close()
        {
            base.Close();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StopMusic());
        }

        protected void OnExitGameButtonClick()
        {
            ExitGameButtonClick?.Invoke();
        }

        private void OnNextPlayerButtonClick()
        {
            NextPlayerButtonClick?.Invoke();
        }

        private IEnumerator PlayMusic()
        {
            float elapsed = 0;
            float targetValue = 0.2f;
            float lerpDuration = 1000;

            _gameAudio.Play();

            while (_gameAudio.volume != targetValue)
            {
                _gameAudio.volume = Mathf.Lerp(_gameAudio.volume, targetValue,
                    elapsed / lerpDuration);
                elapsed += Time.deltaTime;

                yield return null;
            }
        }

        private IEnumerator StopMusic()
        {
            float elapsed = 0;
            float targetValue = 0;
            float lerpDuration = 500;

            while (_gameAudio.volume != targetValue)
            {
                _gameAudio.volume = Mathf.Lerp(_gameAudio.volume, targetValue,
                    elapsed / lerpDuration);
                elapsed += Time.deltaTime;

                yield return null;
            }

            _gameAudio.Stop();
        }
    }
}