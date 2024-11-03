namespace Assets.Scripts
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class StatusCanvasController : MonoBehaviour, ISafeAreaSetter
    {
        [SerializeField]
        private GameObject _statusPanel;
        // [SerializeField]
        // private GameObject _dayCycleImage;
        private UniversalTimer _dayTimer;
        [SerializeField]
        private int _dayTargetTime;

        public RectTransform PanelRectTransform => _statusPanel.GetComponent<RectTransform>();

        // [SerializeField]
        // private TMP_Text _dailySales;
        private void Awake()
        {
            DontDestroyOnLoad(this);
            _dayTimer=new UniversalTimer(1, ChangeDayCycle, StartCoroutine, StopCoroutine);
            // _dayTimer=new UniversalTimer(1, ()=>{}, StartCoroutine, StopCoroutine);
            SceneManager.sceneLoaded+=(_, _)=>GetComponent<Canvas>().worldCamera=Camera.main;
        }
        private void Start()
        {
            StartCoroutine(StartGame());
            ApplySafeArea();
        }
        private IEnumerator StartGame()
        {
            GameManager.Instance.OnChangedDayCycle.AddListener(OnChangedDayCycle);
            // GameManager.Instance.OnChangedDailySales.AddListener((dailySales)=>_dailySales.text=dailySales.ToString());
            GameManager.Instance.DailySales=0;
            GameManager.Instance.CurrentDayCycle=DayCycle.Day;
            yield return null;
            _statusPanel.SetActive(true);

        }
        private void OnChangedDayCycle(DayCycle dayCycle)
        {
            switch(dayCycle)
            {
                case DayCycle.Day:
                    // _dayCycleImage.GetComponent<RectTransform>().eulerAngles=Vector3.zero;
                    // _dayTimer.StartTimer(_dayTargetTime, OnDayTimerIntervalElapsed);
                    GameManager.Instance.DailySales=0;
                    break;
                case DayCycle.Night:
                {
                    break;
                }
            }
        }
        private void ChangeDayCycle()
        {
            //지금은 낮에서 밤으로, 밤에서 낮으로 변하는 화면이 없어서 enum 순서를 2씩 옮김
            GameManager.Instance.CurrentDayCycle=(DayCycle)(((int)GameManager.Instance.CurrentDayCycle + 1) % 4);
        }

        public void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;

            PanelRectTransform.sizeDelta = safeArea.size*(1080/safeArea.width);
            PanelRectTransform.anchoredPosition=safeArea.position;

            print(safeArea);
        }
        // private void OnDayTimerIntervalElapsed()
        // {
        //     _dayCycleImage.GetComponent<RectTransform>().eulerAngles+=new Vector3(0, 0, 180f/_dayTargetTime);
        // }
    }
}