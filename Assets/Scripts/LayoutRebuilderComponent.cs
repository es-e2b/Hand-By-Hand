namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.UI;

    public class LayoutRebuilderComponent : MonoBehaviour
    {
        private void OnEnable()
        {
            RebuildLayout();
        }
        private void RebuildLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());

            RebuildParentLayout();
        }

        private void RebuildParentLayout()
        {
            if (transform.parent != null && transform.parent.TryGetComponent<LayoutRebuilderComponent>(out var parentLayoutRebuilder))
            {
                parentLayoutRebuilder.RebuildLayout();
            }
        }
    }
}