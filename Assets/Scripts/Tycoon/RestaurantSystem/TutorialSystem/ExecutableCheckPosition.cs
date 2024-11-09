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
        public override IEnumerator Pause()
        {
            bool currentPosition=true;
            _xDraggableUI.OnOpenPosition.AddListener((position)=>currentPosition=position);
            yield return new WaitUntil(()=>!currentPosition);
        }
    }
}