namespace Assets.Scripts
{
    using System.Collections;
    using Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem;
    using UnityEngine;

    public class ConnectionManager : MonoBehaviour
    {
        [SerializeField]
        private ExecutableList _dayExecutableList;
        [SerializeField]
        private ExecutableList _nightExecutableList;
        [SerializeField]
        private Canvas _rootCanvas;
        [SerializeField]
        private DayCycle _testDayCycle;

        private void Start()
        {
            _rootCanvas.worldCamera=Camera.main;
            if(GameManager.Instance==null)
            {
                print("Game Manager is null");
                if(_testDayCycle==DayCycle.Night)
                {
                    FindObjectOfType<ElementExecutor>().StartExecutableList(_nightExecutableList);    
                }
                else
                {
                    FindObjectOfType<ElementExecutor>().StartExecutableList(_dayExecutableList);
                }
            }
            else if(GameManager.Instance.CurrentDayCycle==DayCycle.Day)
            {
                print("Game Manager Curren Cycle is Day");
                FindObjectOfType<ElementExecutor>().StartExecutableList(_dayExecutableList);
            }
            else
            {
                print("Game Manager Curren Cycle is Night");
                FindObjectOfType<ElementExecutor>().StartExecutableList(_nightExecutableList);
            }
        }
        public void ChangeDayScene()
        {
            GameManager.Instance.CurrentDayCycle=DayCycle.Day;
        }
        public void ChangeNightScene()
        {
            GameManager.Instance.CurrentDayCycle=DayCycle.Night;
        }
    }
}