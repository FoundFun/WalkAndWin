using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Platforms;
using TMPro;

namespace Players
{
    [RequireComponent(typeof(Animator))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private const float DelayWalk = 0.5f;
        private const int StartIndexPlatform = 0;
        private const string EmptyName = "";

        private readonly int IsJumpAnimation = Animator.StringToHash("IsJump");

        private int _currentIndexPlatform = 0;

        private SpawnerPlatform _spawnerPlatform;
        private Platform _currentPlatform;
        private Animator _animator;
        private AudioSource _jumpAudio;
        private Coroutine _coroutine;
        private TMP_Text _nameText;
        private Vector3 _startPosition;
        private int _counterThrows;
        private int _counterPenalty;
        private int _counterBonus;

        public int CurrentIndexPlatform => _currentIndexPlatform;

        public bool IsFinished { get; private set; }

        public event UnityAction<bool> FinishedGoing;
        public event UnityAction<string, int, int, int> FinishedGame;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _jumpAudio = GetComponent<AudioSource>();
            _nameText = GetComponentInChildren<TMP_Text>();

            _jumpAudio.volume = 1;
        }

        private void OnTriggerExit(Collider other)
        {
            _jumpAudio.Play();
        }

        public void Reset()
        {
            IsFinished = false;
            _nameText.text = EmptyName;
            transform.position = _startPosition;
            _currentIndexPlatform = StartIndexPlatform;
        }

        public void Init(SpawnerPlatform spawnerPlatform)
        {
            _spawnerPlatform = spawnerPlatform;
            _startPosition = _spawnerPlatform.GetPlatform(
                StartIndexPlatform).transform.position;
        }

        public void SetName(string name)
        {
            _nameText.text = name;
        }

        public void OnWalk(int numberStep)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(Walk(numberStep));
        }

        private IEnumerator Walk(int numberStep)
        {
            int polarity = 0;
            int count = 0;
            int index = 0;

            _counterThrows++;

            if (numberStep < 0 && _currentIndexPlatform != _spawnerPlatform.MaxIndex)
            {
                index = numberStep;
                polarity--;
            }
            else
            {
                count = numberStep;
                polarity++;
            }

            for (int i = index; i != count; i++)
            {
                _currentIndexPlatform += polarity;

                if (_currentIndexPlatform < StartIndexPlatform)
                {
                    _currentIndexPlatform = StartIndexPlatform;

                    СhangeCounterPlatform();
                    FinishedGoing?.Invoke(true);

                    yield break;
                }

                _animator.SetBool(IsJumpAnimation, true);

                _currentPlatform = _spawnerPlatform.GetPlatform(_currentIndexPlatform);

                while (transform.position != _currentPlatform.transform.position)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position, _currentPlatform.transform.position,
                        _speed * Time.deltaTime);

                    yield return null;
                }

                yield return new WaitForSeconds(DelayWalk);

                _animator.SetBool(IsJumpAnimation, false);

                if (_currentIndexPlatform >= _spawnerPlatform.MaxIndex)
                {
                    IsFinished = true;

                    FinishedGame?.Invoke(_nameText.text, _counterThrows, _counterBonus, _counterPenalty);

                    yield break;
                }

                yield return null;
            }

            СhangeCounterPlatform();
            FinishedGoing?.Invoke(true);
        }

        private void СhangeCounterPlatform()
        {
            if (_currentPlatform is PenaltyPlatform)
                _counterPenalty++;

            if (_currentPlatform is BonusPlatform)
                _counterBonus++;
        }
    }
}