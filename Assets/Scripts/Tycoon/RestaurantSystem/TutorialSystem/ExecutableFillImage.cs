namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutableFillImage : ExecutableElement
    {
        [SerializeField]
        private Image _targetImage;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private float _animationExponent=1;
        [SerializeField]
        private float _targetFillAmount;
        private float _initialFillAmount;
        public override IEnumerator Begin()
        {
            _initialFillAmount=_targetImage.fillAmount;
            yield return base.Begin();
        }
        public override IEnumerator Execute()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _animationDuration && !_isSkipping)
            {
                float t = elapsedTime / _animationDuration;

                _targetImage.fillAmount=Mathf.Lerp(_initialFillAmount, _targetFillAmount, Mathf.Pow(t, _animationExponent));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield return base.Execute();
        }
        public override IEnumerator Finalize()
        {
            _targetImage.fillAmount=_targetFillAmount;
            yield return base.Finalize();
        }
    }
}