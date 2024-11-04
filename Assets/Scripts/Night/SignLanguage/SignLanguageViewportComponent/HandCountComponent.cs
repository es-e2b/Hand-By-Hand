using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class HandCountComponent : MonoBehaviour
    {
        private HandCount answerHandCount = new HandCount();

        private HandCount playerAnswerHandCount = new HandCount();

        public bool IsCorrect { get; private set; }

        void Awake()
        {
            answerHandCount.UsingHand = UsingHand.None;
            playerAnswerHandCount.UsingHand = UsingHand.None;
            IsCorrect = false;
        }

        public void SetAnswer(HandCount handCount)
        {
            answerHandCount.UsingHand = handCount.UsingHand;
        }

        public void GetPlayerAnswer()
        {
            //오브젝트의 hierarchy에서의 인덱스 받아오기
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            int clickObjectHierarchyIndex = clickObject.transform.GetSiblingIndex();

            UsingHand usingHand = (UsingHand)clickObjectHierarchyIndex;
            playerAnswerHandCount.UsingHand = usingHand;

            if(playerAnswerHandCount.UsingHand == answerHandCount.UsingHand)
                IsCorrect = true;
            else
                IsCorrect = false;
        }

        public void InitBoolean()
        {
            IsCorrect = false;
        }
    }
}