using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class HandPositionComponent : MonoBehaviour
    {
        #region VARIABLEFIELD
        Position answerPosition = new Position();

        Position playerAnswerPosition = new Position();

        List<TMP_Text> textObject = new List<TMP_Text>();

        public bool IsCorrect { get; private set; }

        public bool IsSelected { get; private set; } = false;

        [SerializeField]
        private int ignoreLayoutNumber = 1;
        #endregion

        #region INIT
        void Awake()
        {
            answerPosition.HandPosition = HandPosition.None;
            playerAnswerPosition.HandPosition = HandPosition.None;
            IsCorrect = false;
        }

        void Start()
        {
            for (int i = ignoreLayoutNumber; i < transform.childCount; i++)
            {
                //버튼 오브젝트의 Text 컴포넌트 받아오기
                TMP_Text tmp = transform.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>();
                textObject.Add(tmp);

                int index = i - ignoreLayoutNumber;
                //텍스트 할당
                string buttonText = "";
                switch(index)
                {
                    case 0:
                        buttonText = "머리 위";
                        break;
                    case 1:
                        buttonText = "얼굴";
                            break;
                    case 2:
                        buttonText = "턱";
                        break;
                    case 3:
                        buttonText = "가슴";
                        break;
                    case 4:
                        buttonText = "배";
                        break;
                }
                textObject[index].text = buttonText;
            }
        }
        #endregion

        public void SetAnswer(Position position)
        {
            answerPosition = position;
        }

        public void GetPlayerAnswer()
        {
            //오브젝트의 hierarchy에서의 index 받아오기
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            int clickObjectHierarchyIndex = clickObject.transform.GetSiblingIndex();

            playerAnswerPosition.HandPosition = (HandPosition) (clickObjectHierarchyIndex - ignoreLayoutNumber);

            IsSelected = true;

            if(playerAnswerPosition.HandPosition == answerPosition.HandPosition)
                IsCorrect = true;
            else
                IsCorrect = false;
        }

        public void InitBoolean()
        {
            IsSelected = false;
            IsCorrect = false;
        }
    }
}
