namespace Assets.Scripts.Tycoon.RestaurantSystem.PaymentSystem
{
    using UnityEngine;
    using UnityEngine.UI;
    using MenuData;
    using TMPro;

    public class PaymentMenuObject : MonoBehaviour
    {
        private Menu menu;
        [SerializeField]
        private Image menuImage;
        [SerializeField]
        private TMP_Text menuPrice;
        public Menu Menu
        {
            get => menu;
            set
            {
                menu=value;
                menuImage.sprite=value.Sprite;
                menuPrice.text=value.Price.ToString();
            }
        }
    }
}