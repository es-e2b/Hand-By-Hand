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
        //������ �̹����� ���� ������
        public float selectImageOpacity = 255f;

        public float unselectOutlineImageOpacity = 150f;


        private List<Image> imageComponentList = new List<Image>();
        private List<Button> buttonComponentList = new List<Button>();
        private List<GameObject> buttonGameObject = new List<GameObject>();


        //������Ʈ�� �ʱ�ȭ �� ��쿡 �Ҵ��� ������Ʈ�� ���� ����
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
            //����Ʈ�� ������Ʈ �Ҵ�
            for (int i = ignoreLayoutIndex; i < transform.childCount; i++)
            {
                buttonComponentList.Add(transform.GetChild(i).GetComponent<Button>());
                imageComponentList.Add(transform.GetChild(i).GetComponent<Image>());

                buttonGameObject.Add(transform.GetChild(i).gameObject);
            }

            //��ư ������Ʈ�� ������ �߰�
            for (int i = 0; i < buttonComponentList.Count; i++)
            {
                buttonComponentList[i].onClick.AddListener(AdjustOpacityExceptSelectedButton);
                //buttonComponentList[i].onClick.AddListener(AdjustObjectScale);
            }

            //�ʱ�ȭ�� ����� ������Ʈ ���� ����
            originalImageOpacity = ((Color32)imageComponentList[0].color).a;
            originalObjectScale = imageComponentList[0].gameObject.transform.localScale;
        }

        public void Init()
        {
            formerSelectedButtonIndex = -1;

            for (int i = 0; i < imageComponentList.Count; i++)
            {
                //�̹��� ���� ����
                imageComponentList[i].color = new Color32(255, 255, 255, (byte)originalImageOpacity);
                //ũ�� ����
                imageComponentList[i].gameObject.transform.localScale = originalObjectScale;
            }
        }
        #endregion

        /// <summary>
        /// SignLanguageUIManager���� ChangeToTextUI()�Լ� ����� ��� ��ư ������Ʈ�� Opacity�� ������ ���� ������ ���� �Լ�
        /// ��ư Ŭ���� �ҷ����� ���� �ƴϰ� ��ũ��Ʈ �� �޼ҵ�� ����ϱ� ���� �Լ�
        /// </summary>
        /// <param name="index"></param>
        public void AdjustOpacityOfIndex(int index)
        {
            StartCoroutine(AdjustButtonImageOpacity(index));
        }

        #region BUTTONEVENT
        /// <summary>
        /// ��ư Ŭ���� �ش� �Լ��� �ҷ���
        /// </summary>
        private void AdjustOpacityExceptSelectedButton()
        {
            //������Ʈ�� hierarchy������ �ε��� �޾ƿ���
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            int clickObjectHierarchyIndex = clickObject.transform.GetSiblingIndex() - ignoreLayoutIndex;

            //������ ��ư �� ������ �ߴ�
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
            //������ ��ư �� ������ �ߴ�
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
        /// ��ư Ŭ���� ������Ʈ ũ�� ����
        /// </summary>
        /*
        private void AdjustObjectScale()
        {
            //������Ʈ�� hierarchy������ �ε��� �޾ƿ���
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            int clickObjectHierarchyIndex = clickObject.transform.GetSiblingIndex() - ignoreLayoutIndex;

            //������ ��ư �� ������ �ߴ�
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
        /// selectedButton�� ������ �ø��� ������ Button�� ������ ���ߴ� �ڷ�ƾ
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

            //�ڷ�ƾ ������ ����� ������ velocity ����
            float unselectedImageOpacityTypeFloat;
            byte unselectedImageOpacityTypeByte;

            float selectedImageOpacityTypeFloat;
            byte selectedImageOpacityTypeByte;
            #endregion

            //�̹����� RGB�� ���� 255��� �����Ͽ� ����
            while (time < duringTime)
            {
                ///!if���� ��ư ������Ʈ ���� ������ ����Ͽ� �ۼ��� �ϵ� �ڵ�! �����ϱ� ����� �� ����.
                //��ư�� �������� ���ٸ� ���� ������ ���� ����
                if (formerSelectedButtonIndex == -1)
                {
                    unselectedImageOpacityTypeFloat = imageOriginalOpacity - ((imageOriginalOpacity - unselectImageOpacity) * (time * (1 / duringTime)));
                }
                //�������� �ִٸ� ���� �̹��� ���� ���� ���� ���� ����
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

                    //������ ��ư�Ͻ�
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
                    //���ϸ� ���̱� ���� �ѹ� ���� ���� �ִٸ� ������ ��ư�� ���� ����
                    //�������� ���ٸ� formerSelectedButtonIndex == -1
                    if (formerSelectedButtonIndex == -1)
                    {
                        imageComponentList[i].color = new Color32(R, G, B, unselectedImageOpacityTypeByte);
                    }
                    //�����ٸ� formerSelectedButtonIndex�� �Ҵ�Ǿ� �ִ� �ε��� ������Ʈ�� ����
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

            //���� ���� �ִϸ��̼��� �������� ���� ���� ������ ����� ���������� ���� �Ҵ�
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
        /// ��ư Ŭ���� ������Ʈ ũ�� ����
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
