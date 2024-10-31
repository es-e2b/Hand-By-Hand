namespace Assets.Scripts.Tycoon.RestaurantSystem.PaymentSystem
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;

    public abstract class InputDisplayObject : MonoBehaviour
    {
        [SerializeField]
        protected int index;
        [SerializeField]
        protected GameObject selectedUI;
        [SerializeField]
        protected GameObject questionMarkOjbect;
        [SerializeField]
        protected GameObject answerImage;
        [SerializeField]
        protected GameObject inputImage;
        private int answerValue;
        public virtual int AnswerValue
        {
            get => answerValue;
            set
            {
                answerValue = value;
                if(value>0)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
                inputValue=0;
                answerImage.SetActive(false);
            }
        }
        private int inputValue;
        public virtual int InputValue
        {
            get => inputValue;
            set
            {
                print("Changed Input Value");
                inputValue = value;
                if(value == 0)
                {
                    questionMarkOjbect.SetActive(true);
                    return;
                }
                questionMarkOjbect.SetActive(false);
            }
        }
        protected virtual void Start()
        {
            GetComponent<Button>().onClick.AddListener(()=>SeletIndex.Invoke(index));
            selectedUI.SetActive(false);
            gameObject.SetActive(false);
        }
        public void ToggleSelectedUI(bool isActive)
        {
            selectedUI.SetActive(isActive);
        }
        public void OpenHint()
        {
            answerImage.SetActive(true);
        }
        public UnityAction<int> SeletIndex { get; set; }
    }
}