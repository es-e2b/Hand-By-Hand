namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutablePromptTutorial : ExecutablePromptLegacy
    {
        public override IEnumerator Finalize()
        {
            GameManager.Instance.hasCompletedTutorial=true;
            yield return base.Finalize();
        }
    }
}