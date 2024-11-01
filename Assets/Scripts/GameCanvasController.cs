namespace Assets.Scripts
{
    using TMPro;
    using UnityEngine;

    public class GameCanvasController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _DayCycleImage;
        private UniversalTimer _dayTimer;
        [SerializeField]
        private int _dayTargetTime;
        [SerializeField]
        private TMP_Text _dailySales;
        private void Awake()
        {
            DontDestroyOnLoad(this);
            // _dayTimer=new UniversalTimer(1, ChangeDayCycle, StartCoroutine, StopCoroutine);
            _dayTimer=new UniversalTimer(1, ()=>{}, StartCoroutine, StopCoroutine);
        }
        private void Start()
        {
            GameManager.Instance.OnChangedDayCycle.AddListener(OnChangedDayCycle);
            GameManager.Instance.OnChangedDailySales.AddListener((dailySales)=>_dailySales.text=dailySales.ToString());
            GameManager.Instance.CurrentDayCycle=DayCycle.Day;
        }
        private void OnChangedDayCycle(DayCycle dayCycle)
        {
            switch(dayCycle)
            {
                case DayCycle.Day:
                    _DayCycleImage.GetComponent<RectTransform>().eulerAngles=Vector3.zero;
                    _dayTimer.StartTimer(_dayTargetTime, OnDayTimerIntervalElapsed);
                    GameManager.Instance.DailySales=0;
                    break;
            }
        }
        private void ChangeDayCycle()
        {
            //지금은 낮에서 밤으로, 밤에서 낮으로 변하는 화면이 없어서 enum 순서를 2씩 옮김
            GameManager.Instance.CurrentDayCycle=(DayCycle)(((int)GameManager.Instance.CurrentDayCycle + 2) % 4);
        }
        private void OnDayTimerIntervalElapsed()
        {
            _DayCycleImage.GetComponent<RectTransform>().eulerAngles+=new Vector3(0, 0, 180f/_dayTargetTime);
        }
    }
}