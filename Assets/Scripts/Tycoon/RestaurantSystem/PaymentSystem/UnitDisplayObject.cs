namespace Assets.Scripts.Tycoon.RestaurantSystem.PaymentSystem
{
    using UnityEngine;

    public class UnitDisplayObject : InputDisplayObject
    {
        public override int InputValue
        {
            get => base.InputValue;
            set
            {
                base.InputValue = value;
                if(value>0)
                {
                    inputImage.GetComponent<Animator>().SetInteger("Input Number", value);
                }
            }
        }
    }
}