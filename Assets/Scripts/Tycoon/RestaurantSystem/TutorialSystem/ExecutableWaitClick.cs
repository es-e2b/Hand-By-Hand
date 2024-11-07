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
        public override IEnumerator Begin()
        {
            _waitPanel.SetActive(true);
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
            yield return new WaitUntil(()=>Input.GetMouseButtonDown(0));
        }
        public override IEnumerator Skip()
        {
            throw new NotImplementedException();
        }
        public override IEnumerator Complete()
        {
            _waitPanel.SetActive(false);
            yield break;
        }
    }
}