namespace Assets.Scripts
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class SliderTimer
    {
        private readonly Slider slider;
        private readonly float interval;
        private readonly Action onTimeEnded;
        public SliderTimer(Slider slider, float interval, Action onTimeEnded)
        {
            this.slider = slider;
            this.interval = interval;
            this.onTimeEnded = onTimeEnded;
        }
        public IEnumerator TimerStart(float targetTime)
        {
            slider.gameObject.SetActive(true);
            slider.maxValue = targetTime;
            float leftTime = targetTime;
            while(leftTime>0)
            {
                slider.value = leftTime;
                yield return new WaitForSeconds(interval);
                leftTime -= interval;
            }
            onTimeEnded.Invoke();
        }
    }
}