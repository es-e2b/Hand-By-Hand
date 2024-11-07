namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;

    [Serializable]
    public class ExecutableDisplayer : ExecutableElement
    {
        [SerializeField]
        private GameObject _displayPanel;
        public override IEnumerator Begin()
        {
            _displayPanel.SetActive(true);
            yield return Next();
        }
        public override IEnumerator Next()
        {
            yield return Execute();
            yield return Complete();
        }
        public override IEnumerator Execute()
        {
            yield return null;
        }
        public override IEnumerator Pause()
        {
            throw new NotImplementedException();
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