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

        private List<TMP_Text> buttonText;
        private List<GameObject> viewportObject;
        private ScrollRect scrollViewScrollRectComponent;

        public TMP_Text WordToMake;
        private GameObject UIObjectInSignLanguageCanvas;
        private Vector3 originalUIObjectPosition;
        private float screenHeight = Screen.height;
        #endregion

        #region INIT
        private void Awake()
        {
            buttonText = ButtonListUIComponent.buttonText;
            viewportObject = ViewportListUIComponent.UIObject;
            UIObjectInSignLanguageCanvas = SignLanguageCanvas.transform.Find("UIObject").gameObject;
            originalUIObjectPosition = UIObjectInSignLanguageCanvas.transform.localPosition;
            scrollViewScrollRectComponent = ViewportListUIComponent.gameObject.transform.parent.GetComponent<ScrollRect>();
        }

        private void Start()
        {
            for (int i = 1; i < viewportObject.Count; i++)
            {
                viewportObject[i].transform.SetParent(InvisibleViewportObject.transform);
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

                StartCoroutine(UICanvasVerticalSlideCoroutine(targetPosition));
            }
        }

        public void InActiveUIObject()
        {
            if (SignLanguageCanvas.activeSelf)
            {
                Vector3 targetPosition = originalUIObjectPosition;

                StartCoroutine(UICanvasVerticalSlideCoroutine(targetPosition));
            }
        }
        #endregion


        public void ChangeUIObject()
        {
            //함수를 부른 버튼 오브젝트의 이름 받기
            string eventButtonName = EventSystem.current.currentSelectedGameObject.name;

            for (int i = 0; i < viewportObject.Count; i++)
            {
                //scrollViewObject[i].SetActive(false);
                viewportObject[i].transform.SetParent(InvisibleViewportObject.transform);
            }

            switch (eventButtonName)
            {
                case "HandCountButton":
                    ShowObject(0);
                    break;

                case "SymbolAndDirectionButton":
                    ShowObject(1);
                    break;

                case "HandPositionButton":
                    ShowObject(2);
                    break;

                case "ParticularButton":
                    ShowObject(3);
                    break;
            }

            void ShowObject(int index)
            {
                viewportObject[index].transform.SetParent(ViewportListUIComponent.gameObject.transform);
                scrollViewScrollRectComponent.content = viewportObject[index].GetComponent<RectTransform>();
            }
        }

        #region COROUTINE
        /// <summary>
        /// 현재 위치에서 targetPosition으로 부드럽게 이동
        /// </summary>
        /// <param name="targetPosition">목표 지점</param>
        /// <returns></returns>
        IEnumerator UICanvasVerticalSlideCoroutine(Vector3 targetPosition)
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
            yield return StartCoroutine(AnnounceAnimationDone());
        }

        IEnumerator AnnounceAnimationDone()
        {
            yield return null;
        }
        #endregion
    }
}
