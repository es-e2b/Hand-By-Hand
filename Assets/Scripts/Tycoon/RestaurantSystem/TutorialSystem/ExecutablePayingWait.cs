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
        public override IEnumerator Pause()
        {
            yield return new WaitUntil(()=>PaymentManager.Instance.PayingCustomer==null);
        }
    }
}