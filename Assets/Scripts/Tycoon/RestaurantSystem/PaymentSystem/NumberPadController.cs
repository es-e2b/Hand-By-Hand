namespace Assets.Scripts.Tycoon.RestaurantSystem.PaymentSystem
{
    using System;
    using System.Collections;
    using System.Linq;
    using Assets.Scripts.SignLanguage;
    using UnityEngine;

    public class NumberPadController : MonoBehaviour
    {
        [SerializeField]
        private InputDisplayObject[] numberObjects;
        [SerializeField]
        private InputButton[] numberInputButtons=new InputButton[9];
        [SerializeField]
        private InputButton[] unitInputButtons=new InputButton[2];
        [SerializeField]
        private InputButton deleteButton;
        [SerializeField]
        private InputButton completeButton;
        [SerializeField]
        private GameObject customerImage;
        private UniversalTimer hintTimer;
        [SerializeField]
        private SignLanguageDictionary numberSignDictionary;
        [SerializeField]
        private float hintOpenTime;
        private int selectedIndex;
        public int SelectedIndex
        {
            get => selectedIndex;
            set
            {
                selectedIndex=value;
                for(int i=0; i<numberObjects.Length; i++)
                {
                    numberObjects[i].ToggleSelectedUI(i==value);
                }
                Array.ForEach(numberInputButtons, numberInputButton => numberInputButton.SetInteractable(value%2==0));
                Array.ForEach(unitInputButtons, unitInputButton => unitInputButton.SetInteractable(value%2==1));
            }
        }
        private void Start()
        {
            StartCoroutine(Initialize());
        }
        private IEnumerator Initialize()
        {
            yield return new WaitUntil(()=>PaymentManager.Instance!=null);
            PaymentManager.Instance.OnChangedTenThousandNumber.AddListener(number=>
                {
                numberObjects[0].AnswerValue=number;
                SelectedIndex=number!=0?0:2;
                }
            );
            PaymentManager.Instance.OnChangedTenThousandUnit.AddListener(number=>numberObjects[1].AnswerValue=number);
            PaymentManager.Instance.OnChangedThousandNumber.AddListener(number=>numberObjects[2].AnswerValue=number);
            PaymentManager.Instance.OnChangedThousandUnit.AddListener(number=>numberObjects[3].AnswerValue=number);
            hintTimer=new UniversalTimer(1, ()=>Array.ForEach(numberObjects, numberObject=>{/*numberObject.OpenHint()*/}), StartCoroutine, StopCoroutine);
            PaymentManager.Instance.OnChangedCustomer.AddListener(customer=>hintTimer.StartTimer(hintOpenTime));
            for(int i=1;i<=9;i++)
            {
                int inputNumber=i;
                numberInputButtons[i-1].InputAction=()=>InputInSelectedIndex(inputNumber);
            }
            unitInputButtons[0].transform.Find("Image").GetComponent<Animator>().SetInteger("Input Number", 10000);
            unitInputButtons[1].transform.Find("Image").GetComponent<Animator>().SetInteger("Input Number", 1000);
            unitInputButtons[0].InputAction=()=>InputInSelectedIndex(10000);
            unitInputButtons[1].InputAction=()=>InputInSelectedIndex(1000);
            deleteButton.InputAction=ResetInput;
            completeButton.InputAction=CheckAnswer;

            Array.ForEach(numberObjects, numberObject=>numberObject.SeletIndex=(index)=>SelectedIndex=index);
        }
        private void InputInSelectedIndex(int value)
        {
            numberObjects[SelectedIndex].InputValue=value;
        }
        private void ResetInput()
        {
            Array.ForEach(numberObjects, numberObject=>numberObject.InputValue=0);
        }
        private void CheckAnswer()
        {
            foreach(var numberObject in numberObjects)
            {
                if(numberObject.AnswerValue!=numberObject.InputValue)
                {
                    //아니다 애니메이션
                    ResetInput();
                    return;
                }
            }
            //계산 금액 애니메이션
            StartCoroutine(AnimateNumberSign());
            //맞다 애니메이션
            // PaymentManager.Instance.ReceivePayment();
        }
        private IEnumerator AnimateNumberSign()
        {
            int TenThousandNumber=numberObjects[0].AnswerValue;
            int TenThousandUnit=numberObjects[1].AnswerValue;
            print((TenThousandNumber*TenThousandUnit).ToString());
            Vocabulary vocabulary=numberSignDictionary[(TenThousandNumber*TenThousandUnit).ToString()];
            if(vocabulary!=null)
            {
                yield return SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(customerImage, vocabulary);
            }

            int ThousandNumber=numberObjects[2].AnswerValue;
            int ThousandUnit=numberObjects[3].AnswerValue;
            vocabulary=numberSignDictionary[(ThousandNumber*ThousandUnit).ToString()];
            if(vocabulary!=null)
            {
                yield return SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(customerImage, vocabulary);
            }
            ResetInput();
            PaymentManager.Instance.ReceivePayment();
        }
    }
}