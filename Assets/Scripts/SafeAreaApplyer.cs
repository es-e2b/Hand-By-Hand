namespace Assets.Scripts
{
    using UnityEngine;

    public class SafeAreaApplyer : MonoBehaviour, ISafeAreaSetter
    {

        public RectTransform PanelRectTransform => GetComponent<RectTransform>();
        private void Start()
        {
            ApplySafeArea();
        }
        public void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;

            PanelRectTransform.sizeDelta = safeArea.size*(1080/safeArea.width);
            PanelRectTransform.anchoredPosition=safeArea.position;

            print(safeArea);
        }
    }
}