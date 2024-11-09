namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    [Serializable]
    public class ExecutableExecute : ExecutableElement
    {
        [SerializeField]
        private ExecutableElement[] _executableElements;
        [SerializeField]
        private float _executionDuration;
        private ElementExecutor _elementExecutor;
        public override IEnumerator Initialize()
        {
            _elementExecutor=FindAnyObjectByType<ElementExecutor>();
            yield return base.Initialize();
        }
        public override IEnumerator Finalize()
        {
            Array.ForEach(_executableElements, element=>_elementExecutor.StartExecutableElement(element));
            float elapsedTime = 0f;

            while (elapsedTime < _executionDuration && !_isSkipping)
            {
                float t = elapsedTime / _executionDuration;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if(_isSkipping)
            {
                Array.ForEach(_executableElements, element=>element.Skip());
            }
            yield return base.Finalize();
        }
    }
}