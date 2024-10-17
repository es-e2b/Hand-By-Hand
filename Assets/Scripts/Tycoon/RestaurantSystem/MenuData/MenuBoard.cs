namespace Assets.Scripts.Tycoon.RestaurantSystem.MenuData
{
    using UnityEngine;
    using UnityEngine.UI;
    using OrderSystem;

    public class MenuBoard : MonoBehaviour
    {
        public GameObject FoodPanel;
        public GameObject BeveragePanel;
        public GameObject MenuButtonPrefab;
        private void Start()
        {
            CreateMenuButton(OrderManager.Instance.MenuBoard);
        }
        private void CreateMenuButton(params Menu[] menus)
        {
            foreach(var menu in menus)
            {
                GameObject menuButton=null;
                switch(menu.MenuType)
                {
                    case MenuType.Food:
                    menuButton = Instantiate(MenuButtonPrefab, FoodPanel.transform);
                    break;
                    case MenuType.Beverage:
                    menuButton = Instantiate(MenuButtonPrefab, BeveragePanel.transform);
                    break;
                    default:
                    Debug.LogError("Unknown MenuType: " + menu.MenuType);
                    continue;
                }
                menuButton.GetComponent<MenuButton>().Menu=menu;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(FoodPanel.transform.parent.GetComponent<RectTransform>());
        }
    }
}