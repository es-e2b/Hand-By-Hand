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
        private float _elapsedTime = 0f;
        public override IEnumerator Initialize()
        {
            _initialRotation=_targetRectTransform.eulerAngles;
            _rotationRate=Math.Abs(_initialRotation.z-_rotationMax.z)/(Math.Abs(_initialRotation.z-_rotationMax.z)+Math.Abs(_initialRotation.z-_rotationMin.z));
            yield return base.Initialize();
        }
        public override IEnumerator Next()
        {
            while(_elapsedTime < _animationDuration && !_isSkipping)
            {
                yield return Execute();
            }
            yield return Finalize();
            yield return Pause();
            yield return Complete();
        }
        public override IEnumerator Execute()
        {
            float currentTime = _elapsedTime;
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
                currentTime += Time.deltaTime;
                _elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        public override IEnumerator Finalize()
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
            yield return base.Finalize();
        }
    }
}