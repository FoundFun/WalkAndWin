using UnityEngine;

namespace Views
{
    public abstract class View : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup CanvasGroup;

        protected virtual void Open()
        {
            CanvasGroup.LeanAlpha(1, 1);
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }

        protected virtual void Close()
        {
            CanvasGroup.LeanAlpha(0, 1);
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }
    }
}