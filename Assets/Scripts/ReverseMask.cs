namespace Assets.Scripts
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class ReverseMask : MonoBehaviour
    {
        private RectTransform _parentCanvas;
        [SerializeField]
        private RectTransform _targetRectTransform;
        [SerializeField]
        private GameObject MaskingObject;
        public RectTransform TargetRectTransform
        {
            get=>_targetRectTransform;
            set
            {
                _targetRectTransform = value;

                // 타겟 오브젝트의 월드 좌표 가져오기
                Vector3 worldPosition = _targetRectTransform.position;

                // 카메라의 WorldToScreenPoint를 사용하여 스크린 좌표로 변환
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition)*(_parentCanvas.sizeDelta.x/Screen.width);

                MaskingObject.GetComponent<RectTransform>().anchoredPosition=screenPosition;
                MaskingObject.GetComponent<RectTransform>().sizeDelta=_targetRectTransform.sizeDelta;
                MaskingObject.transform.localScale*=1.25f;
                MaskingObject.GetComponent<Image>().sprite=_targetRectTransform.GetComponent<Image>().sprite;
            }
        }
        private void Start()
        {
            StartCoroutine(A());
        }
        private IEnumerator A()
        {
            yield return new WaitForSeconds(0.5f);
            _parentCanvas=transform.parent.GetComponent<RectTransform>();
            TargetRectTransform=_targetRectTransform;    
        }
    }
}