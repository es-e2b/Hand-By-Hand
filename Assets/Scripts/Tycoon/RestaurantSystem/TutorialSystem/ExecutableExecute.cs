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
        public override void Skip()
        {
            print("Execute Class: Skip Method");
            Array.ForEach(_executableElements, element=>element.Skip());
            base.Skip();
        }
        public override IEnumerator Initialize()
        {
            _elementExecutor=FindAnyObjectByType<ElementExecutor>();
            yield return base.Initialize();
        }
        public override IEnumerator Begin()
        {
            Array.ForEach(_executableElements, element=>_elementExecutor.StartExecutableElement(element));
            yield return base.Begin();
        }
        public override IEnumerator Execute()
        {
            print("Called Execute Method");
            float elapsedTime = 0f;
            while (elapsedTime < _executionDuration && !_isSkipping)
            {
                float t = elapsedTime / _executionDuration;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if(_isSkipping)
            {
                print("Execute Class Skipped");
            }
        }
        public override IEnumerator Finalize()
        {
            print("Called Fininalize Method");
            Array.ForEach(_executableElements, element=>element.Skip());
            yield return base.Finalize();
        }
    }
}