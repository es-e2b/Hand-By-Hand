namespace Assets.Scripts.Tycoon.RestaurantSystem.PaymentSystem
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.Events;

    public class PaymentManager : MonoBehaviour
    {
        public static PaymentManager Instance { get; private set; }
        private Queue<Customer> WaitingCustomer;
        private int _totalPrice;
        private int TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
                
                int currentTenThousand = _totalPrice / 10000;
                OnChangedTenThousandNumber.Invoke(currentTenThousand);
                OnChangedTenThousandUnit.Invoke(currentTenThousand!=0?10000:0);

                print(currentTenThousand+"TenThousand");

                int currentThousand = _totalPrice % 10000 / 1000;
                OnChangedThousandNumber.Invoke(currentThousand);
                OnChangedThousandUnit.Invoke(currentThousand!=0?1000:0);

                print(currentThousand+"Thousand");
            }
        }
        private Customer _payingCustomer;
        public Customer PayingCustomer
        {
            get => _payingCustomer;
            set
            {
                _payingCustomer=value;
                OnChangedCustomer.Invoke(value);
                if(value==null)
                {
                    TotalPrice=0;
                    StartCoroutine(WaitPayingCustomer());
                    return;
                }
                TotalPrice=value.OrderMenus.Sum(menu => menu.Price);
                print(TotalPrice);
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
            WaitingCustomer=new();
            OnChangedCustomer=new();
            OnChangedTenThousandNumber=new();
            OnChangedTenThousandUnit=new();
            OnChangedThousandNumber=new();
            OnChangedThousandUnit=new();
        }
        private void Start()
        {
            StartCoroutine(WaitPayingCustomer());
        }
            public void AddWaitingCustomer(Customer customer)
        {
            WaitingCustomer.Enqueue(customer);
        }
        private IEnumerator WaitPayingCustomer()
        {
            yield return new WaitUntil(() => WaitingCustomer.Count>0);
            PayingCustomer=WaitingCustomer.Dequeue();
        }
        public void ReceivePayment()
        {
            GameManager.Instance.DailySales+=TotalPrice;
            PayingCustomer=null;
        }
        public UnityEvent<Customer> OnChangedCustomer { get; private set; }
        public UnityEvent<int> OnChangedTenThousandNumber { get; private set; }
        public UnityEvent<int> OnChangedTenThousandUnit { get; private set; }
        public UnityEvent<int> OnChangedThousandNumber { get; private set; }
        public UnityEvent<int> OnChangedThousandUnit { get; private set; }
    }
}