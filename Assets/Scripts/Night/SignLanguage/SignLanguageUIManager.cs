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

        private List<TMP_Text> buttonText;
        private List<GameObject> scrollViewObject;

        private GameObject UIObjectInSignLanguageCanvas;
        private Vector3 originalUIObjectPosition;
        private float screenHeight = Screen.height;
        #endregion

        #region INIT
        private void Awake()
        {
            buttonText = ButtonListUIComponent.buttonText;
            scrollViewObject = ViewportListUIComponent.UIObject;
            UIObjectInSignLanguageCanvas = SignLanguageCanvas.transform.Find("UIObject").gameObject;
            originalUIObjectPosition = UIObjectInSignLanguageCanvas.transform.localPosition;
        }

        private void Start()
        {
            //UIObjectInSignLanguageCanvas.transform.position -= new Vector3(0, screenHeight, 0);

            #region OBJECTINACTIVE
            /*if (SignLanguageCanvas.activeSelf)
            {
                //오브젝트 비활성화
                for (int i = 0; i < scrollViewObject.Count; i++)
                { scrollViewObject[i].SetActive(false); }

                //HandCount오브젝트만 활성화
                scrollViewObject[0].SetActive(true);

                //캔버스 비활성화
                SignLanguageCanvas.SetActive(false);
            } */
            #endregion
        }
        #endregion

        #region SETACTIVEFUNCTION
        public void ActiveUIObject()
        {
            if (SignLanguageCanvas.activeSelf)
            {
                Vector3 targetPosition = originalUIObjectPosition + new Vector3(0, screenHeight, 0);

                StartCoroutine( UIObjectVerticalSlideCoroutine(targetPosition) );
            }
        }

        public void InActiveUIObject()
        {
            if (SignLanguageCanvas.activeSelf)
            {
                Vector3 targetPosition = originalUIObjectPosition;

                StartCoroutine( UIObjectVerticalSlideCoroutine(targetPosition) );
            }
        }
        #endregion


        public void ChangeUIObject()
        {
            //함수를 부른 버튼 오브젝트의 이름 받기
            string eventButtonName = EventSystem.current.currentSelectedGameObject.name;

            for (int i = 0; i < scrollViewObject.Count; i++)
            {
                scrollViewObject[i].SetActive(false);
            }

            switch (eventButtonName)
            {
                case "HandCountButton":
                    scrollViewObject[0].SetActive(true);
                    break;

                case "HandSymbolAndDirectionButton":
                    scrollViewObject[1].SetActive(true);
                    break;

                case "HandPositionButton":
                    scrollViewObject[2].SetActive(true);
                    break;

                case "ParticularButton":
                    scrollViewObject[3].SetActive(true);
                    break;
            }
        }

        #region COROUTINE
        /// <summary>
        /// 현재 위치에서 targetPosition으로 부드럽게 이동
        /// </summary>
        /// <param name="targetPosition">목표 지점</param>
        /// <returns></returns>
        IEnumerator UIObjectVerticalSlideCoroutine(Vector3 targetPosition)
        {
            //변수 선언
            #region VARIABLEFIELD
            float UIObjectX = UIObjectInSignLanguageCanvas.transform.localPosition.x;
            float UIObjectZ = UIObjectInSignLanguageCanvas.transform.localPosition.z;

            float velocityY = 0f;
            float smoothTime = 0.3f;

            float offset = 0.1f;
            #endregion

            //패널을 아래로 부드럽게 올린다
            while (targetPosition.y - offset > UIObjectInSignLanguageCanvas.transform.localPosition.y)
            {
                float positionY = Mathf.SmoothDamp(UIObjectInSignLanguageCanvas.transform.localPosition.y, targetPosition.y, ref velocityY, smoothTime);
                UIObjectInSignLanguageCanvas.transform.localPosition = new Vector3(UIObjectX, positionY, UIObjectZ);

                yield return null;
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
