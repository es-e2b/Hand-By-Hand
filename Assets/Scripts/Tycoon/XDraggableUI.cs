namespace Assets.Scripts.Tycoon
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class XDraggableUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField]
        private GameObject targetUI;
        private GraphicRaycaster raycaster;
        private RectTransform targetRectTransform;
        private bool isDragging;
        [SerializeField]
        private float openPositionX;
        [SerializeField]
        private float closePositionX;
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

            targetRectTransform.anchoredPosition += new Vector2(eventData.delta.x, 0);

            float currentPositionX=targetRectTransform.anchoredPosition.x;
            if(currentPositionX>openPositionX)
            {
                targetRectTransform.anchoredPosition = new Vector2(openPositionX, 0);
            }
            else if(currentPositionX<closePositionX)
            {
                targetRectTransform.anchoredPosition = new Vector2(closePositionX, 0);
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isDragging = false;

            float currentPositionY = targetRectTransform.anchoredPosition.y;
            float currentPositionX = targetRectTransform.anchoredPosition.x;
            float distanceToOpenPositionX = Mathf.Abs(currentPositionX - openPositionX);
            float distanceToClosePositionX = Mathf.Abs(currentPositionX - closePositionX);

            targetRectTransform.anchoredPosition = new Vector2(
                distanceToClosePositionX<distanceToOpenPositionX?closePositionX:openPositionX,
                currentPositionY);
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