namespace Assets.Scripts
{
    using UnityEngine;
    using CharacterData;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;
    using Tycoon.RestaurantSystem.TutorialSystem;
    using System.Collections;
    using HandByHand.SoundSystem;
    using UnityEngine.UI;

    public class GameManager : MonoBehaviour
    {
        [field:SerializeField]
        private int StartDayCount { get; set; }
        public static GameManager Instance { get; private set; }
        public bool _hasCompletedTutorial;
        public bool HasCompletedTutorial
        {
            get=>_hasCompletedTutorial;
            set
            {
                _hasCompletedTutorial=value;
                PlayerPrefs.SetInt("HasCompletedTutorial", value?1:0);
            }
        }
        [SerializeField]
        private DayCycle _currentDayCycle;
        public DayCycle CurrentDayCycle
        {
            get => _currentDayCycle;
            set
            {
                _currentDayCycle=value;
                OnChangedDayCycle.Invoke(value);
                SceneManager.LoadScene((int)value);
                if(value==DayCycle.Day || value==DayCycle.Night)
                {
                    PlayerPrefs.SetInt("DayCycle", (int)value);
                }
                SoundManager.Instance.StopBGM();
                SoundManager.Instance.StopSE();
            }
        }
        public Character[] CharacterPool;
        private int _dayCount;
        public int DayCount
        {
            get => _dayCount;
            set
            {
                _dayCount = value;
                OnChangedDayCount.Invoke(value);
                if(PlayerPrefs.HasKey("Day"))
                {
                    PlayerPrefs.SetInt("Day", value);
                }
            }
        }
        private int _dailySales;
        public int DailySales
        {
            get => _dailySales;
            set
            {
                _dailySales = value;
                OnChangedDailySales.Invoke(value);
            }
        }
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
            OnChangedDayCount=new();
            OnChangedDailySales=new();
            OnChangedDayCycle=new();
        }
        private void Start()
        {
            HasCompletedTutorial= PlayerPrefs.GetInt("HasCompletedTutorial", 0) != 0;
        }
        public void ExitGame()
        {
            Application.Quit();
        }
        public void StartExecutableList(ExecutableList executableList)
        {
            StartCoroutine(ExecuteExecutableList(executableList));
        }
        private IEnumerator ExecuteExecutableList(ExecutableList executableList)
        {
            ExecutableElement[] executables=executableList.Executables;
            for(int i=0;i<executables.Length;i++)
            {
                yield return executables[i].Initialize();
            }
        }
        public void ChangeCartoonScene()
        {
            CurrentDayCycle=DayCycle.Cartoon;
        }
        public void ChangeDayScene()
        {
            CurrentDayCycle=DayCycle.Day;
        }
        public void ChangeNightScene()
        {
            CurrentDayCycle=DayCycle.Night;
        }

        public UnityEvent<int> OnChangedDayCount { get; private set; }
        public UnityEvent<int> OnChangedDailySales { get; private set; }
        public UnityEvent<DayCycle> OnChangedDayCycle { get; private set; }
    }
    public enum DayCycle
    {
        Start,
        Cartoon,
        Day,
        Night,
    }
}