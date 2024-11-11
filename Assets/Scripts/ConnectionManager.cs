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

        private void Start()
        {
            StartCoroutine(Initialize());
            _rootCanvas.worldCamera=Camera.main;
        }
        private IEnumerator Initialize()
        {
            yield return new WaitUntil(()=>GameManager.Instance!=null);
            if(GameManager.Instance.CurrentDayCycle==DayCycle.Night)
            {
                FindObjectOfType<ElementExecutor>().StartExecutableList(_nightExecutableList);
            }
            else
            {
                FindObjectOfType<ElementExecutor>().StartExecutableList(_dayExecutableList);
            }
        }
    }
}