using UnityEngine;
using TMPro;

public class GameStatistic : MonoBehaviour
{
    [SerializeField] private UserName _userName;
    [SerializeField] private Throw _throw;
    [SerializeField] private Bonus _bonus;
    [SerializeField] private Penalty _penalty;

    private const string Empty = "";

    private int _counter = 1;

    private TMP_Text[] _names;
    private TMP_Text[] _throws;
    private TMP_Text[] _bonuses;
    private TMP_Text[] _penalties;

    private void Awake()
    {
        _names = _userName.GetComponentsInChildren<TMP_Text>();
        _throws = _throw.GetComponentsInChildren<TMP_Text>();
        _bonuses = _bonus.GetComponentsInChildren<TMP_Text>();
        _penalties = _penalty.GetComponentsInChildren<TMP_Text>();

        Reset();
    }

    public void Reset()
    {
        for (int i = 1; i < _names.Length; i++)
        {
            _names[i].text = Empty;
            _throws[i].text = Empty;
            _bonuses[i].text = Empty;
            _penalties[i].text = Empty;
        }

        _counter = 1;
    }

    public void Set(string name, int throws, int bonus, int penalty)
    {
        _names[_counter].text = name;
        _throws[_counter].text = throws.ToString();
        _bonuses[_counter].text = bonus.ToString();
        _penalties[_counter].text = penalty.ToString();
        _counter++;
    }
}