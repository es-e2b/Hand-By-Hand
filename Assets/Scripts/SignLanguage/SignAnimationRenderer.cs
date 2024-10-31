namespace Assets.Scripts.SignLanguage
{
    using System.Collections;
    using UnityEngine;

    public class SignAnimationRenderer : MonoBehaviour
    {
        #region Field
        public static SignAnimationRenderer Instance { get; private set; }
        
        [field:SerializeField]
        public GameObject Hand { get; private set; }
        #endregion

        #region Method
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            // renderingQueue=new Queue<(GameObject, Vocabulary)>();
        }
        public IEnumerator EnqueueVocabulary(GameObject speaker, Vocabulary vocabulary)
        {
            if(speaker == null)
            {
                Debug.LogError("Speaker object does not exist");
                yield break;
            }
            if(vocabulary == null)
            {
                yield break;
            }
            if(!speaker.TryGetComponent<IndividualSignAnimationRenderer>(out var individualSignAnimationRenderer))
            {
                individualSignAnimationRenderer=speaker.AddComponent<IndividualSignAnimationRenderer>();
            }
            yield return individualSignAnimationRenderer.EnqueueVocabulary(vocabulary);
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