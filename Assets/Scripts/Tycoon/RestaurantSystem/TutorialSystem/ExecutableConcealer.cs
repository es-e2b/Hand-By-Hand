namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutableConcealer : ExecutableElement
    {
        [SerializeField]
        private GameObject[] _concealObjects;
        [SerializeField]
        private Button _skipButton;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private float _puaseDuration;
        private CanvasGroup[] _canvasGroups;
        [SerializeField]
        private float _endingAlpha;
        private bool _isSkipping;
        [SerializeField]
        private Vector2 _endingOffset;
        private RectTransform[] _displayObjectRectTransform;
        private Vector2[] _initialPosition;
        private void OnClickSkipButton()
        {
            _isSkipping=true;
        }
        public override IEnumerator Begin()
        {
            _canvasGroups=new CanvasGroup[_concealObjects.Length];
            _displayObjectRectTransform=new RectTransform[_concealObjects.Length];
            _initialPosition=new Vector2[_concealObjects.Length];
            for(int i=0;i<_concealObjects.Length;i++)
            {
                if(!_concealObjects[i].TryGetComponent<CanvasGroup>(out var canvasGroup))
                {
                    canvasGroup=_concealObjects[i].AddComponent<CanvasGroup>();
                }
                canvasGroup.alpha=1f;
                _canvasGroups[i]=canvasGroup;
                _displayObjectRectTransform[i]=_concealObjects[i].GetComponent<RectTransform>();
                _initialPosition[i]=_displayObjectRectTransform[i].anchoredPosition;
            }
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

                Array.ForEach(_canvasGroups, canvasGroup=>canvasGroup.alpha = Mathf.Lerp(1f, _endingAlpha, t));
                for(int i=0;i<_concealObjects.Length;i++)
                {
                    _displayObjectRectTransform[i].anchoredPosition=_initialPosition[i]+Vector2.Lerp(Vector2.zero, _endingOffset, t);
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if(_isSkipping)
            {
                yield return Skip();
            }
            else
            {
                yield return Pause();
            }
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
        }
        public override IEnumerator Skip()
        {
            Array.ForEach(_canvasGroups, canvasGroup=>canvasGroup.alpha = _endingAlpha);
            for(int i=0;i<_concealObjects.Length;i++)
            {
                _displayObjectRectTransform[i].anchoredPosition=_initialPosition[i];
            }
            yield break;
        }
        public override IEnumerator Complete()
        {
            Array.ForEach(_concealObjects, concealObject=>concealObject.SetActive(false));
            if(_skipButton!=null)
            {
                _skipButton.onClick.RemoveListener(OnClickSkipButton);
                _skipButton.gameObject.SetActive(false);
            }
            yield break;
        }
    }
}