using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Platforms
{
    public abstract class Platform : MonoBehaviour
    {
        private TMP_Text _score;

        public event UnityAction FinishedStepPlayer;

        private void Awake()
        {
            _score = GetComponentInChildren<TMP_Text>();
        }

        public void SetScore(int indexPlatfrom)
        {
            indexPlatfrom++;
            _score.text = indexPlatfrom.ToString();
        }

        public void ActivateEffect()
        {
            FinishedStepPlayer?.Invoke();
        }
    }
}