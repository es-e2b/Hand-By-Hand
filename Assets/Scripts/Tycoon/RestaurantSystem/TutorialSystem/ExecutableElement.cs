namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;

    [Serializable]
    public class ExecutableElement : MonoBehaviour, IExcutable
    {
        [SerializeField]
        private float _watingStartTime;
        public virtual IEnumerator Initialize()
        {
            yield return new WaitForSeconds(_watingStartTime);
            Debug.Log("Called Initialize");
            yield return Begin();
        }
        public virtual IEnumerator Complete()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerator Execute()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerator Next()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerator Pause()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerator Skip()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerator Begin()
        {
            throw new NotImplementedException();
        }
    }
}