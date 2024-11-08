namespace Assets.Scripts.Tycoon.RestaurantSystem.OrderSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using MenuData;
    using PaymentSystem;
    using UnityEngine.UI;
    using Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem;

    public class OrderManager : MonoBehaviour
    {
        public static OrderManager Instance { get; private set; }
        public List<Customer> EatingCustomer;
        [field: SerializeField]
        public Menu[] MenuBoard { get; private set; }
        [SerializeField]
        private GameObject MenuButtonPrefab;
        public int OrderIndex;
        public bool[] OrderChecks;
        /// <summary>
        /// 타이머는 손님과 함께 생성, 화면에 타이머를 SetActive(true)로 만들고, 퇴장할 때 SetActive(false)
        /// </summary>
        private SliderTimer orderTimer;
        [SerializeField]
        private Slider timerSlider;
        private Coroutine timerCoroutine;
        private Coroutine spawnCoroutine;
        public Coroutine SpawnCoroutine { get=>spawnCoroutine; }
        private Customer orderingCustomer;
        public Customer OrderingCustomer
        {
            get => orderingCustomer;
            set
            {
                print("Ordering customer setted some value");
                orderingCustomer = value;
                OnChangedCustomer.Invoke(value);
                if(value == null)
                {
                    print("XZXZXZXZXZXXXZXZXZX");
                    OnCustomerExit();
                    return;
                }
                OnCustomerEnter(value);
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
        }
        private void Start()
        {
            orderTimer=new SliderTimer(timerSlider, 1, OnTimerEnded);
            EatingCustomer=new();
            StartCoroutine(Initailize());
        }
        private IEnumerator Initailize()
        {
            yield return new WaitUntil(()=>GameManager.Instance!=null);
            if(!GameManager.Instance.hasCompletedTutorial)
            {
                yield return new WaitUntil(()=>TutorialManager.Instance!=null);
                yield break;
            }
            spawnCoroutine=StartCoroutine(SpawnCustomer());
        }
        private void OnTimerEnded()
        {
            OrderingCustomer=null;
        }
        public void StopTimer()
        {
            StopCoroutine(timerCoroutine);
            timerSlider.gameObject.SetActive(false);
        }
        /// <summary>
        /// 타이머 시작
        /// </summary>
        public void StartTimer(int time)
        {
            timerCoroutine=StartCoroutine(orderTimer.TimerStart(time));
        }
        public void OnCustomerEnter(Customer customer)
        {
            OrderChecks=new bool[OrderingCustomer.OrderMenus.Length];
            StartTimer(60);
        }
        /// <summary>
        /// 손님 퇴장(식사 시작, 퇴장) 시 사용하는 함수.
        /// 타이머 종료, 손님 생성 함수
        /// </summary>
        public void OnCustomerExit()
        {
            OrderIndex=-1;
            StopTimer();
            if(spawnCoroutine!=null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine=null;
            }
            if(!TutorialManager.Instance.IsTutorialMode)
            {
                spawnCoroutine=StartCoroutine(SpawnCustomer());
            }
        }
        private IEnumerator StartEating(Customer customer)
        {
            EatingCustomer.Add(customer);
            OrderingCustomer = null;
            yield return customer.Eat();
            EatingCustomer.Remove(customer);
            PaymentManager.Instance.AddWaitingCustomer(customer);
        }
        public void OrderCheck(Menu menu)
        {
            if(OrderIndex==-1)
            {
                return;
            }
            if(OrderingCustomer.OrderMenus[OrderIndex]!=menu)
            {
                OnIncorrectAnswer.Invoke();
                return;
            }
            print("Called Order Check Method");
            OnCorrectAnswer.Invoke(OrderIndex);
        }
        public IEnumerator OnAllCorrectAnswer()
        {
            Debug.Log("OnAllCorrectAnswerOnAllCorrectAnswerOnAllCorrectAnswerOnAllCorrectAnswerOnAllCorrectAnswerOnAllCorrectAnswer");
            StartCoroutine(StartEating(OrderingCustomer));
            yield return null;
        }
        private IEnumerator SpawnCustomer()
        {
            print("Call Spawn Customer Method");

            float spawnProbability = 0f;

            while (OrderingCustomer==null)
            {
                yield return new WaitForSeconds(1f);

                spawnProbability += 0.2f;

                print("Spawn Probability is "+spawnProbability);

                if (Random.value <= spawnProbability)
                {
                    OrderingCustomer=new Customer();
                    yield break;
                }
            }
        }

        public UnityEvent<Customer> OnChangedCustomer;
        public UnityEvent<int> OnCorrectAnswer;
        public UnityEvent OnIncorrectAnswer;
    }
}