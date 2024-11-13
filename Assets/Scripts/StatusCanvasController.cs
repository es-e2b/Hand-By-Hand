namespace Assets.Scripts
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class StatusCanvasController : MonoBehaviour
    {
        public static StatusCanvasController Instance { get; private set; }
        [SerializeField]
        private GameObject _statusPanel;
        private UniversalTimer _dayTimer;
        [SerializeField]
        private int _dayTargetTime;
        private void Awake()
        {
            if(Instance!=null && Instance!=this)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(this);
            _dayTimer=new UniversalTimer(1, ()=>ChangeDayCycle(DayCycle.Day), StartCoroutine, StopCoroutine);
            SceneManager.sceneLoaded+=(_, _)=>GetComponent<Canvas>().worldCamera=Camera.main;
        }
        private void Start()
        {
            StartCoroutine(StartGame());
        }
        private IEnumerator StartGame()
        {
            yield return new WaitUntil(()=>GameManager.Instance!=null);
            GameManager.Instance.OnChangedDayCycle.AddListener(OnChangedDayCycle);
            yield return null;
            _statusPanel.SetActive(true);

        }
        private void OnChangedDayCycle(DayCycle dayCycle)
        {
            switch(dayCycle)
            {
                case DayCycle.Day:
                    // _dayTimer.StartTimer(_dayTargetTime, OnDayTimerIntervalElapsed);
                    GameManager.Instance.DailySales=0;
                    break;
                case DayCycle.Night:
                {
                    break;
                }
            }
        }
        private void ChangeDayCycle(DayCycle dayCycle)
        {
            //지금은 낮에서 밤으로, 밤에서 낮으로 변하는 화면이 없어서 enum 순서를 2씩 옮김
            GameManager.Instance.CurrentDayCycle=dayCycle;
        }
    }
}