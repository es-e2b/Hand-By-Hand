namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutableMoveAnchor : ExecutableElement
    {
        [SerializeField]
        private RectTransform _targetRectTransform;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private Vector2 _targetAnchorMin;
        [SerializeField]
        private Vector2 _targetAnchorMax;
        private Vector2 _initialAnchorMin;
        private Vector2 _initialAnchorMax;
        public override IEnumerator Begin()
        {
            _initialAnchorMin=_targetRectTransform.anchorMin;
            _initialAnchorMax=_targetRectTransform.anchorMax;
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

                _targetRectTransform.anchorMax=_targetRectTransform.pivot=Vector2.Lerp(_initialAnchorMax, _targetAnchorMax, t*t*t);
                _targetRectTransform.anchorMin=_targetRectTransform.pivot=Vector2.Lerp(_initialAnchorMin, _targetAnchorMin, t*t*t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _targetRectTransform.anchorMax=_targetAnchorMax;
            _targetRectTransform.anchorMin=_targetAnchorMin;
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