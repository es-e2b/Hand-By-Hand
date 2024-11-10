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
        private Button _button;
        private void OnClick()
        {
            isClicked=true;
        }
        public override IEnumerator Initialize()
        {
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
        }
        public override IEnumerator Complete()
        {
            _button.onClick.RemoveListener(OnClick);
            yield return base.Complete();
        }
    }
}