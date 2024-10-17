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
        public ButtonListUIComponent buttonListUIComponent;
        public ViewportListUIComponent viewportListUIComponent;

        private List<TMP_Text> buttonText;
        private List<GameObject> UIObject;
        #endregion

        private void Awake()
        {
            buttonText = buttonListUIComponent.buttonText;
            UIObject = viewportListUIComponent.UIObject;
        }

        void Start()
        {
            //오브젝트 비활성화
            for (int i = 0; i < UIObject.Count; i++)
            { UIObject[i].SetActive(false); }

            //HandCount오브젝트만 활성화
            UIObject[0].SetActive(true);
        }

        public void ChangeUIObject()
        {
            //함수를 부른 버튼 오브젝트의 이름 받기
            string eventButtonName = EventSystem.current.currentSelectedGameObject.name;

            for(int i = 0; i < UIObject.Count; i++)
            {
                UIObject[i].SetActive(false);
            }

            switch(eventButtonName)
            {
                case "HandCountButton":
                    UIObject[0].SetActive(true);
                    break;

                case "HandSymbolAndDirectionButton":
                    UIObject[1].SetActive(true);
                    break;

                case "HandPositionButton":
                    UIObject[2].SetActive(true);
                    break;

                case "ParticularButton":
                    UIObject[3].SetActive(true);
                    break;
            }
        }


        
    }
}
