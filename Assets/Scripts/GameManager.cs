namespace Assets.Scripts
{
    using UnityEngine;
    using CharacterData;
    using UnityEngine.Events;

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
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
        }
        private void Start()
        {
            DayCount=1;
        }
        [field:SerializeField]
        public UnityEvent<int> OnChangedDayCount { get; private set; }
        [field:SerializeField]
        public UnityEvent<int> OnChangedDailySales { get; private set; }
    }
}