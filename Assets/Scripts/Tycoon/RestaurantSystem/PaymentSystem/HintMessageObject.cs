namespace Assets.Scripts.Tycoon.RestaurantSystem.PaymentSystem
{
    using UnityEngine;
    using TMPro;

    public class HintMessageObject : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _totalPrice;
        private void Start()
        {
            gameObject.SetActive(false);
        }
        public void UpdateTotalPrice(int totalprice)
        {
            _totalPrice.text=totalprice.ToString();
        }
        public void Activate(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}