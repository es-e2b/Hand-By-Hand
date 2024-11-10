namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutableChangeColor : ExecutableElement
    {
        [SerializeField]
        private Image _targetImage;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private float _animationExponent=1;
        [SerializeField]
        private Color _targetColor;
        private Color _initialColor;
        public override IEnumerator Initialize()
        {
            print("Called Change Color Begin");
            _initialColor=_targetImage.color;
            yield return base.Initialize();
        }
        public override IEnumerator Execute()
        {
            float elapsedTime = 0f;

            while (elapsedTime < _animationDuration && !_isSkipping)
            {
                float t = elapsedTime / _animationDuration;

                _targetImage.color=Color.Lerp(_initialColor, _targetColor, Mathf.Pow(t, _animationExponent));

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield return base.Execute();
        }
        public override IEnumerator Finalize()
        {
            _targetImage.color=_targetColor;
            return base.Finalize();
        }
    }
}