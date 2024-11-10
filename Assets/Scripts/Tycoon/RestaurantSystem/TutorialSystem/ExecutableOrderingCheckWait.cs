namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using Assets.Scripts.Tycoon.RestaurantSystem.OrderSystem;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutableOrderingCheckWait : ExecutableElement
    {
        public override IEnumerator Pause()
        {
            yield return new WaitUntil(()=>OrderManager.Instance.OrderingCustomer==null);
        }
    }
}