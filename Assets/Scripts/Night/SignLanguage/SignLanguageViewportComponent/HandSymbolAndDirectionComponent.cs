using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class SymbolAndDirectionComponent : MonoBehaviour
    {
        #region VARIABLEFIELD
        SymbolAndDirection answerSymbolAndDirection = new SymbolAndDirection();
        int answerChoiceGameObjectIndex;

        SymbolAndDirection playerAnswerSymbolAndDirection = new SymbolAndDirection();
        int playerAnswerChoiceGameObjectIndex;

        List<Image> choiceGameObjectImageComponentList = new List<Image>();
        public bool IsSelected { get; private set; } = false;

        public bool IsCorrect { get; private set; }

        //������ �̹����� �����ϱ� ���� �޾ƿ� �̹��� �迭
        Sprite[] choiceSprite;

        public int IgnoreLayoutIndex = 0;
        #endregion

        #region INIT
        void Awake()
        {
            #region INITVARIABLEFIELD
            answerSymbolAndDirection.Sprite = null;
            playerAnswerSymbolAndDirection.Sprite = null;
            IsCorrect = false;

            Image childGameObjectImageComponent;

            for (int i = IgnoreLayoutIndex; i < transform.childCount; i++)
            {
                childGameObjectImageComponent = transform.GetChild(i).gameObject.GetComponent<Image>();
                choiceGameObjectImageComponentList.Add(childGameObjectImageComponent);
            }
            #endregion
        }

        void Start()
        {
            //������ �̹����� ������ �̹����� �迭�� �޾ƿ´�
            choiceSprite = Resources.LoadAll<Sprite>("SymbolAndDirectionSprite");
            if (choiceSprite == null)
            {
                Debug.Log("Resource is null, in HandSymbolAndDirectionComponent.cs");
            }
        }
        #endregion

        public void GetPlayerAnswer()
        {
            //������Ʈ�� hierarchy������ sprite �޾ƿ���
            GameObject clickObject = EventSystem.current.currentSelectedGameObject;
            int clickObjectHierarchyIndex = clickObject.transform.GetSiblingIndex() - IgnoreLayoutIndex;
            Sprite clickObjectSprite = clickObject.GetComponent<Image>().sprite;


            playerAnswerSymbolAndDirection.Sprite = clickObjectSprite;
            playerAnswerChoiceGameObjectIndex = clickObjectHierarchyIndex;

            IsSelected = true;

            if (answerChoiceGameObjectIndex == playerAnswerChoiceGameObjectIndex)
                IsCorrect = true;
            else
                IsCorrect = false;
        }

        #region SETANSWER
        public void SetAnswer(SymbolAndDirection symbolAndDirection)
        {
            answerSymbolAndDirection.Sprite = symbolAndDirection.Sprite;

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

                    //�Ȱ��� ���� ������ ���� �ߺ� ���� && ���� �ߺ� ����
                    if(!drawedNumber.Contains(randomNumber) && randomSprite != answerSymbolAndDirection.Sprite)
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

            choiceGameObjectImageComponentList[randomNumber].sprite = answerSymbolAndDirection.Sprite;
            answerChoiceGameObjectIndex = randomNumber;
        }
        #endregion

        public void InitBoolean()
        {
            IsSelected = false;
            IsCorrect = false;
        }
    }
}
