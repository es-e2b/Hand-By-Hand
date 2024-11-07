namespace Assets.Scripts.Tycoon.RestaurantSystem.OrderSystem
{
    using System.Collections;
    using UnityEngine;

    public class HintMessageDisplayer : TimedDisable
    {
        private RectTransform _rectTransform;
        private Vector2 _currentPosition;
        private CanvasGroup _canvasGroup;
        [SerializeField] private float animationDuration = 0.5f;
        [SerializeField] private Vector2 startPositionOffset = new Vector2(0, -15); //하단 시작 위치 오프셋
        private Coroutine _ShowAnimationCoroutine;
        private Coroutine _HideAnimationCoroutine;
        protected override void Awake()
        {
            _timer=new UniversalTimer(_targetTime, ()=>{_HideAnimationCoroutine=StartCoroutine(HideMessageAnimation());}, StartCoroutine, StopCoroutine);
            _rectTransform=GetComponent<RectTransform>();
            _currentPosition=_rectTransform.anchoredPosition;
            if(!TryGetComponent<CanvasGroup>(out _canvasGroup))
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            _ShowAnimationCoroutine=StartCoroutine(ShowMessageAnimation());
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            StopCoroutine(_ShowAnimationCoroutine);
            if(_HideAnimationCoroutine==null)
            {
                StopCoroutine(_HideAnimationCoroutine);
            }
        }

        private IEnumerator ShowMessageAnimation()
        {
            _canvasGroup.alpha = 0f;
            _rectTransform.anchoredPosition = _currentPosition + startPositionOffset;

            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;

                // 투명도와 위치 보간
                _canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
                _rectTransform.anchoredPosition = Vector2.Lerp(_currentPosition + startPositionOffset, _currentPosition, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 애니메이션 종료 후 위치와 투명도 설정
            _canvasGroup.alpha = 1f;
            _rectTransform.anchoredPosition = _currentPosition;
        }
        private IEnumerator HideMessageAnimation()
        {
            _canvasGroup.alpha = 1f;
            _rectTransform.anchoredPosition = _currentPosition + startPositionOffset;

            float elapsedTime = 0f;

            while (elapsedTime < animationDuration)
            {
                float t = elapsedTime / animationDuration;

                // 투명도와 위치 보간
                _canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
                _rectTransform.anchoredPosition = Vector2.Lerp(_currentPosition, _currentPosition + startPositionOffset, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 애니메이션 종료 후 위치와 투명도 설정
            _canvasGroup.alpha = 0f;
            _rectTransform.anchoredPosition = _currentPosition;
            gameObject.SetActive(false);
        }
    }
}