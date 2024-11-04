namespace Assets.Scripts.Tycoon.RestaurantSystem.OrderSystem
{
    using System.Collections;
    using Assets.Scripts.SignLanguage;
    using Assets.Scripts.Tycoon.RestaurantSystem.MenuData;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class OrderingCustomerController : MonoBehaviour
    {
        [SerializeField]
        private GameObject customerUI;
        [SerializeField]
        private GameObject[] orderMenuButtons;
        [SerializeField]
        private GameObject customerImage;
        [SerializeField]
        private Button replayButton;
        [SerializeField]
        private Button hintButton;
        [SerializeField]
        private GameObject hintMessageOjbect;
        private Customer customer;
        private Menu currentMenu;
        public Menu CurrentMenu
        {
            get => currentMenu;
            set
            {
                currentMenu=value;
                hintMessageOjbect.GetComponentInChildren<TMP_Text>().text = value.Vocabulary.Hint;
                hintMessageOjbect.SetActive(false);
            }
        }
        public Customer Customer
        {
            get => customer;
            set
            {
                customer=value;
                if(value == null)
                {
                    customerUI.SetActive(false);
                    return;
                }
                customerUI.SetActive(true);
                //위의 필드 다 적용하기
                customerImage.GetComponent<Image>().sprite=value.CustomerCharacter.CharacterSprite;
                CheckOrderMenu(value.OrderMenus[0], 0);
                ActivateOrderMenu(value);
            }
        }
        private void Start()
        {
            StartCoroutine(Initailize());
        }
        private IEnumerator Initailize()
        {
            yield return new WaitUntil(()=>OrderManager.Instance!=null);
            OrderManager.Instance.OnChangedCustomer.AddListener((customer)=>Customer=customer);
            OrderManager.Instance.OnCorrectAnswer.AddListener((index)=>
            {
                orderMenuButtons[index].GetComponent<OrderMenuButton>().RemoveQuestionMark();
                if(++index>=Customer.OrderMenus.Length)
                {
                    OrderManager.Instance.OnAllCrrectAnswer();
                    return;
                }
                orderMenuButtons[index].GetComponent<OrderMenuButton>().CheckOrderMenuAction.Invoke(Customer.OrderMenus[index], index);
            });
            foreach(GameObject orderMenuButton in orderMenuButtons)
            {
                orderMenuButton.GetComponent<OrderMenuButton>().CheckOrderMenuAction=CheckOrderMenu;
            }
            replayButton.GetComponent<Button>().onClick.AddListener(()=>
            {
                if(Customer==null)return;
                StartCoroutine(SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(customerImage, currentMenu.Vocabulary));
            });
            hintButton.GetComponent<Button>().onClick.AddListener(()=>
            {
                if(CurrentMenu==null) return;
                hintMessageOjbect.SetActive(false);
                hintMessageOjbect.SetActive(true);
            });
        }
        private void CheckOrderMenu(Menu menu, int index)
        {
            StartCoroutine(SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(customerImage, menu.Vocabulary));
            OrderManager.Instance.OrderIndex=index;
            foreach(GameObject orderMenuButton in orderMenuButtons)
            {
                orderMenuButton.GetComponent<OrderMenuButton>().ToggleSelectedUI(index);
            }
            CurrentMenu=menu;
        }
        //손님에 따라 체크 버튼에 메뉴를 적용하고 setActive(true or false) 하는 함수
        private void ActivateOrderMenu(Customer customer)
        {
            for(int i=0;i<orderMenuButtons.Length;i++)
            {
                print("Order Menu Index");
                if(i<customer.OrderMenus.Length)
                {
                    orderMenuButtons[i].GetComponent<OrderMenuButton>().Menu=customer.OrderMenus[i];
                    orderMenuButtons[i].SetActive(true);
                    continue;
                }
                orderMenuButtons[i].SetActive(false);
            }
        }
    }
}