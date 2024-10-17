namespace Assets.Scripts.Tycoon
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class YDraggableUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField]
        private GameObject targetUI;
        private GraphicRaycaster raycaster;
        private RectTransform targetRectTransform;
        private bool isDragging;
        [SerializeField]
        private float openPositionY;
        [SerializeField]
        private float closePositionY;
        private void Awake()
        {
            raycaster = GetComponentInParent<GraphicRaycaster>();
            targetRectTransform = targetUI.GetComponent<RectTransform>();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if(!IsPointerOverUIObject(eventData)) return;

            isDragging = true;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if(!isDragging) return;

            targetRectTransform.anchoredPosition += new Vector2(0, eventData.delta.y);

            float currentPositionY=targetRectTransform.anchoredPosition.y;
            if(currentPositionY>openPositionY)
            {
                targetRectTransform.anchoredPosition = new Vector2(0, openPositionY);
            }
            else if(currentPositionY<closePositionY)
            {
                targetRectTransform.anchoredPosition = new Vector2(0, closePositionY);
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;

            float currentPositionY = targetRectTransform.anchoredPosition.y;
            float currentPositionX = targetRectTransform.anchoredPosition.x;
            float distanceToOpenPositionY = Mathf.Abs(currentPositionY - openPositionY);
            float distanceToClosePositionY = Mathf.Abs(currentPositionY - closePositionY);

            targetRectTransform.anchoredPosition = new Vector2(
                currentPositionX,
                distanceToClosePositionY<distanceToOpenPositionY?closePositionY:openPositionY);
        }
        private bool IsPointerOverUIObject(PointerEventData eventData)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject == gameObject)
                {
                    return true;
                }
            }
            return false;
        }
    }
}