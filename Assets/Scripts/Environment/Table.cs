using TMPro;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private TMP_Text _hintText;

    public void EnableHint()
    {
        _hintText.gameObject.SetActive(true);
    }

    public void DisableHint()
    {
        _hintText.gameObject.SetActive(false);
    }
}