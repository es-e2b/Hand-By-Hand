namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;

    [Serializable]
    public class ExecutableChangeAlpha : ExecutableElement
    {
        [SerializeField]
        private Color _targetColor;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private float _animationExponent=1;
        [SerializeField]
        private float _targetAlpha;
        private float _initialAlpha;
        public override IEnumerator Begin()
        {
            _initialAlpha=_targetColor.a;
            yield return Begin();
        }
        public override IEnumerator Execute()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _animationDuration && !_isSkipping)
            {
                float t = elapsedTime / _animationDuration;

                _targetColor.a=Mathf.Lerp(_initialAlpha, _targetAlpha, Mathf.Pow(t, _animationExponent));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
           _targetColor.a=_targetAlpha;
            if(_isSkipping)
            {
                yield return Skip();
            }
            yield return Pause();
        }
    }
}