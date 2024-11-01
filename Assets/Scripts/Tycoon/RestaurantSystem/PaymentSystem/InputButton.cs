namespace Assets.Scripts.Tycoon.RestaurantSystem.PaymentSystem
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class InputButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(()=>InputAction.Invoke());
        }
        public void SetInteractable(bool isInteractable)
        {
            GetComponent<Button>().interactable = isInteractable;
        }
        public UnityAction InputAction { get; set; }
    }
}