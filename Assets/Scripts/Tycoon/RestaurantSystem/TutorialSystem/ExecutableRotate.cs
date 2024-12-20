namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;

    [Serializable]
    public class ExecutableRotate : ExecutableElement
    {
        [SerializeField]
        private RectTransform _targetRectTransform;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private float _animationExponent=1f;
        [SerializeField]
        private Vector3 _targetRotation;
        private Vector3 _initialRotation;
        public override IEnumerator Begin()
        {
            _initialRotation=_targetRectTransform.eulerAngles;
            yield return base.Begin();
        }
        public override IEnumerator Execute()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _animationDuration && !_isSkipping)
            {
                float t = elapsedTime / _animationDuration;

                _targetRectTransform.eulerAngles=Vector3.Lerp(_initialRotation, _targetRotation, Mathf.Pow(t, _animationExponent));

                yield return null;
                elapsedTime += Time.deltaTime;
            }
            _targetRectTransform.eulerAngles=_targetRotation;
            yield return base.Execute();
        }
        public override IEnumerator Finalize()
        {
            _targetRectTransform.eulerAngles=_targetRotation;
            yield return base.Finalize();
        }
    }
}