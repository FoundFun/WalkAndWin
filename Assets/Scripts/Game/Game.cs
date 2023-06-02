using System.Collections;
using UnityEngine;
using Views;
using TMPro;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private StartView _startView;
    [SerializeField] private GameView _gameView;
    [SerializeField] private GameOverView _gameOverView;
    [SerializeField] private SpawnerPlatform _spawnerPlatform;
    [SerializeField] private SpawnerPlayer _spawnerPlayer;
    [SerializeField] private Dice _dice;
    [SerializeField] private Table _table;
    [SerializeField] private GameStatistic _gameStatistic;
    [SerializeField] private TMP_InputField _inputUserText;
    [SerializeField] private Button _createPlayerButton;

    private const int MinNumberPlayers = 1;
    private const int NumberPenalty = -3;
    private const string EmptyName = "";

    private Coroutine _coroutine;
    private int _currentNumberPlayers;
    private int _currentActivePlayer;
    private int _numberWinners;
    private string _playerName;
    private bool _isCreatePlayer;

    private void OnEnable()
    {
        _createPlayerButton.onClick.AddListener(OnCreatePlayer);

        _startView.StartButtonClick += OnStartGame;
        _startView.CreateNameButtonClick += OnCreateName;
        _gameView.NextPlayerButtonClick += OnChangePlayer;
        _gameView.ExitGameButtonClick += OnFinishGame;
        _gameOverView.ExitMenuButtonClick += OnExitMenuButtonClick;
        _spawnerPlayer.FinishedPlayer += OnFinishedPlayer;
        _spawnerPlayer.ChangedPlayer += _gameView.EnableButtonNextPlayer;
    }

    private void OnDisable()
    {
        _createPlayerButton.onClick.RemoveListener(OnCreatePlayer);

        _startView.StartButtonClick -= OnStartGame;
        _startView.CreateNameButtonClick -= OnCreateName;
        _gameView.NextPlayerButtonClick -= OnChangePlayer;
        _gameView.ExitGameButtonClick -= OnFinishGame;
        _gameOverView.ExitMenuButtonClick -= OnExitMenuButtonClick;
        _spawnerPlayer.FinishedPlayer -= OnFinishedPlayer;
        _spawnerPlayer.ChangedPlayer -= _gameView.EnableButtonNextPlayer;
    }

    private void Start()
    {
        Reset();
    }

    private void Reset()
    {
        _currentNumberPlayers = 0;
        _currentActivePlayer = 0;
        _numberWinners = 0;
        _playerName = EmptyName;
        _isCreatePlayer = false;

        _gameOverView.CloseScreen();
        _gameView.CloseScreen();
        _gameView.DisableButtonNextPlayer();
        _startView.OpenScreen();
        _startView.EnableButtonCreatePlayer();
        _dice.Reset();
        _dice.gameObject.SetActive(false);
        _spawnerPlayer.Reset();
        _gameStatistic.Reset();
        _createPlayerButton.gameObject.SetActive(true);
    }

    public void ActivateBonus()
    {
        _dice.Reset();
    }

    public void ActivatePenalty()
    {
        _spawnerPlayer.OnMoveCurrentPlayer(NumberPenalty);
    }

    public void ActiveDefault()
    {
        _gameView.EnableButtonNextPlayer();
    }

    private void OnFinishedPlayer()
    {
        _numberWinners++;

        if (_numberWinners >= _currentNumberPlayers)
        {
            OnFinishGame();

            return;
        }

        _gameView.EnableButtonNextPlayer();
    }

    private void OnFinishGame()
    {
        _gameView.CloseScreen();
        _gameOverView.OpenScreen();
    }

    private void OnCreatePlayer()
    {
        if (_currentNumberPlayers < _spawnerPlayer.CountPlayer)
        {
            _playerName = _inputUserText.text;
            _spawnerPlayer.Create(_playerName, _currentNumberPlayers);
            _currentNumberPlayers++;
            _isCreatePlayer = true;

            if (_currentNumberPlayers < _spawnerPlayer.CountPlayer)
                _startView.EnableButtonCreatePlayer();
        }
    }

    private void OnCreateName()
    {
        _startView.DisableButtonCreatePlayer();

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CreateName());
    }

    private void OnStartGame()
    {
        if (_currentNumberPlayers > MinNumberPlayers)
        {
            _table.EnableHint();
            _startView.CloseScreen();
            _gameView.OpenScreen();
            _spawnerPlayer.Enable(_currentActivePlayer);
            _dice.gameObject.SetActive(true);
        }
    }

    private void OnChangePlayer()
    {
        if (_dice.IsRolled && _spawnerPlayer.IsFinishedGoing)
        {
            for (int i = 0; i < _currentNumberPlayers; i++)
            {
                _spawnerPlayer.Disable();

                _currentActivePlayer = _currentActivePlayer < _currentNumberPlayers - 1
                    ? _currentActivePlayer + 1 : 0;

                _spawnerPlayer.Enable(_currentActivePlayer);

                if (!_spawnerPlayer.IsFinished)
                {
                    _gameView.DisableButtonNextPlayer();
                    _dice.Reset();

                    return;
                }
            }
        }
    }

    private IEnumerator CreateName()
    {
        _isCreatePlayer = false;
        _inputUserText.gameObject.SetActive(true);
        _inputUserText.interactable = true;
        _createPlayerButton.gameObject.SetActive(false);
        _createPlayerButton.interactable = false;

        yield return new WaitWhile(() => _inputUserText.text == EmptyName);

        _createPlayerButton.gameObject.SetActive(true);
        _createPlayerButton.interactable = true;

        yield return new WaitWhile(() => _isCreatePlayer == false);

        _inputUserText.gameObject.SetActive(false);
        _inputUserText.interactable = false;
        _inputUserText.text = EmptyName;
        _createPlayerButton.gameObject.SetActive(false);
        _createPlayerButton.interactable = false;
    }

    private void OnExitMenuButtonClick()
    {
        Reset();
        _table.DisableHint();
        _spawnerPlatform.Respawn();
    }
}