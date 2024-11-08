namespace Assets.Scripts
{
    using System.Collections;
    using UnityEngine;

    public class ReverseMask : MonoBehaviour
    {
        private RectTransform _parentCanvas;
        [SerializeField]
        private float _padding;
        private RectTransform _targetRectTransform;
        public RectTransform TargetRectTransform
        {
            get=>_targetRectTransform;
            set
            {
                _targetRectTransform=value;

                _topPanel.gameObject.SetActive(true);
                _leftPanel.gameObject.SetActive(true);
                _rightPanel.gameObject.SetActive(true);
                _bottomPanel.gameObject.SetActive(true);

                // 타겟 오브젝트의 월드 좌표 가져오기
                Vector3 worldPosition = value.position;

                // 카메라의 WorldToScreenPoint를 사용하여 스크린 좌표로 변환
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition)*(_parentCanvas.sizeDelta.x/Screen.width);

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
            _parentCanvas=transform.root.GetComponent<RectTransform>();
            _topPanel.gameObject.SetActive(false);
            _leftPanel.gameObject.SetActive(false);
            _rightPanel.gameObject.SetActive(false);
            _bottomPanel.gameObject.SetActive(false);
        }
        private IEnumerator Initialize()
        {
            yield return null;
            _parentCanvas=transform.parent.GetComponent<RectTransform>();
        }
    }
}