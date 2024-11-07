namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutableButtonWait : ExecutableElement
    {
        [SerializeField]
        private GameObject _buttonObject;
        private bool isClicked = false;
        private void OnClick()
        {
            isClicked=true;
        }
        public override IEnumerator Begin()
        {
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
            if(!_buttonObject.TryGetComponent<Button>(out var button))
            {
                button=_buttonObject.AddComponent<Button>();
            }
            button.onClick.AddListener(OnClick);
            yield return new WaitUntil(()=>isClicked);
            button.onClick.RemoveListener(OnClick);
        }
        public override IEnumerator Skip()
        {
            throw new NotImplementedException();
        }
        public override IEnumerator Complete()
        {
            yield break;
        }
    }
}