using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public abstract class View : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup CanvasGroup;

        protected virtual void Open()
        {
            CanvasGroup.LeanAlpha(1, 1).setIgnoreTimeScale(true);
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
        }

        protected virtual void Close()
        {
            CanvasGroup.LeanAlpha(0, 1).setIgnoreTimeScale(true);
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
        }
    }
}