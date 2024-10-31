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
            for (int i = 0; i < transform.childCount; i++)
            {
                //��ư ������Ʈ�� Text ������Ʈ �޾ƿ���
                TMP_Text tmp = transform.GetChild(i).transform.GetChild(0).GetComponent<TMP_Text>();
                textObject.Add(tmp);

                //�ؽ�Ʈ �Ҵ�
                textObject[i].text = ( (HandPosition) i ).ToString();
            }
        }
        #endregion

        public void SetAnswer(Position position)
        {
            answerPosition = position;
        }

        public void GetPlayerAnswer()
        {
            //������Ʈ�� hierarchy������ index �޾ƿ���
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            int clickObjectHierarchyIndex = clickObject.transform.GetSiblingIndex();

            playerAnswerPosition.HandPosition = (HandPosition) clickObjectHierarchyIndex;    
            if((HandPosition)clickObjectHierarchyIndex == answerPosition.HandPosition)
                IsCorrect = true;
            else
                IsCorrect = false;
        }
    }
}
