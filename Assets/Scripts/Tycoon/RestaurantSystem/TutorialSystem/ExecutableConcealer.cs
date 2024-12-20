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
        private float _animationDuration;
        [SerializeField]
        private float _animationExponent=1;
        private CanvasGroup[] _canvasGroups;
        [SerializeField]
        private float _endingAlpha;
        [SerializeField]
        private Vector2 _endingOffset;
        private RectTransform[] _displayObjectRectTransform;
        private Vector2[] _initialPosition;
        public override IEnumerator Initialize()
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
            yield return base.Initialize();
        }
        public override IEnumerator Execute()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _animationDuration && !_isSkipping)
            {
                float t = elapsedTime / _animationDuration;

                Array.ForEach(_canvasGroups, canvasGroup=>canvasGroup.alpha = Mathf.Lerp(1f, _endingAlpha, Mathf.Pow(t, _animationExponent)));
                for(int i=0;i<_concealObjects.Length;i++)
                {
                    _displayObjectRectTransform[i].anchoredPosition=_initialPosition[i]+Vector2.Lerp(Vector2.zero, _endingOffset, Mathf.Pow(t, _animationExponent));
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield return base.Execute();
        }
        public override IEnumerator Finalize()
        {
            Array.ForEach(_canvasGroups, canvasGroup=>canvasGroup.alpha = _endingAlpha);
            for(int i=0;i<_concealObjects.Length;i++)
            {
                _displayObjectRectTransform[i].anchoredPosition=_initialPosition[i];
            }
            yield return base.Finalize();
        }
        public override IEnumerator Complete()
        {
            Array.ForEach(_concealObjects, concealObject=>concealObject.SetActive(false));
            yield return base.Complete();
        }
    }
}