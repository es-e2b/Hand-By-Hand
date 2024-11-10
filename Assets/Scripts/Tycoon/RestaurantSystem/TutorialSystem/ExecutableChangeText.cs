namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using TMPro;
    using UnityEngine;

    [Serializable]
    public class ExecutableChangeText : ExecutableElement
    {
        [SerializeField]
        private TMP_Text _targetTextObject;
        [TextArea]
        [SerializeField]
        private string _changeText;
        public override IEnumerator Finalize()
        {
            _targetTextObject.text=_changeText;
            yield return base.Finalize();
        }
    }
}