namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutableButtonHighlight : ExecutableElement
    {
        [SerializeField]
        private ReverseMask _buttonPanel;
        [SerializeField]
        private GameObject _buttonObject;
        [SerializeField]
        private GameObject _highlightObject;
        private bool isClicked = false;
        private void OnClick()
        {
            isClicked=true;
        }
        public override IEnumerator Pause()
        {
            _buttonPanel.gameObject.SetActive(true);
            yield return null;
            _buttonPanel.TargetRectTransform=_highlightObject.GetComponent<RectTransform>();
            if(!_buttonObject.TryGetComponent<Button>(out var button))
            {
                button=_buttonObject.AddComponent<Button>();
            }
            button.onClick.AddListener(OnClick);
            yield return new WaitUntil(()=>isClicked);
            button.onClick.RemoveListener(OnClick);
        }
        public override IEnumerator Complete()
        {
            _buttonPanel.gameObject.SetActive(false);
            yield break;
        }
    }
}