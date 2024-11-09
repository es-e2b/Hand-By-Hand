namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    [Serializable]
    public class ExecutableAction : ExecutableElement
    {
        public UnityEvent _action;
        [SerializeField]
        private float _executionDuration;
        public override IEnumerator Finalize()
        {
            _action.Invoke();
            
            float elapsedTime = 0f;

            while (elapsedTime < _executionDuration && !_isSkipping)
            {
                float t = elapsedTime / _executionDuration;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            yield return base.Finalize();
        }
    }
}