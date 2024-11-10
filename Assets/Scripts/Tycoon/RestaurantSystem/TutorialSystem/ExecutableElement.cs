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
        public virtual void Skip()
        {
            _isSkipping=true;
        }
        public virtual IEnumerator Initialize()
        {
            _isSkipping=false;
            if(_skipButton!=null)
            {
                _skipButton.gameObject.SetActive(true);
                _skipButton.onClick.AddListener(Skip);
            }
            float elapsedTime=0f;

            while (elapsedTime < _watingStartTime && !_isSkipping)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if(_isSkipping)
            {
                yield return Finalize();
                yield return Pause();
                yield return Complete();
            }
            else
            {
                yield return Begin();
            }
        }
        public virtual IEnumerator Begin()
        {
            yield return Next();
        }
        public virtual IEnumerator Next()
        {
            yield return Execute();
            yield return Complete();
        }

        public virtual IEnumerator Execute()
        {
            yield return Finalize();
            yield return Pause();
        }
        public virtual IEnumerator Pause()
        {
            _isSkipping=false;
            float elapsedTime=0f;

            while (elapsedTime < _puaseDuration && !_isSkipping)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield break;
        }

        public virtual IEnumerator Finalize()
        {
            yield break;
        }

        public virtual IEnumerator Complete()
        {
            if(_skipButton!=null)
            {
                _skipButton.onClick.RemoveListener(Skip);
                _skipButton.gameObject.SetActive(false);
            }
            yield break;
        }
    }
}