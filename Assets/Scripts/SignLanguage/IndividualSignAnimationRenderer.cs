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
        #endregion

        #region Method
        private void Awake()
        {
            renderingQueue=new();
        }
        public IEnumerator StopAndEnqueueVocabulary(Vocabulary vocabulary)
        {
            if(leftHandRenderingCoroutine!=null)
            {
                StopCoroutine(leftHandRenderingCoroutine);
            }
            if(rightHandRenderingCoroutine!=null)
            {
                StopCoroutine(rightHandRenderingCoroutine);
            }
            renderingQueue.Clear();
            renderingQueue.Enqueue(vocabulary);
            yield return StartAnimation();
        }
        private IEnumerator StartAnimation()
        {
            while (renderingQueue.Count > 0)
            {
                Vocabulary now = renderingQueue.Dequeue();

                if(leftHandObject==null && now.LeftHandshapes.Count>0)
                {
                    leftHandObject=InstantiateHandObject();
                }
                if(rightHandObject==null && now.RightHandshapes.Count>0)
                {
                    rightHandObject=InstantiateHandObject();
                }
                leftHandRenderingCoroutine = StartCoroutine(AnimateHandshape(now.LeftHandshapes, leftHandObject));
                rightHandRenderingCoroutine = StartCoroutine(AnimateHandshape(now.RightHandshapes, rightHandObject));

                yield return rightHandRenderingCoroutine;
                yield return leftHandRenderingCoroutine;
            }
        }
        private IEnumerator AnimateHandshape(List<Handshape> handshapes, GameObject handObject)
        {
            foreach (Handshape handshape in handshapes)
            {
                if(handshape.HandshapeImage==null)
                {
                    handObject.SetActive(false);
                }
                else
                {
                    handObject.SetActive(true);
                    handObject.GetComponent<Image>().sprite = handshape.HandshapeImage;
                }
                yield return MoveHandshape(handshape, handObject);
            }
            handObject.SetActive(false);
        }
        private IEnumerator MoveHandshape(Handshape handshape, GameObject handObject)
        {
            RectTransform rectTransform=handObject.GetComponent<RectTransform>();

            float elapsedTime = 0;
            Vector2 currentPosition;
            Vector3 currentRotation;
            float currentScale;
            
            while (elapsedTime < handshape.Duration)
            {

                currentPosition = Vector2.Lerp(handshape.StartPosition, handshape.EndPosition, elapsedTime / handshape.Duration);
                currentPosition += new Vector2(0.5f, 0.5f);
                rectTransform.anchorMin = rectTransform.anchorMax = currentPosition;

                currentRotation = Vector3.Lerp(handshape.StartRotation, handshape.EndRotation, elapsedTime/handshape.Duration);
                rectTransform.eulerAngles = currentRotation;

                currentScale = (handshape.EndScale-handshape.StartScale)*elapsedTime/handshape.Duration+handshape.StartScale;
                rectTransform.localScale = new Vector3(currentScale,currentScale,currentScale);

                yield return null;
                elapsedTime += Time.deltaTime;
            }

            rectTransform.anchorMin = rectTransform.anchorMax = handshape.EndPosition;
            rectTransform.eulerAngles = handshape.EndRotation;
            rectTransform.localScale = new Vector3(handshape.EndScale, handshape.EndScale, handshape.EndScale);
        }
        private GameObject InstantiateHandObject()
        {
            GameObject handObject= Instantiate((GameObject)Resources.Load("Hand"), transform);
            Rect speakerRect = GetComponent<RectTransform>().rect;
            handObject.GetComponent<RectTransform>().sizeDelta = 0.3f * Mathf.Min(speakerRect.width, speakerRect.height) * Vector2.one;
            return handObject;
        }
        #endregion
    }
}