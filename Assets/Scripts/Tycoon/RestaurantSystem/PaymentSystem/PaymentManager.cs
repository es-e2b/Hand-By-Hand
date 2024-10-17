namespace Assets.Scripts.Tycoon.RestaurantSystem.PaymentSystem
{
    using System.Collections.Generic;
    using UnityEngine;

    public class PaymentManager : MonoBehaviour
    {
        public static PaymentManager Instance { get; private set; }
        private Queue<Customer> WaitingCustomer;
        private Customer paymentCustomer;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            WaitingCustomer=new();
        }
        public void AddWaitingCustomer(Customer customer)
        {
            if(paymentCustomer==null)
            {
                paymentCustomer=customer;
                return;
            }
            WaitingCustomer.Enqueue(customer);
        }
    }
}