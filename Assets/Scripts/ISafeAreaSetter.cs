namespace Assets.Scripts
{
    using UnityEngine;
    public interface ISafeAreaSetter
    {
        RectTransform PanelRectTransform{ get; }
        void ApplySafeArea();
    }
}