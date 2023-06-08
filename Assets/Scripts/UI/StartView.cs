using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Views
{
    public class StartView : View
    {
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _addPlayerButton;
        [SerializeField] private AudioSource _menuAudio;
        [SerializeField] private TMP_Text _previewText;

        private Coroutine _coroutine;
        private Vector3 _startButtonPosition;
        private Vector3 _startPreviewPosition;
        private Vector3 _startAddButtonPosition;

        public event UnityAction StartButtonClick;
        public event UnityAction CreateNameButtonClick;

        private void Awake()
        {
            _menuAudio.volume = 0;
            _startButtonPosition = _startButton.transform.position;
            _startPreviewPosition = _previewText.transform.position;
            _startAddButtonPosition = _addPlayerButton.transform.position;
        }

        private void OnEnable()
        {
            _startButton.onClick.AddListener(OnStartButtonClick);
            _addPlayerButton.onClick.AddListener(OnCreatePlayer);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClick);
            _addPlayerButton.onClick.RemoveListener(OnCreatePlayer);
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
            _addPlayerButton.gameObject.SetActive(true);
            _addPlayerButton.interactable = true;
        }

        public void DisableButtonCreatePlayer()
        {
            _addPlayerButton.gameObject.SetActive(false);
            _addPlayerButton.interactable = false;
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

        private void OnStartButtonClick()
        {
            StartButtonClick?.Invoke();
        }

        private void OnCreatePlayer()
        {
            CreateNameButtonClick?.Invoke();
        }

        private IEnumerator PlayMusic()
        {
            float elapsed = 0;
            float targetValue = 0.2f;
            float lerpDuration = 100;

            _menuAudio.Play();

            while (_menuAudio.volume != targetValue)
            {
                _menuAudio.volume = Mathf.Lerp(_menuAudio.volume, targetValue,
                    elapsed / lerpDuration);

                elapsed += Time.deltaTime;

                yield return null;
            }
        }

        private IEnumerator StopMusic()
        {
            float elapsed = 0;
            float targetValue = 0;
            float lerpDuration = 1000;

            while (_menuAudio.volume != targetValue)
            {
                _menuAudio.volume = Mathf.Lerp(_menuAudio.volume, targetValue,
                    elapsed / lerpDuration);

                elapsed += Time.deltaTime;

                yield return null;
            }

            _menuAudio.Stop();
        }
    }
}