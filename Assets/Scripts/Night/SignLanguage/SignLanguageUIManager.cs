using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class SignLanguageUIManager : MonoBehaviour
    {
        #region VAIRABLE
        #region UI_COMPONENTS
        [Header("UI Components")]
        public GameObject SignLanguageCanvas;
        public TMP_Text WordToMake;
        public Button CompareEventButton;

        [Header("UIListComponent")]
        public ButtonListUIComponent ButtonListUIComponent;
        public ViewportListUIComponent ViewportListUIComponent;

        private List<TMP_Text> buttonTextList;
        private List<GameObject> buttonGameObjectList;
        private List<GameObject> viewportObjectList;
        private bool[] buttonHadPushed = new bool[3] {false, false , false }; 
        private ScrollRect scrollViewScrollRectComponent;

        private GameObject UIObjectInSignLanguageCanvas;
        private Vector3 originalUIObjectPosition;
        private float screenHeight = Screen.height;

        Coroutine horizontalSlideCoroutine;
        public bool isVerticalAnimationDone { get; private set; }
        #endregion
        #endregion

        #region INIT
        private void Awake()
        {
            buttonTextList = ButtonListUIComponent.buttonText;
            buttonGameObjectList = ButtonListUIComponent.buttonGameObject;
            viewportObjectList = ViewportListUIComponent.UIObject;
            UIObjectInSignLanguageCanvas = SignLanguageCanvas.transform.Find("UIObject").gameObject;
            originalUIObjectPosition = UIObjectInSignLanguageCanvas.transform.localPosition;
            scrollViewScrollRectComponent = ViewportListUIComponent.gameObject.transform.parent.GetComponent<ScrollRect>();

            isVerticalAnimationDone = true;
        }

        private void Start()
        {
            //InitViewportObject();
        }

        /// <summary>
        /// UI 재정렬 함수
        /// </summary>
        private void InitViewportObject()
        {
            Vector2 initVector = new Vector2(0, 0);
            StartCoroutine(ViewportHorizontalSlideCoroutine(initVector));
        }

        /// <summary>
        /// 버튼 색깔 초기화 함수
        /// </summary>
        private void InitButtonColor()
        {
            for (int i = 0; i < buttonGameObjectList.Count; i++)
            {
                //버튼 색깔 초기화
                Image image = buttonGameObjectList[i].GetComponent<Image>();

                //R255 G255 B255 A255
                image.color = new Color(1, 1, 1, 1);
            }
        }

        /// <summary>
        /// HadPushed배열 값을 false로 초기화
        /// </summary>
        private void InitHadPushedArray()
        {
            for(int i = 0; i < buttonHadPushed.Length; i++)
                buttonHadPushed[i] = false;
        }
        #endregion

        #region CANVASACTIVATEFUNCTION
        public void ActiveUIObject(string SignLanguageMean)
        {
            if (SignLanguageCanvas.activeSelf)
            {
                Vector3 targetPosition = originalUIObjectPosition + new Vector3(0, screenHeight, 0);
                WordToMake.SetText("Make The Word : " + SignLanguageMean);

                StartCoroutine(UICanvasVerticalSlideCoroutine(targetPosition, false));
            }
        }

        public void InActiveUIObject()
        {
            if (SignLanguageCanvas.activeSelf)
            {
                Vector3 targetPosition = originalUIObjectPosition;

                StartCoroutine(UICanvasVerticalSlideCoroutine(targetPosition, true));
            }
        }
        #endregion

        #region BUTTONEVENTFUNCTION
        public void ChangeUIObject()
        {
            //함수를 부른 버튼 오브젝트의 hierarchy상 인덱스 받기
            int eventButtonSiblingIndex = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();

            Vector2 targetPosition = viewportObjectList[eventButtonSiblingIndex].GetComponent<RectTransform>().anchoredPosition;


            if (horizontalSlideCoroutine != null)
            {
                StopCoroutine(horizontalSlideCoroutine);
            }
            horizontalSlideCoroutine = StartCoroutine(ViewportHorizontalSlideCoroutine(targetPosition));
        }

        public void ChangeToNextUI()
        {
            //함수를 부른 버튼 오브젝트의 hierarchy상 인덱스 받기
            int eventButtonSiblingIndex = EventSystem.current.currentSelectedGameObject.transform.parent.GetSiblingIndex();

            if (eventButtonSiblingIndex != 3 && !buttonHadPushed[eventButtonSiblingIndex])
            {
                Vector2 targetPosition = viewportObjectList[eventButtonSiblingIndex + 1].GetComponent<RectTransform>().anchoredPosition;

                if (horizontalSlideCoroutine != null)
                {
                    StopCoroutine(horizontalSlideCoroutine);
                }
                horizontalSlideCoroutine = StartCoroutine(ViewportHorizontalSlideCoroutine(targetPosition));

                buttonHadPushed[eventButtonSiblingIndex] = true;
            }
        }

        /// <summary>
        /// 선택지 선택시 버튼 색깔을 초록색깔로 변경하는 이벤트용 함수
        /// </summary>
        public void ChangeColorOfButtonEvent()
        {
            string eventButtonName = EventSystem.current.currentSelectedGameObject.transform.parent.name;
            int eventButtonIndex = 0;

            switch (eventButtonName)
            {
                case "HandCountContent":
                    eventButtonIndex = 0;
                    break;
                case "SymbolAndDirectionContent":
                    eventButtonIndex = 1;
                    break;
                case "HandPositionContent":
                    eventButtonIndex = 2;
                    break;
                case "ParticularContent":
                    eventButtonIndex = 3;
                    break;
            }

            Image image = buttonGameObjectList[eventButtonIndex].GetComponent<Image>();

            //R0 G255 B0 A255
            image.color = new Color(0, 1, 0, 1);
        }
        #endregion

        public void CheckWrongAnswerButton(int hierarchyIndex)
        {
            Image image = buttonGameObjectList[hierarchyIndex].GetComponent<Image>();

            //R255 G0 B0 A255
            image.color = new Color(1, 0, 0, 1);
        }

        #region COROUTINE
        /// <summary>
        /// 현재 위치에서 targetPosition으로 부드럽게 "수직" 이동
        /// </summary>
        /// <param name="targetPosition">목표 지점</param>
        /// <param name="isInitObject">오브젝트 위치를 재정렬 할것인지에 대한 boolean</param>
        /// <returns></returns>
        IEnumerator UICanvasVerticalSlideCoroutine(Vector3 targetPosition, bool isInitObject)
        {
            //변수 선언
            #region VARIABLEFIELD
            float UIObjectX = UIObjectInSignLanguageCanvas.transform.localPosition.x;
            float UIObjectZ = UIObjectInSignLanguageCanvas.transform.localPosition.z;

            float velocityY = 0f;
            float smoothTime = 0.3f;

            float offset = 0.1f;
            #endregion

            isVerticalAnimationDone = false;
            //패널을 위로 부드럽게 올린다
            //목표 지점이 패널의 현재 위치보다 위에 있을 경우
            if (targetPosition.y - offset > UIObjectInSignLanguageCanvas.transform.localPosition.y)
            {
                while (targetPosition.y - offset > UIObjectInSignLanguageCanvas.transform.localPosition.y)
                {
                    float positionY = Mathf.SmoothDamp(UIObjectInSignLanguageCanvas.transform.localPosition.y, targetPosition.y, ref velocityY, smoothTime);
                    UIObjectInSignLanguageCanvas.transform.localPosition = new Vector3(UIObjectX, positionY, UIObjectZ);

                    yield return null;
                }
            }
            //목표 지점이 아래 있을 경우
            else
            {
                while (targetPosition.y + offset < UIObjectInSignLanguageCanvas.transform.localPosition.y)
                {
                    float positionY = Mathf.SmoothDamp(UIObjectInSignLanguageCanvas.transform.localPosition.y, targetPosition.y, ref velocityY, smoothTime);
                    UIObjectInSignLanguageCanvas.transform.localPosition = new Vector3(UIObjectX, positionY, UIObjectZ);

                    yield return null;
                }
            }

            UIObjectInSignLanguageCanvas.transform.localPosition = targetPosition;

            //애니메이션 완료를 알림
            yield return StartCoroutine(AnnounceAnimationDone(isInitObject));
        }

        /// <summary>
        /// Viewport를 현재 위치에서 targetPosition으로 좌우 이동
        /// </summary>
        /// <returns></returns>
        IEnumerator ViewportHorizontalSlideCoroutine(Vector2 viewportObjectRectPosition)
        {
            if (!isVerticalAnimationDone)
            {
                yield return new WaitUntil(() => isVerticalAnimationDone == true);
            }

            //변수 선언
            #region VARIABLEFIELD
            //Viewport의 현재 위치를 받아오기 위한 변수
            RectTransform viewportRectTransformComponent = ViewportListUIComponent.GetComponent<RectTransform>();
            Vector2 viewportPosition = new Vector2(0, 0);

            float UIObjectY = viewportRectTransformComponent.anchoredPosition.y;

            //목표 위치를 저장할 변수
            Vector2 targetPosition;

            float velocityX = 0f;
            float smoothTime = 0.15f;

            float offset = 0.5f;
            #endregion

            //viewport오브젝트를 왼쪽으로 밂
            if (viewportObjectRectPosition.x + viewportRectTransformComponent.anchoredPosition.x > viewportPosition.x)
            {
                //반대로 움직여야 하므로 기존 위치에서 값을 뺀다
                targetPosition = viewportPosition - viewportObjectRectPosition;

                while (targetPosition.x + offset < viewportRectTransformComponent.anchoredPosition.x)
                {
                    float positionX = Mathf.SmoothDamp(viewportRectTransformComponent.anchoredPosition.x, targetPosition.x, ref velocityX, smoothTime);
                    viewportRectTransformComponent.anchoredPosition = new Vector2(positionX, UIObjectY);

                    yield return null;
                }
            }
            else
            {
                targetPosition = viewportPosition - viewportObjectRectPosition;

                while (targetPosition.x - offset > viewportRectTransformComponent.anchoredPosition.x)
                {
                    float positionX = Mathf.SmoothDamp(viewportRectTransformComponent.anchoredPosition.x, targetPosition.x, ref velocityX, smoothTime);
                    viewportRectTransformComponent.anchoredPosition = new Vector2(positionX, UIObjectY);

                    yield return null;
                }
            }

            viewportRectTransformComponent.anchoredPosition = targetPosition;

            yield return null;
        }

        IEnumerator AnnounceAnimationDone(bool isInitObject)
        {
            if (isInitObject)
            {
                InitViewportObject();
                InitButtonColor();
                InitHadPushedArray();
            }

            isVerticalAnimationDone = true;
            yield return null;
        }
        #endregion
    }
}
