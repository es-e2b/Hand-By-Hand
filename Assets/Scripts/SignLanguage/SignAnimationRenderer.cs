namespace Assets.Scripts.SignLanguage
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class SignAnimationRenderer : MonoBehaviour
    {
        #region Field
        public static SignAnimationRenderer Instance { get; private set; }
        private Queue<(GameObject, Vocabulary)> renderingQueue;
        private GameObject rightHandObject;
        private GameObject leftHandObject;
        private Coroutine leftHandRenderingCoroutine;
        private Coroutine rightHandRenderingCoroutine;
        private bool isRedering;

        // public Vocabulary vocabulary1;
        // public GameObject speaker;
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
            renderingQueue=new Queue<(GameObject, Vocabulary)>();

            leftHandObject = new GameObject("Left Hand", typeof(Image));
            rightHandObject = new GameObject("Right Hand", typeof(Image));

            leftHandObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300,300);
            rightHandObject.GetComponent<RectTransform>().sizeDelta = new Vector2(300,300);

            // leftHandObject.transform.SetParent(speaker.transform, false);
            // rightHandObject.transform.SetParent(speaker.transform, false);

            leftHandObject.transform.localScale=Vector2.zero;
            rightHandObject.transform.localScale=Vector2.zero;

            // EnqueueVocabulary(speaker, vocabulary1);
        }
        private bool IsVocabularyRenderingComplete()
        {
            if(leftHandRenderingCoroutine==null && rightHandRenderingCoroutine==null)
            {
                return true;
            }
            return false;
        }
        public void EnqueueVocabulary(GameObject speaker, Vocabulary vocabulary)
        {
            renderingQueue.Enqueue((speaker, vocabulary));
            if(!isRedering)
            {
                StartCoroutine(StartAnimation());
            }
        }
        public IEnumerator StopAndEnqueueVocabulary(GameObject speaker, Vocabulary vocabulary)
        {
            yield return StopAnimation();
            yield return new WaitUntil(() => IsVocabularyRenderingComplete());
            EnqueueVocabulary(speaker, vocabulary);
        }
        private IEnumerator StartAnimation()
        {
            isRedering=true;
            while (renderingQueue.Count > 0)
            {
                (GameObject speaker, Vocabulary vocabulary) now = renderingQueue.Dequeue();
                RectTransform speakerRectTransform=now.speaker.GetComponent<RectTransform>();
                
                rightHandObject.transform.SetParent(now.speaker.transform, false);
                rightHandObject.GetComponent<RectTransform>().localScale=speakerRectTransform.localScale;

                leftHandObject.transform.SetParent(now.speaker.transform, false);
                leftHandObject.GetComponent<RectTransform>().localScale=speakerRectTransform.localScale;

                rightHandRenderingCoroutine = StartCoroutine(AnimateHandshape(now.vocabulary.RightHandshapes, rightHandObject));
                leftHandRenderingCoroutine = StartCoroutine(AnimateHandshape(now.vocabulary.LeftHandshapes, leftHandObject));

                yield return new WaitUntil(() => IsVocabularyRenderingComplete());
            }
            StartCoroutine(StopAnimation());
        }
        private IEnumerator StopAnimation()
        {
            renderingQueue.Clear();
            yield return StopLeftHandAnimation();
            yield return StopRightHandAnimation();
            isRedering=false;
        }
        private IEnumerator StopLeftHandAnimation()
        {
            if(leftHandRenderingCoroutine == null) yield break;
            print("Called Stop Left Hand Animation Method");
            leftHandObject.transform.localScale=Vector2.zero;
            StopCoroutine(leftHandRenderingCoroutine);
            leftHandRenderingCoroutine = null;
        }
        private IEnumerator StopRightHandAnimation()
        {
            if(rightHandRenderingCoroutine == null) yield break;
            print("Called Stop Right Hand Animation Method");
            rightHandObject.transform.localScale=Vector2.zero;
            StopCoroutine(rightHandRenderingCoroutine);
            rightHandRenderingCoroutine = null;
        }
        private IEnumerator AnimateHandshape(List<Handshape> handshapes, GameObject handObject)
        {
            foreach (Handshape handshape in handshapes)
            {
                handObject.GetComponent<Image>().sprite = handshape.HandshapeImage;
                yield return MoveHandshape(handshape, handObject);
            }
            if (handObject == leftHandObject)
            {
                yield return StopLeftHandAnimation();
            }
            else if (handObject == rightHandObject)
            {
                yield return StopRightHandAnimation();
            }
        }
        private IEnumerator MoveHandshape(Handshape handshape, GameObject handObject)
        {
            RectTransform rectTransform=handObject.GetComponent<RectTransform>();

            float elapsedTime = 0;
            Vector2 currentPosition;
            Vector3 currentRotation;

            while (elapsedTime < handshape.Duration)
            {
                currentPosition = Vector2.Lerp(handshape.StartPosition, handshape.EndPosition, elapsedTime / handshape.Duration);
                currentPosition += new Vector2(0.5f, 0.5f);
                rectTransform.anchorMin = rectTransform.anchorMax = currentPosition;

                currentRotation = Vector3.Lerp(handshape.StartRotation, handshape.EndRotation, elapsedTime/handshape.Duration);
                rectTransform.eulerAngles = currentRotation;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            rectTransform.anchorMin = rectTransform.anchorMax = handshape.EndPosition;
            rectTransform.eulerAngles = handshape.EndRotation;
        }
        #endregion
    }
}