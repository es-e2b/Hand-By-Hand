using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class WhatIsSelectedComponent : MonoBehaviour
    {
        private List<Image> imageComponentList = new List<Image>();
        private List<Button> buttonComponentList = new List<Button>();

        private int formerSelectedButtonIndex = -1;
        [SerializeField]
        private int ignoreLayoutIndex = 0;

        public float unselectImageOpacity = 100f;
        //������ �̹����� ���� ������
        public float selectImageOpacity = 255f;

        //������Ʈ�� �ʱ�ȭ �� ��쿡 �Ҵ��� ������Ʈ�� ���� ����
        private float originalImageOpacity;

        #region INIT
        void Start()
        {
            for (int i = ignoreLayoutIndex; i < transform.childCount; i++)
            {
                buttonComponentList.Add(transform.GetChild(i).GetComponent<Button>());

                try
                {
                    imageComponentList.Add(transform.GetChild(i).GetComponent<Image>());
                }
                catch { continue; }
            }

            for (int i = 0; i < buttonComponentList.Count; i++)
            {
                buttonComponentList[i].onClick.AddListener(AdjustOpacityExceptSelectedButton);
            }

            originalImageOpacity = ((Color32)imageComponentList[0].color).a;
        }

        public void Init()
        {
            formerSelectedButtonIndex = -1;

            for (int i = 0; i < imageComponentList.Count; i++)
            {
                imageComponentList[i].color = new Color32(255, 255, 255, (byte)originalImageOpacity);
            }
        }
        #endregion

        //SignLanguageUIManager���� ChangeToTextUI()�Լ� ����� ��� ��ư ������Ʈ�� Opacity�� ������ ���� ������ ���� �Լ�
        public void AdjustOpacityOfIndex(int index)
        {
            StartCoroutine(AdjustOpacity(index));
        }

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

            StartCoroutine(AdjustOpacity(clickObjectHierarchyIndex));
        }

        IEnumerator AdjustOpacity(int selectedButtonIndex)
        {
            #region OFFSETVARIABLE
            float duringTime = 0.5f;
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

            yield return null;
        }
    }
}
