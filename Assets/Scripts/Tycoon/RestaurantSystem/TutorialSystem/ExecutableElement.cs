namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutableElement : MonoBehaviour, IExcutable
    {
        [SerializeField]
        private float _watingStartTime;
        [SerializeField]
        protected float _puaseDuration;
        [SerializeField]
        protected Button _skipButton;
        protected bool _isSkipping;
        [SerializeField]
        protected bool isRequiredClickToComplete;
        protected void OnClickSkipButton()
        {
            _isSkipping=true;
        }
        public virtual IEnumerator Initialize()
        {
            yield return new WaitForSeconds(_watingStartTime);
            yield return Begin();
        }
        public virtual IEnumerator Begin()
        {
            if(_skipButton!=null)
            {
                _skipButton.gameObject.SetActive(true);
                _skipButton.onClick.AddListener(OnClickSkipButton);
            }
            yield return Next();
        }
        public virtual IEnumerator Next()
        {
            yield return Execute();
            yield return Complete();
        }

        public virtual IEnumerator Execute()
        {
            if(_isSkipping)
            {
                yield return Skip();
            }
            yield return Pause();
        }


        public virtual IEnumerator Pause()
        {
            _isSkipping=false;
            if(isRequiredClickToComplete)
            {
                yield return new WaitUntil(()=>_isSkipping);
                yield break;
            }
            float elapsedTime=0f;

            while (elapsedTime < _puaseDuration && !_isSkipping)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield break;
        }

        public virtual IEnumerator Skip()
        {
            yield break;
        }

        public virtual IEnumerator Complete()
        {
            if(_skipButton!=null)
            {
                _skipButton.onClick.RemoveListener(OnClickSkipButton);
                _skipButton.gameObject.SetActive(false);
            }
            yield break;
        }
    }
}