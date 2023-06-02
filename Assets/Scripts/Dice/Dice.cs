using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    private const int MinTorque = 200;
    private const int MaxTorque = 501;
    private const float ResetTime = 10;

    private DiceSide[] _sides;
    private ControlsInput _input;
    private Rigidbody _rigidbody;
    private Coroutine _coroutine;
    private Vector3 _startPosition;
    private int _diceValue;
    private bool _hasLanded;
    private bool _thrown;

    public bool IsRolled { get; private set; }

    public event UnityAction<int> Rolled;

    private void Awake()
    {
        _input = new ControlsInput();
        _sides = GetComponentsInChildren<DiceSide>();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Dice.Roll.performed += context => Roll();
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Dice.Roll.performed -= context => Roll();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;
        _rigidbody.useGravity = false;
    }

    public void Reset()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _diceValue = 0;
        IsRolled = false;
        _thrown = false;
        _hasLanded = false;
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = false;
        transform.position = _startPosition;
    }

    private void Roll()
    {
        if (!_thrown && !_hasLanded)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            StartCoroutine(Land());

            _thrown = true;
            _rigidbody.useGravity = true;

            _rigidbody.AddTorque(
                Random.Range(MinTorque, MaxTorque),
                Random.Range(MinTorque, MaxTorque),
                Random.Range(MinTorque, MaxTorque));
        }
    }

    private IEnumerator Land()
    {
        float elapsedTime = 0;

        while (_diceValue == 0)
        {
            if (_rigidbody.IsSleeping() && !_hasLanded && _thrown)
            {
                _hasLanded = true;
                _rigidbody.useGravity = false;
                _rigidbody.isKinematic = true;

                _diceValue = GetNumber();

                if (_diceValue != 0)
                {
                    Rolled?.Invoke(_diceValue);
                    IsRolled = true;
                }
            }
            else if (_rigidbody.IsSleeping() && _hasLanded && _diceValue == 0)
            {
                Reset();
                Roll();
            }
            else if (elapsedTime > ResetTime)
            {
                Reset();
                Roll();
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    private int GetNumber()
    {
        _diceValue = 0;

        foreach (var side in _sides)
        {
            if (side.IsGround)
                return side.Number;
        }

        return _diceValue;
    }
}