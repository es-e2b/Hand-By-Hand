namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutableRotate : ExecutableElement
    {
        [SerializeField]
        private RectTransform _targetRectTransform;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private Vector3 _targetRotation;
        private Vector3 _initialRotation;
        public override IEnumerator Begin()
        {
            _initialRotation=_targetRectTransform.eulerAngles;
            Debug.Log(_initialRotation);
            yield return base.Begin();
        }
        public override IEnumerator Execute()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _animationDuration && !_isSkipping)
            {
                float t = elapsedTime / _animationDuration;

                _targetRectTransform.eulerAngles=Vector3.Lerp(_initialRotation, _targetRotation, t*t*t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _targetRectTransform.eulerAngles=_targetRotation;
            yield return base.Execute();
        }
    }
}