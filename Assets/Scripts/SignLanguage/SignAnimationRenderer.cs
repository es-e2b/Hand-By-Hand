namespace Assets.Scripts.SignLanguage
{
    using System.Collections;
    using UnityEngine;

    public class SignAnimationRenderer : MonoBehaviour
    {
        #region Field
        private static SignAnimationRenderer _instance;
        public static SignAnimationRenderer Instance
        {
            get
            {
                if(_instance==null)
                {
                    SignAnimationRenderer obj = new GameObject("SignAnimationRenderer", typeof(SignAnimationRenderer)).GetComponent<SignAnimationRenderer>();
                    DontDestroyOnLoad(obj);
                    _instance=obj;
                }
                return _instance;
            }
        }
        #endregion

        #region Method
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance=this;
        }
        public IEnumerator StopAndEnqueueVocabulary(GameObject speaker, Vocabulary vocabulary)
        {
            if(!speaker.TryGetComponent<IndividualSignAnimationRenderer>(out var individualSignAnimationRenderer))
            {
                individualSignAnimationRenderer=speaker.AddComponent<IndividualSignAnimationRenderer>();
            }
            yield return individualSignAnimationRenderer.StopAndEnqueueVocabulary(vocabulary);
        }
        #endregion
    }
}