namespace Assets.Scripts.Tycoon.RestaurantSystem
{
    using System.Collections;
    using MenuData;
    using UnityEngine;
    using OrderSystem;
    using CharacterData;

    public class Customer
    {
        public Menu[] OrderMenus;
        public Character CustomerCharacter;
        public Customer()
        {
            Character[] characterPool = GameManager.Instance.CharacterPool;
            CustomerCharacter = characterPool[UnityEngine.Random.Range(0, characterPool.Length)];
            CreateRandomOrderMenus();
        }
        private void CreateRandomOrderMenus()
        {
            int menuCount = UnityEngine.Random.Range(1, 5);
            OrderMenus=new Menu[menuCount];
            Menu[] menus=OrderManager.Instance.MenuBoard;
            for(int i=0;i<menuCount;i++)
            {
                OrderMenus[i]=menus[UnityEngine.Random.Range(0, menus.Length)];
            }
        }
        public IEnumerator Eat()
        {
            Debug.Log("EatEatEatEatEatEatEatEatEatEatEatEatEatEatEatEatEatEatEatEat");
            float totalEatingDuration = 0;

            for (int i = 0; i < OrderMenus.Length; i++)
            {
                totalEatingDuration += OrderMenus[i].EatingDuration;
            }

            yield return new WaitForSeconds(totalEatingDuration);
        }
    }
}