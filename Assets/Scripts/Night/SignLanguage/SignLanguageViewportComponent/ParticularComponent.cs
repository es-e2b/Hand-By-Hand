using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class ParticularComponent : MonoBehaviour
    {
        #region VARIABLEFIELD
        Particular answerParticular = new Particular();

        Particular playerAnswerParticular = new Particular();

        List<Image> choiceGameObjectImageComponentList = new List<Image>();

        Sprite[] choiceSprite;

        int answerChoiceGameObjectIndex;

        public bool IsCorrect { get; private set; }
        #endregion

        #region INIT
        void Awake()
        {
            answerParticular._Particular = ParticularEnum.None;
            playerAnswerParticular._Particular = ParticularEnum.None;
        }

        void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Image imageComponent = transform.GetChild(i).GetComponent<Image>();
                choiceGameObjectImageComponentList.Add(imageComponent);
            }

            //무작위 이미지로 삽입할 이미지를 배열에 받아온다
            choiceSprite = Resources.LoadAll<Sprite>("ParticularSprite");
            if (choiceSprite == null)
            {
                Debug.Log("Resource is null, in ParticularComponenet.cs");
            }
        }
        #endregion

        public void GetPlayerAnswer()
        {
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            int clickObjectHierarchyIndex = clickObject.transform.GetSiblingIndex();

            if(clickObjectHierarchyIndex == answerChoiceGameObjectIndex)
            {
                IsCorrect = true;
            }
            else
                IsCorrect = false;
        }

        #region SETANSWER
        public void SetAnswer(Particular particular)
        {
            answerParticular._Particular = particular._Particular;
            answerParticular.Sprite = particular.Sprite;

            SetRandomChoice(transform.childCount);
            SetCorrectChoice(transform.childCount);
        }

        private void SetRandomChoice(int childCount)
        {
            int randomNumber;
            Sprite randomSprite;
            List<int> drawedNumber = new List<int>();

            for (int i = 0; i < childCount; i++)
            {
                while(true)
                {
                    randomNumber = Random.Range(0, choiceSprite.Length);
                    randomSprite = choiceSprite[randomNumber];

                    //똑같은 랜덤 선택지 지정 중복 방지 && 정답 중복 방지
                    if (!drawedNumber.Contains(randomNumber) && randomSprite != answerParticular.Sprite)
                    {
                        drawedNumber.Add(randomNumber);
                        choiceGameObjectImageComponentList[i].sprite = randomSprite;
                        break;
                    }
                }
            }
        }

        private void SetCorrectChoice(int childCount)
        {
            int randomNumber = Random.Range(0, childCount);

            choiceGameObjectImageComponentList[randomNumber].sprite = answerParticular.Sprite;
            answerChoiceGameObjectIndex = randomNumber;
        }
        #endregion
    }
}
