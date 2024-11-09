namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;

    [Serializable]
    public class ExecutableShake : ExecutableElement
    {
        [SerializeField]
        private RectTransform _targetRectTransform;
        [SerializeField]
        private float _animationDuration;
        [SerializeField]
        private float _shakeInterval=1;
        [SerializeField]
        private Vector3 _rotationMin;
        [SerializeField]
        private Vector3 _rotationMax;
        private Vector3 _initialRotation;
        private float _rotationRate;
        private float elapsedTime = 0f;
        public override IEnumerator Begin()
        {
            _initialRotation=_targetRectTransform.eulerAngles;
            _rotationRate=Math.Abs(_initialRotation.z-_rotationMax.z)/(Math.Abs(_initialRotation.z-_rotationMax.z)+Math.Abs(_initialRotation.z-_rotationMin.z));
            if(_skipButton!=null)
            {
                _skipButton.gameObject.SetActive(true);
                _skipButton.onClick.AddListener(OnClickSkipButton);
            }
            yield return Next();
        }
        public override IEnumerator Next()
        {
            while(elapsedTime < _animationDuration && !_isSkipping)
            {
                yield return Execute();
            }
            if(_isSkipping)
            {
                yield return Skip();
            }
            yield return Pause();
            yield return Complete();
        }
        public override IEnumerator Execute()
        {
            float currentTime = elapsedTime;
            while(currentTime>_shakeInterval)
            {
                currentTime-=_shakeInterval;
            }
            while (currentTime < _shakeInterval && !_isSkipping)
            {
                float t = currentTime / _shakeInterval;
                if(t<_rotationRate/2)
                {
                    t/=_rotationRate/2;
                    _targetRectTransform.eulerAngles=Vector3.Lerp(_initialRotation, _rotationMax, t);
                }
                else if(t>=_rotationRate/2 && t<_rotationRate)
                {
                    t-=_rotationRate/2;
                    t/=_rotationRate/2;
                    _targetRectTransform.eulerAngles=Vector3.Lerp(_rotationMax, _initialRotation,t);
                }
                else if(t>=_rotationRate && t<_rotationRate*3/2)
                {
                    t-=_rotationRate;
                    t/=(1-_rotationRate)/2;
                    _targetRectTransform.eulerAngles=Vector3.Lerp(_initialRotation, _rotationMin, t);
                }
                else
                {
                    t-=_rotationRate*3/2;
                    t/=(1-_rotationRate)/2;
                    _targetRectTransform.eulerAngles=Vector3.Lerp(_rotationMin, _initialRotation,t);
                }
                print(t+": "+_targetRectTransform.eulerAngles);
                currentTime += Time.deltaTime;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        public override IEnumerator Pause()
        {
            _isSkipping=false;
            float elapsedTime=0f;

            while (elapsedTime < _puaseDuration && !_isSkipping)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield break;
        }
        public override IEnumerator Skip()
        {
            yield break;
        }
        public override IEnumerator Complete()
        {
            float currentTime = _animationDuration;
            while(currentTime>_shakeInterval)
            {
                currentTime-=_shakeInterval;
            }
            float t = currentTime / _shakeInterval;
            if(t<_rotationRate/2)
            {
                _targetRectTransform.eulerAngles=Vector3.Lerp(_initialRotation, _rotationMax, t);
            }
            else if(t>=_rotationRate/2 && t<_rotationRate)
            {
                _targetRectTransform.eulerAngles=Vector3.Lerp(_rotationMax, _initialRotation,t);
            }
            else if(t>=_rotationRate && t<_rotationRate*3/2)
            {
                _targetRectTransform.eulerAngles=Vector3.Lerp(_initialRotation, _rotationMin, t);
            }
            else
            {
                _targetRectTransform.eulerAngles=Vector3.Lerp(_rotationMin, _initialRotation,t);
            }
            if(_skipButton!=null)
            {
                _skipButton.onClick.RemoveListener(OnClickSkipButton);
                _skipButton.gameObject.SetActive(false);
            }
            yield break;
        }
    }
}