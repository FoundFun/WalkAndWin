using UnityEngine;

public class DiceSide : MonoBehaviour
{
    [SerializeField] private int _oppositeNumber;

    public int Number => _oppositeNumber;

    public bool IsGround { get; private set; }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Table table))
            IsGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Table table))
            IsGround = false;
    }
}