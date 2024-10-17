namespace Assets.Scripts.Tycoon.RestaurantSystem
{
    using UnityEngine;

    public class RestaurantManager : MonoBehaviour
    {
        public static RestaurantManager Instance { get; private set; }
        public int TotalEarnedMoney;
        public float RemainingTime;
        [SerializeField]
        private float startTime;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    }
}