using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Players;
using Platforms;
using UnityEngine.Events;

public class SpawnerPlayer : ObjectPool<Player>
{
    [SerializeField] private Player[] _players;
    [SerializeField] private Dice _dice;
    [SerializeField] private SpawnerPlatform _spawnerPlatform;
    [SerializeField] private GameStatistic _statistic;

    private List<Player> _pool;
    private Coroutine _coroutine;
    private int _currentPlayer;

    public int CountPlayer => _pool.Count;
    public bool IsFinished => _pool[_currentPlayer].IsFinished;

    public bool IsFinishedGoing { get; private set; }

    public event UnityAction FinishedPlayer;
    public event UnityAction ChangedPlayer;

    private void Awake()
    {
        _pool = Initialize(_players, _players.Length, false);
        Init(_pool);
    }

    private void OnEnable()
    {
        _dice.Rolled += OnMoveCurrentPlayer;
    }

    private void OnDisable()
    {
        _dice.Rolled -= OnMoveCurrentPlayer;
    }

    public void Reset()
    {
        foreach (var player in _pool)
        {
            player.Reset();
            player.gameObject.SetActive(false);
        }
    }

    public void Enable(int indexPlayer)
    {
        IsFinishedGoing = false;
        _currentPlayer = indexPlayer;
        _pool[_currentPlayer].FinishedGoing += OnFinishedGoing;
        _pool[_currentPlayer].FinishedGame += OnFinishGame;
    }

    public void Disable()
    {
        _pool[_currentPlayer].FinishedGoing -= OnFinishedGoing;
        _pool[_currentPlayer].FinishedGame -= OnFinishGame;
    }

    public void Create(string name, int index)
    {
        if (TryGetObject(out Player player, index))
        {
            player.Reset();
            player.SetName(name);
            player.gameObject.SetActive(true);
        }
    }

    public void OnMoveCurrentPlayer(int numberStep)
    {
        _pool[_currentPlayer].OnWalk(numberStep);
    }

    private void Init(List<Player> pool)
    {
        foreach (var player in pool)
        {
            player.Init(_spawnerPlatform);
            player.Reset();
            player.gameObject.SetActive(false);
        }
    }

    private void OnFinishedGoing(bool state)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(FinishedGoing(state));
    }

    private IEnumerator FinishedGoing(bool state)
    {
        float delay = 0.3f;

        yield return new WaitForSeconds(delay);

        IsFinishedGoing = state;

        if (_pool[_currentPlayer].CurrentIndexPlatform
            < _spawnerPlatform.MaxIndexPlatforms
            && _pool[_currentPlayer].CurrentIndexPlatform > 0)
        {
            Platform platform = _spawnerPlatform.GetPlatform(
                _pool[_currentPlayer].CurrentIndexPlatform);

            platform.ActivateEffect();
        }
        else
        {
            ChangedPlayer?.Invoke();
        }
    }

    private void OnFinishGame(string name, int throws, int bonus, int penalty)
    {
        IsFinishedGoing = true;

        _statistic.Set(name, throws, bonus, penalty);
        FinishedPlayer?.Invoke();
    }
}