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
        [SerializeField]
        private ExecutableList _restaurantTimerList;
        [SerializeField]
        private ExecutableList _restaurantInitializationList;
        [SerializeField]
        private ElementExecutor _executor;
        public bool IsTutorialMode;
        public Customer TutorialCustomer;
        [SerializeField]
        private GameObject _skipButton;
        [SerializeField]
        private GameObject _skipPromptPanel;
        [SerializeField]
        private GameObject _draggableUIObject;
        [SerializeField]
        private GameObject _menuButtonBlock;
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
            _executor.StartExecutableList(_restaurantInitializationList);
            if(GameManager.Instance.HasCompletedTutorial)
            {
                _executor.StartExecutableList(_restaurantTimerList);
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
            ObjectDisplayAnimator objectDisplayAnimator=FindObjectOfType<ObjectDisplayAnimator>();
            if(objectDisplayAnimator!=null)
            {
                StartCoroutine(HidePanelAndRestart(objectDisplayAnimator));
            }
            else
            {
                GameManager.Instance.CurrentDayCycle=DayCycle.Day;
            }
        }
        public void SetHasCompletedTutorial()
        {
            GameManager.Instance.HasCompletedTutorial=true;
        }
        private IEnumerator HidePanelAndRestart(ObjectDisplayAnimator objectDisplayAnimator)
        {
            yield return objectDisplayAnimator.HideMessageAnimation();
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
            if(showPrompt)
            {
                _skipPromptPanel.SetActive(showPrompt);
            }
            else
            {
                ObjectDisplayAnimator objectDisplayAnimator=_skipPromptPanel.GetComponentInChildren<ObjectDisplayAnimator>();
                if(objectDisplayAnimator!=null)
                {
                    StartCoroutine(HideSkipPanel(objectDisplayAnimator));
                }
                else
                {
                    _skipPromptPanel.SetActive(false);
                }
            }
        }
        public IEnumerator HideSkipPanel(ObjectDisplayAnimator objectDisplayAnimator)
        {
            yield return objectDisplayAnimator.HideMessageAnimation();
            objectDisplayAnimator.gameObject.SetActive(true);
            _skipPromptPanel.SetActive(false);
        }
        public void StopTime(bool stop)
        {
            Time.timeScale=stop?0:1;
        }
        public void SetActiveDraggableUIObject(bool onOff)
        {
            _draggableUIObject.SetActive(onOff);
        }
        public void SetActiveMenuButtonBlock(bool onOff)
        {
            _menuButtonBlock.SetActive(onOff);
        }
    }
}