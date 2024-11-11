namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using HandByHand.SoundSystem;
    using UnityEngine;

    [Serializable]
    public class ExecutableStopBGM : ExecutableElement
    {
        public override IEnumerator Finalize()
        {
            SoundManager.Instance.StopBGM();
            return base.Finalize();
        }
    }
}