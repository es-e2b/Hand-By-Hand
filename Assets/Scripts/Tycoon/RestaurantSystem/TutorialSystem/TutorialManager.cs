namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using Assets.Scripts.Tycoon.RestaurantSystem.OrderSystem;
    using UnityEngine;

    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager Instance { get; private set; }
        [SerializeField]
        private ExecutableList _executableList;
        public bool IsTutorialMode;
        public Customer TutorialCustomer;
        [SerializeField]
        private GameObject _skipButton;
        [SerializeField]
        private GameObject _skipPromptPanel;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        private void Start()
        {
            StartCoroutine(StartTutorial());   
        }
        private IEnumerator StartTutorial()
        {
            yield return new WaitUntil(()=>GameManager.Instance!=null);
            if(GameManager.Instance.hasCompletedTutorial)
            {
                yield break;
            }
            ExecutableElement[] _executables=_executableList.Executables;
            for(int i=0;i<_executables.Length;i++)
            {
                yield return _executables[i].Initialize();
            }
            Restart();
        }
        public void OnTutorialMode()
        {
            IsTutorialMode=true;
        }
        public void Restart()
        {
            GameManager.Instance.CurrentDayCycle=DayCycle.Day;
        }
        public void GenerateTutorialCustomer()
        {
            TutorialCustomer=new TutorialCustomer();
            OrderManager.Instance.OrderingCustomer=TutorialCustomer;
            OrderManager.Instance.StopTimer();
            OrderManager.Instance.StartTimer(1000000);
        }
        public void RenderTutorialSignAnimation()
        {
            FindObjectOfType<OrderingCustomerController>().CheckOrderMenu(TutorialCustomer.OrderMenus[0], 0);
        }
        public void OpenSkipButton()
        {
            _skipButton.SetActive(true);
        }
        public void ToggleSkipPrompt(bool showPrompt)
        {
            _skipPromptPanel.SetActive(showPrompt);
        }
        public void StopTime(bool stop)
        {
            Time.timeScale=stop?0:1;
        }
    }
}