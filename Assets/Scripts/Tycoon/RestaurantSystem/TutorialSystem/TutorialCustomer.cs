namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System.Collections;
    using MenuData;
    using UnityEngine;
    using OrderSystem;
    using CharacterData;

    public class TutorialCustomer : Customer
    {
        public TutorialCustomer()
        {
            Character[] characterPool = GameManager.Instance.CharacterPool;
            CustomerCharacter = characterPool[1];
            CreateBasicMenus();
        }
        private void CreateBasicMenus()
        {
            int menuCount = 1;
            OrderMenus=new Menu[menuCount];
            Menu[] menus=OrderManager.Instance.MenuBoard;
            OrderMenus[0]=menus[2];
        }
    }
}