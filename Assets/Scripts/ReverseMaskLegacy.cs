namespace Assets.Scripts
{
    using System.Collections;
    using UnityEngine;

    public class ReverseMaskLegacy : MonoBehaviour
    {
        private RectTransform _parentCanvas;
        [SerializeField]
        private RectTransform _targetRectTransform;
        [SerializeField]
        private float _padding;
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

                Debug.Log(screenPosition);
                Debug.Log(Screen.width+", "+Screen.height);

                // 상단 패널
                _topPanel.sizeDelta = new Vector2(_parentCanvas.sizeDelta.x, _parentCanvas.sizeDelta.y - screenPosition.y - value.sizeDelta.y / 2);
                _topPanel.anchoredPosition = new Vector2(0, screenPosition.y + value.sizeDelta.y / 2 + _padding);

                // 하단 패널
                _bottomPanel.sizeDelta = new Vector2(_parentCanvas.sizeDelta.x, screenPosition.y - value.sizeDelta.y / 2 - _padding);
                _bottomPanel.anchoredPosition = new Vector2(0, 0);

                // 좌측 패널
                _leftPanel.sizeDelta = new Vector2(screenPosition.x - value.sizeDelta.x / 2 - _padding, value.sizeDelta.y + _padding*2);
                _leftPanel.anchoredPosition = new Vector2(0, screenPosition.y - value.sizeDelta.y / 2 - _padding);

                // 우측 패널
                _rightPanel.sizeDelta = new Vector2(_parentCanvas.sizeDelta.x - screenPosition.x - value.sizeDelta.x / 2, value.sizeDelta.y + _padding*2);
                _rightPanel.anchoredPosition = new Vector2(screenPosition.x + value.sizeDelta.x / 2 + _padding, screenPosition.y - value.sizeDelta.y / 2 - _padding);
            }
        }
        [SerializeField]
        private RectTransform _topPanel;
        [SerializeField]
        private RectTransform _leftPanel;
        [SerializeField]
        private RectTransform _rightPanel;
        [SerializeField]
        private RectTransform _bottomPanel;
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