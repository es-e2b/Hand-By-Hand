namespace Assets.Scripts.Tycoon.RestaurantSystem.PaymentSystem
{
    using System.Collections;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    public class PayingCustomerController : MonoBehaviour
    {
        private GameObject customerUI;
        [SerializeField]
        private GameObject[] paymentMenuObjects;
        [SerializeField]
        private GameObject customerImage;
        [SerializeField]
        private HintMessageObject hintMessageOjbect;
        private UniversalTimer hintMessageTimer;
        [SerializeField]
        private float hintMessageOpenTime;
        private Customer Customer
        {
            set
            {
                if(value == null)
                {
                    customerUI.SetActive(false);
                    hintMessageTimer.StopTimer();
                    return;
                }
                gameObject.SetActive(true);
                //위의 필드 다 적용하기
                customerImage.GetComponent<Image>().sprite=value.CustomerCharacter.CharacterSprite;
                hintMessageTimer.StartTimer(hintMessageOpenTime);
                ActivatePaymentMenu(value);
            }
        }
        private void Start()
        {
            customerUI=gameObject;
            hintMessageTimer=new UniversalTimer(1, ()=>hintMessageOjbect.Activate(true), StartCoroutine, StopCoroutine);
            PaymentManager.Instance.OnChangedCustomer.AddListener((customer)=>Customer=customer);
            PaymentManager.Instance.OnChangedTotalPrice.AddListener(hintMessageOjbect.UpdateTotalPrice);
            customerUI.SetActive(false);
        }
        //손님에 따라 체크 버튼에 메뉴를 적용하고 setActive(true or false) 하는 함수
        private void ActivatePaymentMenu(Customer customer)
        {
            for(int i=0;i<paymentMenuObjects.Length;i++)
            {
                if(i<customer.OrderMenus.Length)
                {
                    paymentMenuObjects[i].GetComponent<PaymentMenuObject>().Menu=customer.OrderMenus[i];
                    paymentMenuObjects[i].SetActive(true);
                    print(i);
                    continue;
                }
                paymentMenuObjects[i].SetActive(false);
            }
        }
    }
}