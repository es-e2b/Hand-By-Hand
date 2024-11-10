namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutablePrompt : ExecutableElement
    {
        [SerializeField]
        private ExecutableElement _beforeSelectExecuteElement;
        [SerializeField]
        private ExecutableElement[] _executableElements;
        [SerializeField]
        private Button[] _executeButtons;
        [SerializeField]
        private ExecutableElement _afterSelectExecuteElement;
        private bool _isSelected;
        private int _executeIndex;
        private void SelectElement(int index)
        {
            _executeIndex=index;
            _isSelected=true;
        }
        public override IEnumerator Begin()
        {
            if(_isSkipping)
            {
                _beforeSelectExecuteElement.Skip();
            }
            if(_beforeSelectExecuteElement!=null)
            {
                yield return _beforeSelectExecuteElement.Initialize();
            }
            _isSkipping=false;
            for(int i=0;i<_executeButtons.Length;i++)
            {
                _executeButtons[i].onClick.AddListener(()=>SelectElement(i));
            }
            yield return base.Begin();
        }
        public override IEnumerator Next()
        {
            yield return new WaitUntil(()=>_isSelected);
            for(int i=0;i<_executeButtons.Length;i++)
            {
                _executeButtons[i].onClick.RemoveAllListeners();
            }
            yield return base.Next();
        }
        public override IEnumerator Execute()
        {
            if(_isSkipping)
            {
                _afterSelectExecuteElement.Skip();
            }
            if(_afterSelectExecuteElement!=null)
            {
                yield return _afterSelectExecuteElement.Initialize();
            }
            _skipButton.gameObject.SetActive(false);
            yield return _executableElements[_executeIndex].Initialize();
            _skipButton.gameObject.SetActive(true);
        }
    }
}