namespace Assets.Scripts.Cartoon
{
    using System.Collections;
    using Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem;
    using UnityEngine;

    public class CartoonSceneManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _connectionCanvas;
        public void SetActiveConnectionCanvas()
        {
            _connectionCanvas.SetActive(true);
        }
    }
}