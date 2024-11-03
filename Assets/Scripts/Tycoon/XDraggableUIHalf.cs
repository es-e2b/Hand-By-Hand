namespace Assets.Scripts.Tycoon
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class XDraggableUIHalf : XDraggableUI
    {
        public override void OnDrag(PointerEventData eventData)
        {
            if(!isDragging) return;

            targetRectTransform.anchoredPosition += new Vector2(eventData.delta.x, 0)/2;

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
    }
}