namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.UI;

    public class LayoutRebuilderComponent : MonoBehaviour
    {
        private void OnEnable()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }
    }
}