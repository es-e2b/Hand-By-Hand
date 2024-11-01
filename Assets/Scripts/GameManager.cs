namespace Assets.Scripts
{
    using UnityEngine;
    using CharacterData;
    using UnityEngine.Events;

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        private DayCycle _currentDayCycle;
        public DayCycle CurrentDayCycle
        {
            get => _currentDayCycle;
            set
            {
                _currentDayCycle=value;
                OnChangedDayCycle.Invoke(value);
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
            DayCount=1;
        }
        public UnityEvent<int> OnChangedDayCount { get; private set; }
        public UnityEvent<int> OnChangedDailySales { get; private set; }
        public UnityEvent<DayCycle> OnChangedDayCycle { get; private set; }
    }
    public enum DayCycle
    {
        Day,
        DayToNight,
        Night,
        NightToDay,
    }
}