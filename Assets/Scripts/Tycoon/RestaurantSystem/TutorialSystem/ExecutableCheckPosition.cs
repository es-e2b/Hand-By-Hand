namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;

    [Serializable]
    public class ExecutableCheckPosition : ExecutableElement
    {
        [SerializeField]
        private XDraggableUIAddEvent _xDraggableUI;
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
            bool currentPosition=true;
            _xDraggableUI.OnOpenPosition.AddListener((position)=>currentPosition=position);
            yield return new WaitUntil(()=>!currentPosition);
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