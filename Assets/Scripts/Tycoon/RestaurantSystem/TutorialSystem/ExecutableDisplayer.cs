namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutableDisplayer : ExecutableElement
    {
        [SerializeField]
        private GameObject[] _displayObjects;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private float _animationExponent=1;
        private CanvasGroup[] _canvasGroups;
        [SerializeField]
        private float _staringAlpha;
        [SerializeField]
        private Vector2 _startingOffset;
        private RectTransform[] _displayObjectRectTransform;
        private Vector2[] _initialPosition;
        public override IEnumerator Begin()
        {
            _canvasGroups=new CanvasGroup[_displayObjects.Length];
            _displayObjectRectTransform=new RectTransform[_displayObjects.Length];
            _initialPosition=new Vector2[_displayObjects.Length];
            for(int i=0;i<_displayObjects.Length;i++)
            {
                if(!_displayObjects[i].TryGetComponent<CanvasGroup>(out var canvasGroup))
                {
                    canvasGroup=_displayObjects[i].AddComponent<CanvasGroup>();
                }
                canvasGroup.alpha=_staringAlpha;
                _canvasGroups[i]=canvasGroup;
                _displayObjectRectTransform[i]=_displayObjects[i].GetComponent<RectTransform>();
                _displayObjects[i].GetComponent<RectTransform>().anchoredPosition+=_startingOffset;
                _initialPosition[i]=_displayObjectRectTransform[i].anchoredPosition;
                _displayObjects[i].SetActive(true);
            }
            if(_skipButton!=null)
            {
                _skipButton.gameObject.SetActive(true);
                _skipButton.onClick.AddListener(OnClickSkipButton);
            }
            yield return Next();
        }
        public override IEnumerator Execute()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _animationDuration && !_isSkipping)
            {
                float t = elapsedTime / _animationDuration;

                Array.ForEach(_canvasGroups, canvasGroup=>canvasGroup.alpha = Mathf.Lerp(_staringAlpha, 1f, Mathf.Pow(t, _animationExponent)));
                for(int i=0;i<_displayObjects.Length;i++)
                {
                    _displayObjectRectTransform[i].anchoredPosition=_initialPosition[i]+Vector2.Lerp(_startingOffset, Vector2.zero, Mathf.Pow(t, _animationExponent));
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Array.ForEach(_canvasGroups, canvasGroup=>canvasGroup.alpha = 1f);
            for(int i=0;i<_displayObjects.Length;i++)
            {
                _displayObjectRectTransform[i].anchoredPosition=_initialPosition[i];
            }
            if(_isSkipping)
            {
                yield return Skip();
            }
            yield return Pause();
        }
    }
}