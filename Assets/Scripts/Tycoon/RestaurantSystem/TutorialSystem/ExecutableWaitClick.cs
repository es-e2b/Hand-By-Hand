namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using Assets.Scripts.Tycoon.RestaurantSystem.OrderSystem;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutableWaitClick : ExecutableElement
    {
        [SerializeField]
        private GameObject _waitPanel;
        public override IEnumerator Pause()
        {
            _waitPanel.SetActive(true);
            yield return new WaitUntil(()=>Input.GetMouseButtonDown(0));
            yield return base.Pause();
        }
        public override IEnumerator Complete()
        {
            _waitPanel.SetActive(false);
            yield break;
        }
    }
}