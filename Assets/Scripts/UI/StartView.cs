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
        [SerializeField] private Button _createPlayerButton;
        [SerializeField] private AudioSource _menuAudio;
        [SerializeField] private TMP_Text _previewText;

        private Coroutine _coroutine;

        public event UnityAction StartButtonClick;
        public event UnityAction CreateNameButtonClick;

        private void Awake()
        {
            _menuAudio.volume = 0;
        }

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
            float heightRange = 300;
            float buttonPositionY = 370;
            float previewPositionY = 370;

            base.Open();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(PlayMusic());

            _previewText.gameObject.transform.position =
                new Vector2(_previewText.transform.position.x, Screen.height + heightRange);
            _startButton.gameObject.transform.position =
                new Vector2(_startButton.transform.position.x, -Screen.height);
            _createPlayerButton.gameObject.transform.position
                = new Vector2(_createPlayerButton.transform.position.x, -Screen.height);

            _previewText.gameObject.LeanScale(Vector3.one, 0.7f);
            _previewText.gameObject.LeanMoveLocalY(previewPositionY, 1.2f)
                .setEaseInOutExpo().setEaseInOutBack();
            _startButton.gameObject.LeanMoveLocalY(-buttonPositionY, 2)
                .setEaseInOutExpo().setEaseInOutBack();
            _createPlayerButton.gameObject.LeanMoveLocalY(-buttonPositionY, 2.4f)
                .setEaseInOutExpo().setEaseInOutBack();
        }

        protected override void Close()
        {
            base.Close();

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StopMusic());

            _previewText.gameObject.LeanScale(Vector3.zero, 0.2f);
            _previewText.gameObject.LeanMoveLocalY(Screen.height, 0.15f)
                .setEaseInOutBack();
            _startButton.gameObject.LeanMoveLocalY(-Screen.height, 0.15f)
                .setEaseInOutBack();
            _createPlayerButton.gameObject.LeanMoveLocalY(-Screen.height, 0.15f)
                .setEaseInOutBack();
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