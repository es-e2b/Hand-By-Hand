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
        public override IEnumerator Pause()
        {
            _parentPanel.SetActive(true);
            yield return new WaitForSeconds(_watingTime);
        }
        public override IEnumerator Complete()
        {
            _parentPanel.SetActive(false);
            yield break;
        }
    }
}