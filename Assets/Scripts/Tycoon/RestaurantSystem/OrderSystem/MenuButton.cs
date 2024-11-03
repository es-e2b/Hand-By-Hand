namespace Assets.Scripts.Tycoon.RestaurantSystem.OrderSystem
{
    using MenuData;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class MenuButton : MonoBehaviour
    {
        private Menu menu;
        [SerializeField]
        private TMP_Text menuName;
        [SerializeField]
        private Image menuImage;
        public Menu Menu
        {
            get => menu;
            set
            {
                menu = value;
                menuImage.sprite = value.Sprite;
                menuName.text = value.Name;
                GetComponent<Button>().onClick.AddListener(()=>OrderManager.Instance.OrderCheck(Menu));
            }
        }
    }
}