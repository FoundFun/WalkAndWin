using System.Collections.Generic;
using UnityEngine;
using Platforms;
using System.Collections;

public class SpawnerPlatform : ObjectPool<Platform>
{
    [SerializeField] private Platform[] _platforms;
    [SerializeField] private PointContainer _pointContainer;
    [SerializeField] private Game _game;

    private Vector3 _maxScalePlatform = new Vector3(25, 25, 25);
    private Vector3 _startScalePlatform = new Vector3(14, 14, 14);

    private Point[] _spawnPoints;
    private List<Platform> _pool;
    private Coroutine _coroutine;

    public int MaxIndexPlatforms => _pool.Count - 1;

    private void Awake()
    {
        _spawnPoints = _pointContainer.GetComponentsInChildren<Point>();
        _pool = Initialize(_platforms, _spawnPoints.Length, true);

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Spawn());
    }

    public void Respawn()
    {
        if (_pool != null)
        {
            DisableEffect();

            for (int i = _pool.Count - 1; i >= 1; i--)
            {
                int j = Random.Range(0, i + 1);

                Platform template = _pool[j];
                _pool[j] = _pool[i];
                _pool[i] = template;
            }
        }

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Spawn());
    }

    public Platform GetPlatform(int indexPlatform)
    {
        if (indexPlatform <= _pool.Count && indexPlatform >= 0)
            return _pool[indexPlatform];
        else
            return null;
    }

    private IEnumerator Spawn()
    {
        float zoomSpeed = 0.2f;
        float delay = 0.1f;

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            if (TryGetObject(out Platform platform, i))
            {
                platform.transform.position = _spawnPoints[i].transform.position;
                platform.gameObject.SetActive(true);
                platform.SetScore(i);
                platform.gameObject.LeanScale(_maxScalePlatform, zoomSpeed);

                yield return new WaitForSeconds(delay);

                platform.gameObject.LeanScale(_startScalePlatform, zoomSpeed);

                yield return new WaitForSeconds(delay);
            }

            yield return null;
        }

        EnableEffect();
    }

    private void EnableEffect()
    {
        foreach (var platform in _pool)
        {
            if (platform is BonusPlatform)
                platform.FinishedStepPlayer += _game.ActivateBonus;
            else if (platform is PenaltyPlatform)
                platform.FinishedStepPlayer += _game.ActivatePenalty;
            else
                platform.FinishedStepPlayer += _game.ActiveDefault;
        }
    }

    private void DisableEffect()
    {
        foreach (var platform in _pool)
        {
            platform.gameObject.SetActive(false);

            if (platform is BonusPlatform)
                platform.FinishedStepPlayer -= _game.ActivateBonus;
            else if (platform is PenaltyPlatform)
                platform.FinishedStepPlayer -= _game.ActivatePenalty;
            else
                platform.FinishedStepPlayer -= _game.ActiveDefault;
        }
    }
}