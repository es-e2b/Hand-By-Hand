namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using UnityEngine;

    public class SkipButton : MonoBehaviour
    {
        private void Awake()
        {
            ApplySafeArea();
        }
        private void ApplySafeArea()
        {
            RectTransform panelRectTransform=GetComponent<RectTransform>();
            Rect safeArea = Screen.safeArea;

            panelRectTransform.sizeDelta = safeArea.size*(1080/safeArea.width);
            panelRectTransform.anchoredPosition=safeArea.position;

            print(safeArea);
        }
    }
}