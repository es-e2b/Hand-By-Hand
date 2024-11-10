namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;

    [Serializable]
    public class ExecutableScale : ExecutableElement
    {
        [SerializeField]
        private RectTransform _targetRectTransform;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private float _animationExponent=1;
        [SerializeField]
        private Vector3 _targetScale;
        private Vector3 _initialScale;
        public override IEnumerator Begin()
        {
            _initialScale=_targetRectTransform.localScale;
            yield return base.Begin();
        }
        public override IEnumerator Execute()
        {
            print("Scale class: Execute Method");
            float elapsedTime = 0f;

            while (elapsedTime < _animationDuration && !_isSkipping)
            {
                float t = elapsedTime / _animationDuration;

                _targetRectTransform.localScale=Vector3.Lerp(_initialScale, _targetScale, Mathf.Pow(t, _animationExponent));
                print("Scale class: _targetRectTransform.localScale"+_targetRectTransform.localScale);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        public override IEnumerator Finalize()
        {
            print("Scale class: Finalize Method");
            _targetRectTransform.localScale=_targetScale;
            yield return base.Finalize();
        }
    }
}