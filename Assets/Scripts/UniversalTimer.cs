namespace Assets.Scripts
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class UniversalTimer
    {
        private readonly float _interval;
        private Coroutine _timerCoroutine;
        private readonly Func<IEnumerator, Coroutine> _startTimer;
        private readonly Action _onTimeEnded;
        private readonly Action<Coroutine> _stopTime;
        public UniversalTimer(float interval, Action onTimeEnded, Func<IEnumerator, Coroutine> startTimer, Action<Coroutine> stopTime)
        {
            _interval = interval;
            _startTimer = startTimer;
            _onTimeEnded = onTimeEnded;
            _stopTime = stopTime;
        }
        public void StartTimer(float targetTime)
        {
            _timerCoroutine = _startTimer.Invoke(TimerStart(targetTime));
        }
        private IEnumerator TimerStart(float targetTime)
        {
            float leftTime = targetTime;
            while (leftTime > 0)
            {
                yield return new WaitForSeconds(_interval);
                leftTime -= _interval;
            }
            _onTimeEnded.Invoke();
        }
        public void StopTimer()
        {
            if (_timerCoroutine != null)
            {
                _stopTime.Invoke(_timerCoroutine);
                _timerCoroutine = null; // 코루틴 중지 후 null로 설정
            }
        }
    }
}