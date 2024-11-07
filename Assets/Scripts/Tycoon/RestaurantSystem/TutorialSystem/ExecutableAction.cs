namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    [Serializable]
    public class ExecutableAction : ExecutableElement
    {
        [SerializeField]
        private bool isClickRequired;
        public UnityEvent _action;
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
            _action.Invoke();
            yield return Pause();
        }
        public override IEnumerator Pause()
        {
            if(isClickRequired)
            {
                yield return new WaitUntil(()=>Input.GetMouseButtonDown(0));
            }
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