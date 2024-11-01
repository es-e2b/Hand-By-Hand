namespace Assets.Scripts.SignLanguage
{
    using UnityEngine;

    public class SignAnimationRendererTest : MonoBehaviour
    {
        [SerializeField]
        private GameObject speaker;
        [SerializeField]
        private Vocabulary vocabulary;
        private void Start()
        {
            StartCoroutine(SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(speaker, vocabulary));
        }
        private void Update()
        {
            if(Input.GetMouseButton(0))
            {
                StartCoroutine(SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(speaker, vocabulary));
            }
        }
    }
}