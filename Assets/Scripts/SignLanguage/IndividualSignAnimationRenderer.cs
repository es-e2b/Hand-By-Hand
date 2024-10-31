namespace Assets.Scripts.SignLanguage
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class IndividualSignAnimationRenderer : MonoBehaviour
    {
        #region Field
        private Queue<Vocabulary> renderingQueue;
        private GameObject rightHandObject;
        private GameObject leftHandObject;
        private Coroutine leftHandRenderingCoroutine;
        private Coroutine rightHandRenderingCoroutine;
        private bool isRendering;
        #endregion

        #region Method
        private void Awake()
        {
            renderingQueue=new();
        }
        private void Start()
        {
            leftHandObject = Instantiate(SignAnimationRenderer.Instance.Hand, transform);
            rightHandObject = Instantiate(SignAnimationRenderer.Instance.Hand, transform);

            Rect speakerRect = gameObject.GetComponent<RectTransform>().rect;

            float handSize = Mathf.Min(speakerRect.width, speakerRect.height)*0.3f;

            leftHandObject.GetComponent<RectTransform>().sizeDelta = new Vector2(handSize, handSize);
            rightHandObject.GetComponent<RectTransform>().sizeDelta = new Vector2(handSize, handSize);
        }
        // private bool IsVocabularyRenderingComplete()
        // {
        //     if(leftHandRenderingCoroutine==null && rightHandRenderingCoroutine==null)
        //     {
        //         return true;
        //     }
        //     return false;
        // }
        public IEnumerator EnqueueVocabulary(Vocabulary vocabulary)
        {
            renderingQueue.Enqueue(vocabulary);
            if(!isRendering)
            {
                yield return StartAnimation();
            }
        }
        public IEnumerator StopAndEnqueueVocabulary(Vocabulary vocabulary)
        {
            yield return StopAnimation();
            yield return EnqueueVocabulary(vocabulary);
        }
        private IEnumerator StartAnimation()
        {
            isRendering=true;
            while (renderingQueue.Count > 0)
            {
                Vocabulary now = renderingQueue.Dequeue();
                RectTransform speakerRectTransform=GetComponent<RectTransform>();
                rightHandObject.GetComponent<RectTransform>().localScale=speakerRectTransform.localScale;
                leftHandObject.GetComponent<RectTransform>().localScale=speakerRectTransform.localScale;

                rightHandRenderingCoroutine = StartCoroutine(AnimateHandshape(now.RightHandshapes, rightHandObject));
                leftHandRenderingCoroutine = StartCoroutine(AnimateHandshape(now.LeftHandshapes, leftHandObject));

                // yield return null;
                yield return rightHandRenderingCoroutine;
                yield return leftHandRenderingCoroutine;
                print("Both Hand is Stop");
            }
            yield return StopAnimation();
        }
        private IEnumerator StopAnimation()
        {
            renderingQueue.Clear();
            yield return StopLeftHandAnimation();
            yield return StopRightHandAnimation();
            isRendering=false;
        }
        private IEnumerator StopLeftHandAnimation()
        {
            if(leftHandRenderingCoroutine == null) yield break;
            print("Called Stop Left Hand Animation Method");
            leftHandObject.transform.localScale=Vector2.zero;
            // StopCoroutine(leftHandRenderingCoroutine);
            leftHandRenderingCoroutine = null;
        }
        private IEnumerator StopRightHandAnimation()
        {
            if(rightHandRenderingCoroutine == null) yield break;
            print("Called Stop Right Hand Animation Method");
            rightHandObject.transform.localScale=Vector2.zero;
            // StopCoroutine(rightHandRenderingCoroutine);
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