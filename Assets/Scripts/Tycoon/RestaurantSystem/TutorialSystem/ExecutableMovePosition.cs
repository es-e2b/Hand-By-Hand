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
            // print("Move Position Class: _targetRectTransform.anchoredPosition - "+_targetRectTransform.anchoredPosition);
            yield return base.Begin();
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
            yield return base.Execute();
        }
        public override IEnumerator Finalize()
        {
            print("Move Position Class: Called Finalize Method");
            _targetRectTransform.anchoredPosition=_initialPosition+_targetPosition;
            print("Move Position Class: _targetRectTransform.anchoredPosition - "+_targetRectTransform.anchoredPosition);
            print("Move Position Class: _initialPosition - "+_initialPosition);
            print("Move Position Class: _targetPosition - "+_targetPosition);
            yield return base.Finalize();
        }
    }
}