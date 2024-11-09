namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;

    [Serializable]
    public class ExecutableMovePosition : ExecutableElement
    {
        [SerializeField]
        private RectTransform _targetRectTransform;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private float _animationExponent=1;
        [SerializeField]
        private Vector2 _targetPosition;
        private Vector2 _initialPosition;
        public override IEnumerator Begin()
        {
            _initialPosition=_targetRectTransform.anchoredPosition;
            if(_skipButton!=null)
            {
                _skipButton.gameObject.SetActive(true);
                _skipButton.onClick.AddListener(OnClickSkipButton);
            }
            yield return Next();
        }
        public override IEnumerator Next()
        {
            yield return Execute();
            yield return Complete();
        }
        public override IEnumerator Execute()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _animationDuration && !_isSkipping)
            {
                float t = elapsedTime / _animationDuration;

                _targetRectTransform.anchoredPosition=_initialPosition+Vector2.Lerp(Vector2.zero, _targetPosition, Mathf.Pow(t, _animationExponent));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _targetRectTransform.anchoredPosition=_initialPosition+_targetPosition;
            if(_isSkipping)
            {
                yield return Skip();
            }
            yield return Pause();
        }
        public override IEnumerator Pause()
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
        public override IEnumerator Skip()
        {
            yield break;
        }
        public override IEnumerator Complete()
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