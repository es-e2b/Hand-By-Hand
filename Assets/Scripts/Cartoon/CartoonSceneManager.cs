namespace Assets.Scripts.Cartoon
{
    using System.Collections;
    using Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem;
    using UnityEngine;

    public class CartoonSceneManager : MonoBehaviour
    {
        [SerializeField]
        private ExecutableList _executableList;
        private void Start()
        {
            
        }
        public void ChangeDayScene()
        {
            GameManager.Instance.CurrentDayCycle=DayCycle.Day;
        }
    }
}