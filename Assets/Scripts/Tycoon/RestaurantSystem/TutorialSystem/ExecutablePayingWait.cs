namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using Assets.Scripts.Tycoon.RestaurantSystem.OrderSystem;
    using Assets.Scripts.Tycoon.RestaurantSystem.PaymentSystem;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutablePayingWait : ExecutableElement
    {
        public override IEnumerator Begin()
        {
            yield return Next();
        }
        public override IEnumerator Next()
        {
            yield return Execute();
            yield return Complete();
        }
        public override IEnumerator Execute()
        {
            yield return Pause();
        }
        public override IEnumerator Pause()
        {
            yield return new WaitUntil(()=>PaymentManager.Instance.PayingCustomer==null);
        }
        public override IEnumerator Skip()
        {
            throw new NotImplementedException();
        }
        public override IEnumerator Complete()
        {
            yield break;
        }
    }
}