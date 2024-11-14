namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.UI;

    public class StartSceneManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject CreditPanel;

        public void ChangeScene(DayCycle dayCycle)
        {
            GameManager.Instance.CurrentDayCycle = dayCycle;
        }
        public void ChangeCartoonScene()
        {
            ChangeScene(DayCycle.Cartoon);
        }
        public void ChangeDayScene()
        {
            ChangeScene(DayCycle.Day);
        }
        public void ChangeNightScene()
        {
            ChangeScene(DayCycle.Night);
        }
        public void StartGame()
        {
            GameManager.Instance.CurrentDayCycle=(DayCycle)PlayerPrefs.GetInt("DayCycle", (int)DayCycle.Cartoon);
        }
        public void ResetGame()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("Day", 1);
            GameManager.Instance.HasCompletedTutorial=false;
            GameManager.Instance.CurrentDayCycle=DayCycle.Cartoon;
        }
        public void ToggleContinueButton(Button continueButton)
        {
            continueButton.interactable=PlayerPrefs.HasKey("DayCycle");
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