namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    [Serializable]
    public class ExecutableWaitForSeconds : ExecutableElement
    {
        [SerializeField]
        private GameObject _parentPanel;
        [SerializeField]
        private float _watingTime;
        public override IEnumerator Begin()
        {
            _parentPanel.SetActive(true);
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
            yield return new WaitForSeconds(_watingTime);
        }
        public override IEnumerator Skip()
        {
            throw new NotImplementedException();
        }
        public override IEnumerator Complete()
        {
            _parentPanel.SetActive(false);
            yield break;
        }
    }
}