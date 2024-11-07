using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Reflection;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class SignLanguageUIManager : MonoBehaviour
    {
        #region VAIRABLE

        #region UI_VARIABLE

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
        
        private GameObject UIObjectInSignLanguageCanvas;
        private Vector3 originalUIObjectPosition;
        private float screenHeight = Screen.height;
        private Vector3 originalCompareEventButtonPosition;
        
        #endregion


        #region BE_VARIABLE

        private bool[] buttonHadPushed = new bool[4] { false, false, false, false };
        private bool completeButtonHadPushed { get; set; } = false;
        public int presentPanelIndex { get; private set; } = 0;

        Coroutine horizontalSlideCoroutine;
        public bool IsVerticalAnimationDone { get; private set; }
        public bool SignLanguageUIActiveSelf { get; private set; } = false;

        [HideInInspector]
        public List<int> incorrectAnswerIndexList = new List<int>() { -1 };

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
            originalCompareEventButtonPosition = CompareEventButton.GetComponent<RectTransform>().anchoredPosition;

            IsVerticalAnimationDone = true;
        }

        private void Start()
        {
            if (UIObjectInSignLanguageCanvas.GetComponent<RectTransform>().anchoredPosition != new Vector2(0, -1 * screenHeight))
                UIObjectInSignLanguageCanvas.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -1 * screenHeight);
        }

        /// <summary>
        /// UI 재정렬 함수 - Non-using
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
            for (int i = 0; i < buttonHadPushed.Length; i++)
                buttonHadPushed[i] = false;
        }

        /// <summary>
        /// CompareEventButton의 위치를 원래대로 재조정
        /// </summary>
        private void InitCompareEventButtonPosition()
        {
            CompareEventButton.transform.localPosition = originalCompareEventButtonPosition;
        }

        private void InitViewportAndButtonOpacity()
        {
            for(int i = 0; i < viewportObjectList.Count; i++)
            {
                viewportObjectList[i].GetComponent<WhatIsSelectedComponent>().Init();
            }
            this.ButtonListUIComponent.GetComponent<WhatIsSelectedComponent>().AdjustOpacityOfIndex(0);
        }

        private void InitButtonInteractable()
        {
            SetButtonInteractable(true);
        }
        #endregion

        /// <summary>
        /// viewportObjectList[index]로 패널을 부드럽게 슬라이드 이동, 상단 탭의 투명도 변경까지
        /// </summary>
        /// <param name="index"></param>
        public void ChangeUITab(int index, float animationWaitingTime = 0)
        {
            Vector2 targetPosition = viewportObjectList[index].GetComponent<RectTransform>().anchoredPosition;

            if (horizontalSlideCoroutine != null)
            {
                StopCoroutine(horizontalSlideCoroutine);
            }
            horizontalSlideCoroutine = StartCoroutine(ViewportHorizontalSlideCoroutine(targetPosition, animationWaitingTime));

            //상단 버튼 오브젝트 투명도 변경
            this.ButtonListUIComponent.GetComponent<WhatIsSelectedComponent>().AdjustOpacityOfIndex(index);

            presentPanelIndex = index;
        }

        #region CANVASACTIVATEFUNCTION
        public void ActiveUIObject(string SignLanguageMean)
        {
            if (SignLanguageCanvas.activeSelf)
            {
                Vector3 targetPosition = new Vector3(0, 0, 0);
                WordToMake.SetText("수화로 표현해보자!\n\"" + SignLanguageMean + "\"");
                SignLanguageUIActiveSelf = true;

                StartCoroutine(UICanvasVerticalSlideCoroutine(targetPosition, false));
            }
        }

        public void InActiveUIObject()
        {
            if (SignLanguageCanvas.activeSelf)
            {
                Vector3 targetPosition = new Vector3(0, -1 * screenHeight, 0);
                SignLanguageUIActiveSelf = false;

                StartCoroutine(UICanvasVerticalSlideCoroutine(targetPosition, true));
            }
        }
        #endregion

        #region BUTTONEVENTFUNCTION
        /// <summary>
        /// 상단 버튼 눌렀을시 호출하는 함수
        /// </summary>
        public void ChangeUITab()
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

        /// <summary>
        /// 버튼이 선택되지 않았을 때 눌렀을 시 다음 UI탭으로 이동
        /// 처음 한번만 작동
        /// </summary>
        public void ChangeToNextUITab()
        {
            int unselectedIndex = FindUnselectedNextUI();

            if (unselectedIndex != -1 && !buttonHadPushed[unselectedIndex])
            {
                ChangeUITab(unselectedIndex);
                buttonHadPushed[unselectedIndex] = true;
            }

            CheckAndShowCompareButton();
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

            float A = image.color.a;

            //R0 G255 B0 A255
            image.color = new Color(0, 1, 0, A);
        }
        #endregion

        #region COMPAREANSWER
        //SignLanguage.cs에서 수화 정답 비교 후 UI변경을 위해 불러오는 함수


        /// <summary>
        /// 모든 버튼의 상호작용 여부 변경
        /// </summary>
        /// <param name="interactable"></param>
        public void SetButtonInteractable(bool interactable)
        {
            for (int i = 0; i < buttonGameObjectList.Count; i++)
            {
                buttonGameObjectList[i].GetComponent<Button>().interactable = interactable;
            }
        }

        /// <summary>
        /// 틀린 정답을 고른 버튼의 색깔을 빨간색으로 변경
        /// </summary>
        /// <param name="hierarchyIndex"></param>
        public void CheckWrongAnswerButton(int hierarchyIndex)
        {
            buttonGameObjectList[hierarchyIndex].GetComponent<Button>().interactable = true;

            Image image = buttonGameObjectList[hierarchyIndex].GetComponent<Image>();

            //R255 G0 B0 A255
            image.color = new Color(1, 0, 0, 1);
        }

        /// <summary>
        /// SignLanguage.cs, ComparingSignLanguage() 함수에서 수화 비교후 틀린 정답으로 패널을 바꾸도록 함.
        /// </summary>
        public void ChangeToWrongAnswerTab()
        {
            this.incorrectAnswerIndexList = FindIncorrectNextUI();

            if (incorrectAnswerIndexList[0] != -1)
            {
                ChangeUITab(incorrectAnswerIndexList[0]);
            }
        }

        /// <summary>
        /// ChangeToNextUI() 함수 실행 후 아래 함수를 실행하여 수화 완성 여부 판단
        /// 정답이 선택 되지 않은 것이 있는지 체크 후 없다면 compare 버튼을 등장시킴
        /// </summary>
        private void CheckAndShowCompareButton()
        {
            if (FindUnselectedNextUI() == -1)
            {
                Vector3 offsetPosition = new Vector3(0, -1065, 0);
                StartCoroutine(CompareButtonVerticalSlideCoroutine(offsetPosition));
                completeButtonHadPushed = true;
            }
        }

        #region FINDFUNCTION
        /// <summary>
        /// 선택되지 않은 UI의 인덱스를 반환
        /// </summary>
        /// <returns></returns>
        private int FindUnselectedNextUI()
        {
            if (viewportObjectList[0].GetComponent<HandCountComponent>().IsSelected == false)
            {
                return 0;
            }
            else if (viewportObjectList[1].GetComponent<SymbolAndDirectionComponent>().IsSelected == false)
            {
                return 1;
            }
            else if (viewportObjectList[2].GetComponent<HandPositionComponent>().IsSelected == false)
            {
                return 2;
            }
            else if (viewportObjectList[3].GetComponent<ParticularComponent>().IsSelected == false)
            {
                return 3;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 틀린 정답인 인덱스들의 리스트를 반환
        /// </summary>
        /// <returns></returns>
        private List<int> FindIncorrectNextUI()
        {
            List<int> incorrectIndexList = new List<int>();

            if (viewportObjectList[0].GetComponent<HandCountComponent>().IsCorrect == false)
            {
                incorrectIndexList.Add(0);
            }
            if (viewportObjectList[1].GetComponent<SymbolAndDirectionComponent>().IsCorrect == false)
            {
                incorrectIndexList.Add(1);
            }
            if (viewportObjectList[2].GetComponent<HandPositionComponent>().IsCorrect == false)
            {
                incorrectIndexList.Add(2);
            }
            if (viewportObjectList[3].GetComponent<ParticularComponent>().IsCorrect == false)
            {
                incorrectIndexList.Add(3);
            }


            if(incorrectIndexList.Count > 0)
            {
                return incorrectIndexList;
            }
            else
            {
                incorrectIndexList.Add(-1);
                return incorrectIndexList;
            }
        }
        #endregion

        #endregion

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

            IsVerticalAnimationDone = false;
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
        IEnumerator ViewportHorizontalSlideCoroutine(Vector2 viewportObjectRectPosition, float animationWaitingTime = 0)
        {
            if (!IsVerticalAnimationDone)
            {
                yield return new WaitUntil(() => IsVerticalAnimationDone == true);
            }

            yield return new WaitForSeconds(animationWaitingTime);

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
                InitCompareEventButtonPosition();
                InitViewportAndButtonOpacity();
                InitButtonInteractable();
                completeButtonHadPushed = false;

                SignLanguageUIActiveSelf = false;
                incorrectAnswerIndexList = new List<int>() { -1 };
                presentPanelIndex = 0;
            }

            IsVerticalAnimationDone = true;
            yield return null;
        }

        IEnumerator CompareButtonVerticalSlideCoroutine(Vector3 targetPosition)
        {
            #region VARIABLEFIELD
            float UIObjectX = CompareEventButton.transform.localPosition.x;
            float UIObjectZ = CompareEventButton.transform.localPosition.z;

            float velocityY = 0f;
            float smoothTime = 0.3f;

            float offset = 0.1f;
            #endregion

            //패널을 위로 부드럽게 올린다
            //목표 지점이 패널의 현재 위치보다 위에 있을 경우
            if (targetPosition.y - offset > CompareEventButton.transform.localPosition.y)
            {
                while (targetPosition.y - offset > CompareEventButton.transform.localPosition.y)
                {
                    float positionY = Mathf.SmoothDamp(CompareEventButton.transform.localPosition.y, targetPosition.y, ref velocityY, smoothTime);
                    CompareEventButton.transform.localPosition = new Vector3(UIObjectX, positionY, UIObjectZ);

                    yield return null;
                }
            }
            //목표 지점이 아래 있을 경우
            else
            {
                while (targetPosition.y + offset < CompareEventButton.transform.localPosition.y)
                {
                    float positionY = Mathf.SmoothDamp(CompareEventButton.transform.localPosition.y, targetPosition.y, ref velocityY, smoothTime);
                    CompareEventButton.transform.localPosition = new Vector3(UIObjectX, positionY, UIObjectZ);

                    yield return null;
                }
            }

            CompareEventButton.transform.localPosition = targetPosition;

            yield return null;
        }
        #endregion
    }
}
