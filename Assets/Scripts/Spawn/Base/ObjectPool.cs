using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private GameObject _spawnContainer;

    private List<T> _poolObject;
    private int _index;

    protected List<T> Initialize(T[] gameObject, int capacity, bool isRandomSpawn)
    {
        _poolObject = new List<T>(gameObject.Length);

        for (int i = 0; i < capacity; i++)
        {
            if (isRandomSpawn)
                _index = Random.Range(0, gameObject.Length);
            else
                _index = i;

            T template = Instantiate(gameObject[_index], _spawnContainer.transform);

            _poolObject.Add(template);
        }

        return _poolObject;
    }

    protected bool TryGetObject(out T gameObject, int index)
    {
        gameObject = _poolObject.ElementAtOrDefault(index);

        return gameObject != null;
    }
}