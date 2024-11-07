namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutablePromptTutorial : ExecutablePrompt
    {
        public override IEnumerator Pause()
        {
            GameManager.Instance.hasCompletedTutorial=true;
            yield return base.Pause();
        }
    }
}