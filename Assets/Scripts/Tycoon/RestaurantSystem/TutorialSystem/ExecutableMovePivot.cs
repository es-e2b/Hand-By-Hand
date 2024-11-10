namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;

    [Serializable]
    public class ExecutableMovePivot : ExecutableElement
    {
        [SerializeField]
        private RectTransform _targetRectTransform;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private float _animationExponent=1;
        [SerializeField]
        private Vector2 _targetPivot;
        private Vector2 _initialPivot;
        public override IEnumerator Begin()
        {
            _initialPivot=_targetRectTransform.pivot;
            yield return base.Begin();
        }
        public override IEnumerator Execute()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _animationDuration && !_isSkipping)
            {
                float t = elapsedTime / _animationDuration;

                _targetRectTransform.pivot=Vector2.Lerp(_initialPivot, _targetPivot, Mathf.Pow(t, _animationExponent));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _targetRectTransform.pivot=_targetPivot;
            yield return base.Execute();
        }
        public override IEnumerator Finalize()
        {
            _targetRectTransform.pivot=_targetPivot;
            yield return base.Finalize();
        }
    }
}