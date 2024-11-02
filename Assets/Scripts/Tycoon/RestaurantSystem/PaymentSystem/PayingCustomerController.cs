namespace Assets.Scripts.Tycoon.RestaurantSystem.PaymentSystem
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class PayingCustomerController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] paymentMenuObjects;
        [SerializeField]
        private GameObject customerImage;
        private Customer Customer
        {
            set
            {
                if(value == null)
                {
                    gameObject.SetActive(false);
                    return;
                }
                gameObject.SetActive(true);
                //위의 필드 다 적용하기
                customerImage.GetComponent<Image>().sprite=value.CustomerCharacter.CharacterSprite;
                ActivatePaymentMenu(value);
            }
        }
        private void Start()
        {
            StartCoroutine(Initailize());
        }
        private IEnumerator Initailize()
        {
            yield return new WaitUntil(()=>PaymentManager.Instance!=null);
            PaymentManager.Instance.OnChangedCustomer.AddListener((customer)=>Customer=customer);
            gameObject.SetActive(false);
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