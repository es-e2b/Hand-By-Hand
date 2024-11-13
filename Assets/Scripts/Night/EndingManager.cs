using Assets.Scripts;
using Assets.Scripts.SignLanguage;
using HandByHand.NightSystem.DialogueSystem;
using HandByHand.SoundSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HandByHand.NightSystem
{
    public class EndingManager : MonoBehaviour
    {
        public DialogueFileSO endingDialogue;

        [Header("UI")]
        [SerializeField]
        private Image endingBackground;

        [SerializeField]
        private Image fadePanel;

        [SerializeField]
        private Image fadePanel2;

        [SerializeField]
        private Image fadePanel3;

        [SerializeField]
        private TMP_Text _NPCText;

        [SerializeField]
        private GameObject blinkIcon;

        [SerializeField]
        private Image textPanel;

        [SerializeField]
        private Image endingScreen;

        [SerializeField]
        private Image endingImage;

        [SerializeField]
        private Image choice;

        [SerializeField]
        private TMP_Text[] endingText = new TMP_Text[3];

        private string[] endingString = new string[3] { "당신의 선택으로 주인공은 마을에 남아 마을 주민들과 행복하게 살았습니다.",
                                                        "그렇게 영원히, 행복하게 살았답니다.", "…The End."};

        [Header("Variable")]
        [SerializeField]
        float fadeTime = 3f;

        private void Awake()
        {
            endingBackground.color = new Color(1, 1, 1, 0);

            fadePanel.color = new Color(0, 0, 0, 0);
            fadePanel2.color = new Color(0, 0, 0, 0);
            fadePanel3.color = new Color(0, 0, 0, 0);

            textPanel.color = new Color(1, 1, 1, 0);

            endingScreen.color = new Color32(100, 100, 100, 0);
            endingImage.color = new Color(1, 1, 1, 0);

            choice.color = new Color(1, 1, 1, 0);

            blinkIcon.SetActive(false);
        }

        private void Start()
        {
            StartCoroutine(StartEnding());
        }

        IEnumerator StartEnding()
        {
            float waitingOffset = 0.5f;

            //밤 씬 전제 페이드 아웃
            yield return StartCoroutine(AdjustImageAlpha(fadePanel, true));

            yield return new WaitForSeconds(waitingOffset);

            yield return StartCoroutine(AdjustImageAlpha(endingBackground, true));

            yield return new WaitForSeconds(waitingOffset);

            yield return StartCoroutine(AdjustImageAlpha(textPanel, true));

            yield return new WaitForSeconds(waitingOffset);

            int itemCount = 0;

            List<DialogueItem> itemList = new List<DialogueItem>(endingDialogue.DialogueItemList);

            while (true)
            {
                if (itemCount >= itemList.Count)
                {
                    break;
                }

                blinkIcon.SetActive(false);

                //Print Text
                yield return StartCoroutine(TextPrintAnimation(_NPCText, ((NPCText)itemList[itemCount]).Text));

                blinkIcon.SetActive(true);

                yield return StartCoroutine(WaitUntilTouchInput());

                //Play Click SE
                SoundManager.Instance.PlaySE(SoundName.Select);

                itemCount++;
            }

            blinkIcon.SetActive(false);

            yield return StartCoroutine(AdjustImageAlpha(choice, true));

            yield return StartCoroutine(WaitUntilTouchInput());

            yield return StartCoroutine(AdjustImageAlpha(choice, false));

            yield return new WaitForSeconds(waitingOffset);

            yield return StartCoroutine(AdjustImageAlpha(fadePanel2, true));

            yield return new WaitForSeconds(waitingOffset);

            yield return StartCoroutine(AdjustImageAlpha(endingScreen, true));

            yield return new WaitForSeconds(waitingOffset);

            yield return StartCoroutine(AdjustImageAlpha(endingImage, true));

            yield return new WaitForSeconds(waitingOffset);

            for (int i = 0; i < endingText.Length; i++)
            {
                yield return StartCoroutine(TextPrintAnimation(endingText[i], endingString[i]));

                yield return new WaitForSeconds(waitingOffset);
            }

            yield return StartCoroutine(WaitUntilTouchInput());

            yield return new WaitForSeconds(waitingOffset);

            fadeTime = 5f;

            yield return StartCoroutine(AdjustImageAlpha(fadePanel3, true));

            PlayerPrefs.DeleteAll();

            GameManager.Instance.CurrentDayCycle = DayCycle.Start;
        }


        IEnumerator TextPrintAnimation(TMP_Text textComp,string text)
        {
            int count = 0;
            int textLength = text.Length;

            textComp.SetText("");

            while (count != textLength)
            {
                textComp.text += text[count].ToString();

                //색상 추가
                if (text[count].ToString() == "<")
                {
                    while (text[count].ToString() != ">")
                    {
                        count++;
                        textComp.text += text[count].ToString();
                    }
                }

                count++;
                yield return new WaitForSeconds(0.1f);
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


        #region ALPHACOROUTINE
        IEnumerator AdjustImageAlpha(Image image, bool isIncreasingAlpha)
        {
            float time = 0;
            float alpha;

            float r = image.color.r;
            float g = image.color.g;
            float b = image.color.b;

            while (time <= fadeTime)
            {
                if (isIncreasingAlpha)
                    alpha = time / fadeTime;
                else
                    alpha = (fadeTime - time) / fadeTime;

                image.color = new Color(r, g, b, alpha);

                time += Time.deltaTime;
                yield return null;
            }

            if (isIncreasingAlpha)
                image.color = new Color(r, g, b, 1);
            else
                image.color = new Color(r, g, b, 0);

            yield return null;
        }

        IEnumerator AdjustTextAlpha(TMP_Text text, bool isIncreasingAlpha)
        {
            float time = 0;
            float alpha;

            float r = text.color.r;
            float g = text.color.g;
            float b = text.color.b;

            while (time <= fadeTime)
            {
                if (isIncreasingAlpha)
                    alpha = time / fadeTime;
                else
                    alpha = (fadeTime - time) / fadeTime;

                text.color = new Color(r, g, b, alpha);

                time += Time.deltaTime;
                yield return null;
            }

            if (isIncreasingAlpha)
                text.color = new Color(r, g, b, 1);
            else
                text.color = new Color(r, g, b, 0);

            yield return null;
        }
        #endregion
    }
}