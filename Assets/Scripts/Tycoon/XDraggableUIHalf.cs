namespace Assets.Scripts.Tycoon
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class XDraggableUIHalf : XDraggableUI
    {
        public override void OnDrag(PointerEventData eventData)
        {
            if(!isDragging) return;

            float pixelRatio = Screen.width / 1080f;
            targetRectTransform.anchoredPosition += new Vector2(eventData.delta.x / pixelRatio/2, 0);

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