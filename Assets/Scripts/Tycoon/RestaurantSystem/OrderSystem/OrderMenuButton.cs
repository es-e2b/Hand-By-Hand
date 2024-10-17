namespace Assets.Scripts.Tycoon.RestaurantSystem.OrderSystem
{
    using UnityEngine;
    using UnityEngine.UI;
    using MenuData;
    using System;
    using UnityEngine.Events;

    public class OrderMenuButton : MonoBehaviour
    {
        private Menu menu;
        [SerializeField]
        private GameObject selectedUI;
        [SerializeField]
        private int orderIndex;
        [SerializeField]
        private GameObject questionMarkOjbect;
        public UnityAction<Menu, int> CheckOrderMenuAction;
        public Menu Menu
        {
            get => menu;
            set
            {
                menu=value;
                questionMarkOjbect.SetActive(true);
                GetComponent<Image>().sprite=menu.Sprite;
            }
        }
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Invoke);

            gameObject.SetActive(false);
        }
        private void Invoke()
        {
            CheckOrderMenuAction.Invoke(menu, orderIndex);
        }
        public void ToggleSelectedUI(int index)
        {
            selectedUI.SetActive(index == orderIndex);
        }
        public void RemoveQuestionMark()
        {
            questionMarkOjbect.SetActive(false);
        }
    }
}