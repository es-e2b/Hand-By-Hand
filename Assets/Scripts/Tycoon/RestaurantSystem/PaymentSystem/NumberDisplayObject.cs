namespace Assets.Scripts.Tycoon.RestaurantSystem.PaymentSystem
{
    using UnityEngine;
    using UnityEngine.UI;

    public class NumberDisplayObject : InputDisplayObject
    {
        [SerializeField]
        private Sprite[] numberSigns=new Sprite[10];
        public override int AnswerValue
        {
            get => base.AnswerValue;
            set
            {
                base.AnswerValue = value;
                answerImage.GetComponent<Image>().sprite = numberSigns[value];
            }
        }
        public override int InputValue
        {
            get => base.InputValue;
            set
            {
                base.InputValue = value;
                print("InputValue is "+value);
                inputImage.GetComponent<Image>().sprite = numberSigns[value];
            }
        }
    }
}