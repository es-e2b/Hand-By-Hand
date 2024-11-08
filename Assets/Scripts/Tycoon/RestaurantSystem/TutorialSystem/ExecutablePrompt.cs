namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using Assets.Scripts.Tycoon.RestaurantSystem.OrderSystem;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutablePrompt : ExecutableElement
    {
        [SerializeField]
        private GameObject _promptPanel;
        [SerializeField]
        private Button[] _buttons;
        private bool isClicked = false;
        private void OnClick()
        {
            isClicked=true;
        }
        public override IEnumerator Begin()
        {
            _promptPanel.SetActive(true);
            yield return Next();
        }
        public override IEnumerator Next()
        {
            yield return Execute();
            yield return Complete();
        }
        public override IEnumerator Execute()
        {
            yield return Pause();
        }
        public override IEnumerator Pause()
        {
            Array.ForEach(_buttons, button=>button.onClick.AddListener(OnClick));
            yield return new WaitUntil(()=>isClicked);
            Array.ForEach(_buttons, button=>button.onClick.RemoveListener(OnClick));
        }
        public override IEnumerator Skip()
        {
            throw new NotImplementedException();
        }
        public override IEnumerator Complete()
        {
            if(_promptPanel.transform.GetChild(1).TryGetComponent<ObjectDisplayAnimator>(out var objectDisplayAnimator))
            {
                yield return objectDisplayAnimator.HideMessageAnimation();
            }
            _promptPanel.SetActive(false);
            yield break;
        }
    }
}