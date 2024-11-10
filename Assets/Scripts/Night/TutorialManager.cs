using Assets.Scripts.SignLanguage;
using HandByHand.SoundSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

///심각하게 하드코딩이 되어있음.
namespace HandByHand.NightSystem.DialogueSystem
{
    public class TutorialManager : MonoBehaviour
    {
        public DialogueFileSO DialogueFileSO;

        private Vocabulary Vocabulary;

        public TMP_Text TextComponent;

        public GameObject BlinkIcon;

        public float TextPrintDelay = 0.1f;

        public GameObject BackgroundPanel;
        public GameObject PunchParentObject;
        public GameObject upperButtonObjects;

        private GameObject punchPanel;
        private List<GameObject> punchHole = new List<GameObject>();
        private GameObject centerPunchHole;
        private GameObject meanPunchHole;

        public GameObject Character;

        public GameObject PopupVocabulary;
        public GameObject PopupCharacter;


        public bool PopupClose { get; private set; } = false;

        public void Start()
        {
            //init punchObjects
            punchPanel = PunchParentObject.transform.GetChild(0).gameObject;
            for (int i = 0; i < upperButtonObjects.transform.childCount; i++)
            {
                punchHole.Add(upperButtonObjects.transform.GetChild(i).gameObject);
            }
            centerPunchHole = PunchParentObject.transform.GetChild(2).gameObject;
            meanPunchHole = PunchParentObject.transform.GetChild(3).gameObject;

            if (PopupVocabulary.activeSelf)
            {
                PopupVocabulary.SetActive(false);
            }

            if (BlinkIcon.activeSelf)
            {
                BlinkIcon.SetActive(false);
            }

            punchPanel.SetActive(false);
        }

        public IEnumerator StartTutorial(Tutorial tutorialItem)
        {
            int itemCount = 0;

            List<DialogueItem> itemList = new List<DialogueItem>(DialogueFileSO.DialogueItemList);

            Vocabulary = tutorialItem.Vocabulary;


            while (true)
            {
                if (itemCount >= itemList.Count)
                {
                    break;
                }

                BlinkIcon.SetActive(false);

                switch (itemCount)
                {
                    case 3:
                        BackgroundPanel.SetActive(false);
                        punchPanel.SetActive(true);
                        punchPanel.transform.SetParent(punchHole[0].transform);
                        break;
                    case 4:
                        punchPanel.transform.SetParent(punchHole[1].transform);
                        break;
                    case 5:
                        punchPanel.transform.SetParent(punchHole[2].transform);
                        break;
                    case 6:
                        punchPanel.transform.SetParent(punchHole[3].transform);
                        break;
                    case 7:
                        punchPanel.transform.SetParent(meanPunchHole.transform);
                        break;
                    case 8:
                        //옆으로 밀어 안 보이게 해두기
                        Character.GetComponent<RectTransform>().anchoredPosition += new Vector2(600, 0);
                        punchPanel.transform.SetParent(centerPunchHole.transform);

                        PopupVocabulary.SetActive(true);

                        yield return StartCoroutine(TextPrintAnimation(((NPCText)itemList[itemCount]).Text));

                        BlinkIcon.SetActive(true);

                        yield return StartCoroutine(WaitUntilTouchInput());

                        BlinkIcon.SetActive(false);

                        yield return StartCoroutine(SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(PopupCharacter, Vocabulary));

                        PopupVocabulary.SetActive(false);

                        punchPanel.transform.SetParent(PunchParentObject.transform);

                        itemCount++;
                        continue;
                    case 9:
                        Character.GetComponent<RectTransform>().anchoredPosition -= new Vector2(600, 0);

                        punchPanel.transform.SetParent(PunchParentObject.transform);

                        BackgroundPanel.SetActive(true);

                        break;
                }

                //Print Text
                yield return StartCoroutine(TextPrintAnimation(((NPCText)itemList[itemCount]).Text));

                BlinkIcon.SetActive(true);

                yield return StartCoroutine(WaitUntilTouchInput());

                //Play Click SE
                SoundManager.Instance.PlaySE(SoundName.Click);

                itemCount++;
            }
        }

        IEnumerator TextPrintAnimation(string text)
        {
            int count = 0;
            int textLength = text.Length;

            TextComponent.SetText("");

            while (count != textLength)
            {
                TextComponent.text += text[count].ToString();

                //색상 추가
                if (text[count].ToString() == "<")
                {
                    while (text[count].ToString() != ">")
                    {
                        count++;
                        TextComponent.text += text[count].ToString();
                    }
                }

                count++;
                yield return new WaitForSeconds(TextPrintDelay);
            }
        }

        IEnumerator WaitUntilTouchInput()
        {
            while (!(Input.touchCount > 0))
            {
                yield return null;
            }

            yield return null;
        }

        public void ClosePopupVocabulary()
        {
            PopupVocabulary.SetActive(false);
            PopupClose = true;
        }

    }
}