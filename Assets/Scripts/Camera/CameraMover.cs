using System.Collections;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Landscape _landscape;
    [SerializeField] private float _speed;

    private Coroutine _corouitne;

    public void OnTwist()
    {
        if (_corouitne != null)
            StopCoroutine(_corouitne);

        _corouitne = StartCoroutine(Twist());
    }

    private IEnumerator Twist()
    {
        while (true)
        {
            transform.LookAt(_landscape.transform);
            transform.RotateAround(_landscape.transform.position, Vector3.up, _speed * Time.deltaTime);

            yield return null;
        }
    }
}