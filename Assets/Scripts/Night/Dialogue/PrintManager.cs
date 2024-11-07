using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using HandByHand.NightSystem.SignLanguageSystem;
using UnityEngine.UI;


namespace HandByHand.NightSystem.DialogueSystem
{
    public class PrintManager : MonoBehaviour
    {
        #region UICOMPONENTS
        [Header("UIComponentField")]

        [SerializeField]
        private GameObject signLanguageCanvas;

        [SerializeField]
        private GameObject dialogueCanvas;

        public TMP_Text NPCText;

        public TMP_Text NPCName;

        public PlayerChoiceButtonComponentList PlayerChoiceButtonComponentList;
        private List<Button> playerChoiceButtonComponentList;
        private List<TMP_Text> playerChoiceTextComponentList;
        #endregion

        #region VARIABLE
        [Header("VariableField")]

        public float ChoiceObjectAnimationTime = 1f;

        private int poolingCount;

        public float textPrintDelay = 0.1f;

        public bool IsPrintEnd { get; private set; } = false;

        [HideInInspector]
        public List<GameObject> PooledChoiceObjectList;

        private DialogueChoiceSelectManager dialogueChoiceSelectManager;
        #endregion

        #region INIT

        private void Awake()
        {
            if (!signLanguageCanvas.activeSelf)
                signLanguageCanvas.SetActive(true);

            IsPrintEnd = false;
            dialogueChoiceSelectManager = transform.parent.Find("DialogueChoiceSelectManager").GetComponent<DialogueChoiceSelectManager>();
        }

        void Start()
        {
            playerChoiceButtonComponentList = PlayerChoiceButtonComponentList.ButtonComponentList;
            playerChoiceTextComponentList = PlayerChoiceButtonComponentList.TextComponentList;
        }
        #endregion


        public void StartPrint(DialogueItem dialogueItem)
        {
            switch (dialogueItem.itemType)
            {
                case ItemType.NPCText:
                    NPCText.text = "";

                    InitNPCName( ( ( NPCText ) dialogueItem ).Name );
                    StartCoroutine(TextPrintAnimation(((NPCText)dialogueItem).Text));
                    break;

                case ItemType.PlayerChoice:
                    poolingCount = ((PlayerChoice)dialogueItem).ChoiceContentList.Count;
                    PoolingChoiceObject();

                    InitContentOnChoiceObject( ( ( PlayerChoice ) dialogueItem) );
                    StartCoroutine(ChoiceObjectFadeAnimation("on"));
                    break;

                case ItemType.PlayerText:
                    poolingCount = ((PlayerText)dialogueItem).Text.Count;
                    PoolingChoiceObject();

                    SetButtonInteractable(false);

                    InitContentOnChoiceObject( ( ( PlayerText ) dialogueItem ) );
                    StartCoroutine(ChoiceObjectFadeAnimation("on"));
                    break;
            }
        }

        #region NPCTEXTFUNCTION

        private void InitNPCName(string name)
        {
            NPCName.SetText(name);
        }

        #endregion

        #region PLAYERCHOICEFUNCTION
        private void PoolingChoiceObject()
        {
            List<GameObject> objectList = new List<GameObject>();

            //Object pooling
            for (int i = 0; i < poolingCount; i++)
            {
                playerChoiceButtonComponentList[i].gameObject.SetActive(true);
                objectList.Add(playerChoiceButtonComponentList[i].gameObject);
            }

            PooledChoiceObjectList = objectList;
        }

        #region RETURNFUNCTION
        public void ReturnChoiceObject()
        {
            StartCoroutine(ChoiceObjectFadeAnimation("off"));
        }

        void ObjectSetActiveFalse()
        {
            for (int i = 0; i < PooledChoiceObjectList.Count; i++)
            {
                PooledChoiceObjectList[i].gameObject.SetActive(false);
            }
        }
        #endregion

        private void InitContentOnChoiceObject(PlayerChoice playerChoiceItem)
        {
            for (int i = 0; i < PooledChoiceObjectList.Count; i++)
            {
                //choice ������Ʈ�� SO�Ҵ�
                PooledChoiceObjectList[i].GetComponent<ChoiceInformation>().signLanguageSO = playerChoiceItem.ChoiceContentList[i].SignLanguageItem;

                TMP_Text textComponent = PooledChoiceObjectList[i].transform.GetChild(0).GetComponent<TMP_Text>();

                textComponent.SetText(playerChoiceItem.ChoiceContentList[i].ChoiceText);
            }
        }
        #endregion

        #region PLAYERTEXTFUNCTION

        private void SetButtonInteractable(bool type)
        {
            for (int i = 0; i < PooledChoiceObjectList.Count; i++)
            {
                PooledChoiceObjectList[i].GetComponent<Button>().interactable = type;
            }
        }

        private void InitContentOnChoiceObject(PlayerText playerTextItem)
        {
            for (int i = 0; i < PooledChoiceObjectList.Count; i++)
            {
                TMP_Text textComponent = PooledChoiceObjectList[i].transform.GetChild(0).GetComponent<TMP_Text>();

                textComponent.SetText(playerTextItem.Text[i]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offsetTime">텍스트를 띄워놓는 시간</param>
        private void ReturnChoiceObject(float offsetTime)
        {
            Invoke("ObjectSetActiveFalse", offsetTime);
        }

        #endregion

        #region COROUTINE

        IEnumerator TextPrintAnimation(string text)
        {
            int count = 0;
            int textLength = text.Length;

            while (count != textLength)
            {
                NPCText.text += text[count].ToString();
                count++;
                yield return new WaitForSeconds(textPrintDelay);
            }

            yield return StartCoroutine(AnnouncePrintDone());
        }

        /// <summary>
        /// 플레이어 선택지 오브젝트 풀링시 애니메이션
        /// </summary>
        /// <param name="poolingCount">buttonComponentList의 오브젝트 풀링 수</param>
        /// <param name="type">오브젝트 풀링시 : on, 반환시 : off</param>
        /// <returns>yield return StartCoroutine(AnnouncePrintDone());</returns>
        IEnumerator ChoiceObjectFadeAnimation(string type)
        {
            float time = 0;
            float alpha = 0;

            ColorBlock colorBlock = playerChoiceButtonComponentList[0].colors;
            float previewAlpha = colorBlock.normalColor.a;

            bool condition = true;

            while (condition)
            {
                switch (type)
                {
                    case "on":
                        alpha = time / ChoiceObjectAnimationTime + previewAlpha;
                        condition = alpha < 1;
                        break;

                    case "off":
                        alpha = previewAlpha - ( time / ChoiceObjectAnimationTime );
                        condition = alpha > 0;
                        break;
                }

                for (int i = 0; i < poolingCount; i++)
                {
                    colorBlock = playerChoiceButtonComponentList[i].colors;
                    colorBlock.normalColor = new Color(1f, 1f, 1f, alpha);
                    playerChoiceButtonComponentList[i].colors = colorBlock;
                }
                time += Time.deltaTime;

                yield return null;
            }

            switch (type)
            {
                case "on":
                    for (int i = 0; i < poolingCount; i++)
                    {
                        colorBlock = playerChoiceButtonComponentList[i].colors;
                        colorBlock.normalColor = new Color(1f, 1f, 1f, 1f);
                        playerChoiceButtonComponentList[i].colors = colorBlock;
                    }
                    break;

                case "off":
                    for (int i = 0; i < poolingCount; i++)
                    {
                        colorBlock = playerChoiceButtonComponentList[i].colors;
                        colorBlock.normalColor = new Color(1f, 1f, 1f, 0);
                        playerChoiceButtonComponentList[i].colors = colorBlock;
                    }

                    //Object Reset
                    poolingCount = 0;

                    SetButtonInteractable(true);

                    for (int i = 0; i < PooledChoiceObjectList.Count; i++)
                    {
                        PooledChoiceObjectList[i].gameObject.SetActive(false);
                    }
                    break;
            }

            yield return StartCoroutine(AnnouncePrintDone());
        }

        IEnumerator AnnouncePrintDone()
        {
            IsPrintEnd = true;
            yield return null;

            IsPrintEnd = false;
            yield return null;
        }

        #endregion
    }
}