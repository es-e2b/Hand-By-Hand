namespace Assets.Scripts
{
    using System;
    using System.Collections;
    using UnityEngine;

    public class UniversalTimer
    {
        private readonly float interval;
        private readonly Action<float> onTimeElapsed;
        private readonly Action onTimeEnded;
        public UniversalTimer(float interval, Action<float> onTimeElapsed, Action onTimeEnded)
        {
            this.interval = interval;
            this.onTimeElapsed = onTimeElapsed;
            this.onTimeEnded = onTimeEnded;
        }
        public IEnumerator TimerStart(float targetTime)
        {
            float leftTime = targetTime;
            while(leftTime>0)
            {
                onTimeElapsed.Invoke(leftTime);
                yield return new WaitForSeconds(interval);
                leftTime -= interval;
            }
            onTimeEnded.Invoke();
        }
    }
}