namespace Assets.Scripts.SignLanguage
{
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
        public void EnqueueVocabulary(GameObject speaker, Vocabulary vocabulary)
        {
            if(speaker == null)
            {
                Debug.LogError("Speaker object does not exist");
                return;
            }
            if(!speaker.TryGetComponent<IndividualSignAnimationRenderer>(out var individualSignAnimationRenderer))
            {
                individualSignAnimationRenderer=speaker.AddComponent<IndividualSignAnimationRenderer>();
            }
            individualSignAnimationRenderer.EnqueueVocabulary(vocabulary);
        }
        public void StopAndEnqueueVocabulary(GameObject speaker, Vocabulary vocabulary)
        {
            if(!speaker.TryGetComponent<IndividualSignAnimationRenderer>(out var individualSignAnimationRenderer))
            {
                individualSignAnimationRenderer=speaker.AddComponent<IndividualSignAnimationRenderer>();
            }
            individualSignAnimationRenderer.StopAndEnqueueVocabulary(vocabulary);
        }
        #endregion
    }
}