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
        private Button _button;
        private void OnClick()
        {
            isClicked=true;
        }
        public override IEnumerator Initialize()
        {
            _buttonPanel.gameObject.SetActive(true);
            yield return null;
            _buttonPanel.TargetRectTransform=_highlightObject.GetComponent<RectTransform>();
            if(!_buttonObject.TryGetComponent<Button>(out _button))
            {
                _button=_buttonObject.AddComponent<Button>();
            }
            _button.onClick.AddListener(OnClick);
            yield return Pause();
        }
        public override IEnumerator Pause()
        {
            yield return new WaitUntil(()=>isClicked);
            yield return base.Pause();
        }
        public override IEnumerator Complete()
        {
            _button.onClick.RemoveListener(OnClick);
            _buttonPanel.gameObject.SetActive(false);
            yield break;
        }
    }
}