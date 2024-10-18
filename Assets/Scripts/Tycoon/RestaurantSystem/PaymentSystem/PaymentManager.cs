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
                OnChangedTotalPrice.Invoke(value);
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
                    StartCoroutine(WaitPayingCustomer());
                    return;
                }
                TotalPrice=value.OrderMenus.Sum(menu => menu.Price);
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
            OnChangedTotalPrice=new();
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
        public UnityEvent<Customer> OnChangedCustomer { get; private set; }
        public UnityEvent<int> OnChangedTotalPrice { get; private set; }
    }
}