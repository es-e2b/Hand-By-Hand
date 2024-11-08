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
        private float _targetFillAmount;
        private float _initialFillAmount;
        public override IEnumerator Execute()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _animationDuration && !_isSkipping)
            {
                float t = elapsedTime / _animationDuration;

                _targetImage.fillAmount=Mathf.Lerp(_initialFillAmount, _targetFillAmount, t*t*t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _targetImage.fillAmount=_targetFillAmount;
            if(_isSkipping)
            {
                yield return Skip();
            }
            yield return Pause();
        }
    }
}