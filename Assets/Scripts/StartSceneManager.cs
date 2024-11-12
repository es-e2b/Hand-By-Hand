namespace Assets.Scripts
{
    using System.Collections;
    using Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem;
    using UnityEngine;

    public class StartSceneManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject CreditPanel;

        public void ChangeScene(DayCycle dayCycle)
        {
            GameManager.Instance.CurrentDayCycle=dayCycle;
        }

        public void CreditOnOff()
        {
            if(CreditPanel.activeSelf)
            {
                CreditPanel.SetActive(false);
            }
            else
            {
                CreditPanel.SetActive(true);
            }
        }   
    }
}