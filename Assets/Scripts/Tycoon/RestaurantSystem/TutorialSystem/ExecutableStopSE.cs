namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using HandByHand.SoundSystem;
    using UnityEngine;

    [Serializable]
    public class ExecutableStopSE : ExecutableElement
    {
        [SerializeField]
        private SoundName soundName = SoundName.Select;
        public override IEnumerator Finalize()
        {
            SoundManager.Instance.StopSE();
            return base.Finalize();
        }
    }
}