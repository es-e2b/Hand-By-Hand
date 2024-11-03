namespace Assets.Scripts.Tycoon
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class XDraggableUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField]
        protected GameObject targetUI;
        protected GraphicRaycaster raycaster;
        protected RectTransform targetRectTransform;
        protected bool isDragging;
        [SerializeField]
        protected float openPositionX;
        [SerializeField]
        protected float closePositionX;
        protected void Awake()
        {
            raycaster = GetComponentInParent<GraphicRaycaster>();
            targetRectTransform = targetUI.GetComponent<RectTransform>();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if(!IsPointerOverUIObject(eventData)) return;

            isDragging = true;
        }
        public virtual void OnDrag(PointerEventData eventData)
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

            float targetX = distanceToClosePositionX < distanceToOpenPositionX ? closePositionX : openPositionX;
            Vector2 targetPosition = new Vector2(targetX, currentPositionY);

            float moveDistance = Mathf.Abs(currentPositionX - targetX);
            float maxDistance = Mathf.Abs(openPositionX - closePositionX) / 2;
            float maxTime = 0.2f;
            float targetTime = (moveDistance / maxDistance) * maxTime;

            StartCoroutine(SmoothMove(targetPosition, targetTime));
        }

        private IEnumerator SmoothMove(Vector2 targetPosition, float duration)
        {
            Vector2 startPosition = targetRectTransform.anchoredPosition;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                targetRectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            targetRectTransform.anchoredPosition = targetPosition;
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