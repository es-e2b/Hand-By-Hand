namespace Assets.Scripts
{
    using UnityEngine;

    public class TimedDisable : MonoBehaviour
    {
        [SerializeField]
        protected float _targetTime=1.5f;
        protected UniversalTimer _timer;
        protected virtual void Awake()
        {
            _timer=new UniversalTimer(0.1f, ()=>gameObject.SetActive(false), StartCoroutine, StopCoroutine);
        }
        protected virtual void OnEnable()
        {
            _timer.StartTimer(_targetTime);
        }
        protected virtual void OnDisable()
        {
            _timer.StopTimer();    
        }
    }
}