using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Resources.Scripts
{
    [RequireComponent(typeof(Button))]
    public class IndicatorButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        public void OnPointerUp(PointerEventData eventData)
        {
            OnClickUp?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }

        public UnityEvent OnClick;
        public UnityEvent OnClickUp;
    }
}
