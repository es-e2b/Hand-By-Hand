using HandByHand.SoundSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class WhatIsSelectedComponent : MonoBehaviour
    {
        #region VARIABLE

        [SerializeField]
        private int ignoreLayoutIndex = 0;

        public float adjustScaleOffset = 0.05f;

        public float unselectImageOpacity = 150f;
        //선택한 이미지가 얻을 투명도값
        public float selectImageOpacity = 255f;

        public float unselectOutlineImageOpacity = 150f;


        private List<Image> imageComponentList = new List<Image>();
        private List<Button> buttonComponentList = new List<Button>();
        private List<GameObject> buttonGameObject = new List<GameObject>();


        //오브젝트를 초기화 할 경우에 할당할 오브젝트의 기존 투명도
        private float originalImageOpacity;
        private float originalOutlineImageOpacity;
        private Vector3 originalObjectScale;

        private int formerSelectedButtonIndex = -1;

        Coroutine AdjustButtonImageOpacityCoroutine = null;
        Coroutine AdjustObjectScaleCoroutine = null;

        private bool isInputOn = true;
        #endregion

        #region INIT
        void Awake()
        {
            //리스트에 오브젝트 할당
            for (int i = ignoreLayoutIndex; i < transform.childCount; i++)
            {
                buttonComponentList.Add(transform.GetChild(i).GetComponent<Button>());
                imageComponentList.Add(transform.GetChild(i).GetComponent<Image>());

                buttonGameObject.Add(transform.GetChild(i).gameObject);
            }

            //버튼 오브젝트에 리스너 추가
            for (int i = 0; i < buttonComponentList.Count; i++)
            {
                buttonComponentList[i].onClick.AddListener(AdjustOpacityExceptSelectedButton);
                //buttonComponentList[i].onClick.AddListener(AdjustObjectScale);
            }

            //초기화시 사용할 오브젝트 기존 투명도
            originalImageOpacity = ((Color32)imageComponentList[0].color).a;
            originalObjectScale = imageComponentList[0].gameObject.transform.localScale;
        }

        public void Init()
        {
            formerSelectedButtonIndex = -1;

            for (int i = 0; i < imageComponentList.Count; i++)
            {
                //이미지 투명도 조정
                imageComponentList[i].color = new Color32(255, 255, 255, (byte)originalImageOpacity);
                //크기 조정
                imageComponentList[i].gameObject.transform.localScale = originalObjectScale;
            }
        }
        #endregion

        /// <summary>
        /// SignLanguageUIManager에서 ChangeToTextUI()함수 실행시 상단 버튼 오브젝트의 Opacity값 변경을 위해 별도로 만든 함수
        /// 버튼 클릭시 불러오는 것이 아니고 스크립트 내 메소드로 사용하기 위한 함수
        /// </summary>
        /// <param name="index"></param>
        public void AdjustOpacityOfIndex(int index)
        {
            StartCoroutine(AdjustButtonImageOpacity(index));
        }

        #region BUTTONEVENT
        /// <summary>
        /// 버튼 클릭시 해당 함수를 불러옴
        /// </summary>
        private void AdjustOpacityExceptSelectedButton()
        {
            //오브젝트의 hierarchy에서의 인덱스 받아오기
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            int clickObjectHierarchyIndex = clickObject.transform.GetSiblingIndex() - ignoreLayoutIndex;

            //눌렀던 버튼 또 누를시 중단
            if (clickObjectHierarchyIndex == formerSelectedButtonIndex)
            {
                return;
            }

            if(isInputOn == false)
            {
                return;
            }
            else
            {
                isInputOn = false;
            }

            AdjustObjectScale(clickObjectHierarchyIndex);

            if (AdjustButtonImageOpacityCoroutine != null)
            {
                StopCoroutine(AdjustButtonImageOpacityCoroutine);
                AdjustButtonImageOpacityCoroutine = null;
            }

            AdjustButtonImageOpacityCoroutine = StartCoroutine(AdjustButtonImageOpacity(clickObjectHierarchyIndex));
            Invoke("OnInput", 0.6f);
        }

        private void OnInput()
        {
            isInputOn = true;
        }

        private void AdjustObjectScale(int clickObjectHierarchyIndex)
        {
            //눌렀던 버튼 또 누를시 중단
            if (clickObjectHierarchyIndex == formerSelectedButtonIndex)
            {
                return;
            }

            if (AdjustObjectScaleCoroutine != null)
            {
                StopCoroutine(AdjustObjectScaleCoroutine);
                AdjustObjectScaleCoroutine = null;
            }

            AdjustObjectScaleCoroutine = StartCoroutine(AdjustObjectSize(clickObjectHierarchyIndex));
        }

        /// <summary>
        /// 버튼 클릭시 오브젝트 크기 조정
        /// </summary>
        /*
        private void AdjustObjectScale()
        {
            //오브젝트의 hierarchy에서의 인덱스 받아오기
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            int clickObjectHierarchyIndex = clickObject.transform.GetSiblingIndex() - ignoreLayoutIndex;

            //눌렀던 버튼 또 누를시 중단
            if (clickObjectHierarchyIndex == formerSelectedButtonIndex)
            {
                return;
            }

            if (isInputOn == false)
            {
                return;
            }

            if (AdjustObjectScaleCoroutine != null)
            {
                StopCoroutine(AdjustObjectScaleCoroutine);
                AdjustObjectScaleCoroutine = null;
            }

            AdjustObjectScaleCoroutine = StartCoroutine(AdjustObjectSize(clickObjectHierarchyIndex));
        }
        */
        #endregion

        #region COROUTINE
        /// <summary>
        /// selectedButton은 투명도를 올리고 나머지 Button은 투명도를 낮추는 코루틴
        /// </summary>
        /// <param name="selectedButtonIndex">list index</param>
        /// <returns></returns>
        IEnumerator AdjustButtonImageOpacity(int selectedButtonIndex)
        {
            #region OFFSETVARIABLE
            float duringTime = 0.25f;
            #endregion

            #region STATICVARIABLE
            //static variable
            float time = 0;

            byte R;
            byte G;
            byte B;

            Color32 color32 = imageComponentList[0].color;
            float imageOriginalOpacity = (float)(color32.a);

            //코루틴 내에서 사용할 가변적 velocity 변수
            float unselectedImageOpacityTypeFloat;
            byte unselectedImageOpacityTypeByte;

            float selectedImageOpacityTypeFloat;
            byte selectedImageOpacityTypeByte;
            #endregion

            //이미지의 RGB는 전부 255라는 가정하에 조정
            while (time < duringTime)
            {
                ///!if문은 버튼 오브젝트 투명도 조절을 고려하여 작성한 하드 코드! 이해하기 어려울 수 있음.
                //버튼을 누른적이 없다면 기존 투명도를 통해 연산
                if (formerSelectedButtonIndex == -1)
                {
                    unselectedImageOpacityTypeFloat = imageOriginalOpacity - ((imageOriginalOpacity - unselectImageOpacity) * (time * (1 / duringTime)));
                }
                //누른적이 있다면 선택 이미지 투명도 변수 값을 통해 연산
                else
                {
                    unselectedImageOpacityTypeFloat = selectImageOpacity - ((selectImageOpacity - unselectImageOpacity) * (time * (1 / duringTime)));
                }
                unselectedImageOpacityTypeByte = (byte)unselectedImageOpacityTypeFloat;

                selectedImageOpacityTypeFloat = ((selectImageOpacity - imageOriginalOpacity) * (time * (1 / duringTime))) + imageOriginalOpacity;
                selectedImageOpacityTypeByte = (byte)selectedImageOpacityTypeFloat;


                for (int i = 0; i < imageComponentList.Count; i++)
                {
                    R = ((Color32)imageComponentList[i].color).r;
                    G = ((Color32)imageComponentList[i].color).g;
                    B = ((Color32)imageComponentList[i].color).b;

                    //선택한 버튼일시
                    if (i == selectedButtonIndex)
                    {
                        if (imageComponentList[i].color == new Color32(R, G, B, (byte)selectImageOpacity))
                        {
                            continue;
                        }

                        else
                        {
                            imageComponentList[i].color = new Color32(R, G, B, selectedImageOpacityTypeByte);
                        }
                    }
                    //부하를 줄이기 위해 한번 누른 적이 있다면 눌렀던 버튼만 투명도 조정
                    //누른적이 없다면 formerSelectedButtonIndex == -1
                    if (formerSelectedButtonIndex == -1)
                    {
                        imageComponentList[i].color = new Color32(R, G, B, unselectedImageOpacityTypeByte);
                    }
                    //눌렀다면 formerSelectedButtonIndex에 할당되어 있는 인덱스 오브젝트만 조정
                    else
                    {
                        if (i == formerSelectedButtonIndex)
                        {
                            imageComponentList[i].color = new Color32(R, G, B, unselectedImageOpacityTypeByte);
                        }
                    }
                }

                time += Time.deltaTime;
                yield return null;
            }

            //투명도 조정 애니메이션이 끝났을시 변수 연산 오차를 고려해 최종적으로 변수 할당
            for (int i = 0; i < imageComponentList.Count; i++)
            {
                R = ((Color32)imageComponentList[i].color).r;
                G = ((Color32)imageComponentList[i].color).g;
                B = ((Color32)imageComponentList[i].color).b;

                if (i == selectedButtonIndex)
                {
                    imageComponentList[i].color = new Color32(R, G, B, (byte)selectImageOpacity);
                    continue;
                }
                imageComponentList[i].color = new Color32(R, G, B, (byte)unselectImageOpacity);
            }

            formerSelectedButtonIndex = selectedButtonIndex;

            AdjustButtonImageOpacityCoroutine = null;
            yield break;
        }

        /// <summary>
        /// 버튼 클릭시 오브젝트 크기 조정
        /// </summary>
        /// <param name="selectedButtonIndex"></param>
        /// <returns></returns>
        IEnumerator AdjustObjectSize(int selectedButtonIndex)
        {
            float maxSize = originalObjectScale.x + adjustScaleOffset;
            float minSize = originalObjectScale.x - adjustScaleOffset;

            float offset = 0.005f;

            float velocityY = 0;
            float smoothTime = 0.01f;

            float selectedObjectScale;

            while (buttonGameObject[selectedButtonIndex].transform.localScale.x < maxSize - offset)
            {
                for (int i = 0; i < buttonComponentList.Count; i++)
                {
                    if (i == selectedButtonIndex)
                    {
                        selectedObjectScale = Mathf.SmoothDamp(buttonGameObject[i].transform.localScale.x, maxSize, ref velocityY, smoothTime);
                        buttonGameObject[i].transform.localScale = new Vector3(selectedObjectScale, selectedObjectScale, 0);
                    }
                    else
                    {
                        selectedObjectScale = Mathf.SmoothDamp(buttonGameObject[i].transform.localScale.x, minSize, ref velocityY, smoothTime);
                        buttonGameObject[i].transform.localScale = new Vector3(selectedObjectScale, selectedObjectScale, 0);
                    }
                }

                yield return null;
            }

            for (int i = 0; i < buttonComponentList.Count; i++)
            {
                if (i == selectedButtonIndex)
                {
                    buttonGameObject[i].transform.localScale = new Vector3(maxSize, maxSize, 0);
                }
                else
                {
                    selectedObjectScale = Mathf.SmoothDamp(buttonGameObject[i].transform.localScale.x, minSize, ref velocityY, smoothTime);
                    buttonGameObject[i].transform.localScale = new Vector3(selectedObjectScale, selectedObjectScale, 0);
                }
            }

            AdjustObjectScaleCoroutine = null;
            yield break;
        }
        #endregion
    }
}
