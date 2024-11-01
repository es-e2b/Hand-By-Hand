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
        #region UI_COMPONENTS
        [Header("UI Components")]
        public GameObject SignLanguageCanvas;

        public ButtonListUIComponent ButtonListUIComponent;
        public ViewportListUIComponent ViewportListUIComponent;
        public GameObject InvisibleViewportObject;

        private List<TMP_Text> buttonTextList;
        private List<GameObject> buttonGameObjectList;
        private List<GameObject> viewportObjectList;
        private ScrollRect scrollViewScrollRectComponent;

        public TMP_Text WordToMake;
        public Button CompareEventButton;
        private GameObject UIObjectInSignLanguageCanvas;
        private Vector3 originalUIObjectPosition;
        private float screenHeight = Screen.height;
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
        }

        private void Start()
        {
            InitViewportObject();
        }

        /// <summary>
        /// UI 재정렬 함수
        /// </summary>
        private void InitViewportObject()
        {
            //Viewport의 UI를 재정렬
            viewportObjectList[0].transform.SetParent(ViewportListUIComponent.transform);
            
            for (int i = 1; i < viewportObjectList.Count; i++)
            {
                viewportObjectList[i].transform.SetParent(InvisibleViewportObject.transform);
            }
        }

        /// <summary>
        /// 버튼 색깔 초기화 함수
        /// </summary>
        private void InitButtonColor()
        {
            for(int i = 0; i < buttonGameObjectList.Count; i++)
            {
                //버튼 색깔 초기화
                Image image = buttonGameObjectList[i].GetComponent<Image>();

                //R255 G255 B255 A255
                image.color = new Color(1, 1, 1, 1);
            }
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

        #region BUTTONFUNCTION
        public void ChangeUIObject()
        {
            //함수를 부른 버튼 오브젝트의 hierarchy상 인덱스 받기
            int eventButtonSiblingIndex = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();

            //모든 오브젝트를 안 보이게 처리
            for (int i = 0; i < viewportObjectList.Count; i++)
            {
                viewportObjectList[i].transform.SetParent(InvisibleViewportObject.transform);
            }

            //함수를 부른 오브젝트만 보이도록 처리
            viewportObjectList[eventButtonSiblingIndex].transform.SetParent(ViewportListUIComponent.gameObject.transform);
            //스크롤이 가능하도록 처리
            //scrollViewScrollRectComponent.content = viewportObjectList[eventButtonSiblingIndex].GetComponent<RectTransform>();
        }

        public void ChangeToNextUI()
        {
            //함수를 부른 버튼 오브젝트의 hierarchy상 인덱스 받기
            int eventButtonSiblingIndex = EventSystem.current.currentSelectedGameObject.transform.GetSiblingIndex();


        }

        public void ChangeColorOfButton()
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

        #region COROUTINE
        /// <summary>
        /// 현재 위치에서 targetPosition으로 부드럽게 이동
        /// </summary>
        /// <param name="targetPosition">목표 지점</param>
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

        IEnumerator AnnounceAnimationDone(bool isInitObject)
        {
            if (isInitObject)
            {
                InitViewportObject();
                InitButtonColor();
            }
            yield return null;
        }
        #endregion
    }
}
