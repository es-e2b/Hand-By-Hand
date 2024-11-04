using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using HandByHand.NightSystem.SignLanguageSystem;
using UnityEngine.UI;


//��ȭ�� ����ϴ� �Ŵ����Դϴ�.
namespace HandByHand.NightSystem.DialogueSystem
{
    public class PrintManager : MonoBehaviour
    {
        #region PREFABFIELD

        [Header("PrefabField")]
        public GameObject PlayerDialoguePrefab;
        public GameObject NPCDialoguePrefab;
        public GameObject PlayerChoicePrefab;

        private GameObject instancePrefab;

        private List<GameObject> instantiatedPrefab = new List<GameObject>();
        #endregion

        #region UICOMPONENTS
        [Header("UIComponentField")]

        [SerializeField]
        private GameObject signLanguageCanvas;

        [SerializeField]
        private GameObject dialogueCanvas;

        private GameObject instantiatePanel;
        public GameObject dialoguePanel;

        public GameObject ScrollView;

        public TMP_Text NPCText;
        public PlayerChoiceButtonComponentList PlayerChoiceButtonComponentList;
        private List<Button> playerChoiceButtonComponentList;
        private List<TMP_Text> playerChoiceTextComponentList;
        #endregion

        #region VARIABLE
        [Header("VariableField")]

        public float ChoiceObjectAnimationTime = 1f;

        private int poolingCount;

        public float PanelUpperMovingOffset;

        public float textPrintDelay = 0.1f;

        public bool IsPrintEnd { get; private set; }

        //DialogueChoiceSelectManager���� ������ ������Ʈ�� �޾ƿ��� ���� ����
        [HideInInspector]
        public List<GameObject> PooledChoiceObjectList;

        //�������� ��� ���� �� �г��� �ø� ���̸� ��Ƶ� ��
        private float movedPanelHeightOfChoice = 0;

        DialogueChoiceSelectManager dialogueChoiceSelectManager;

        private VerticalLayoutGroup playerChoiceVerticalLayoutGroup;
        #endregion

        #region INIT

        private void Awake()
        {
            if (!signLanguageCanvas.activeSelf)
                signLanguageCanvas.SetActive(true);

            playerChoiceVerticalLayoutGroup = PlayerChoiceButtonComponentList.GetComponent<VerticalLayoutGroup>();

            IsPrintEnd = false;
            instantiatePanel = dialogueCanvas.transform.Find("InstantiatePanel").gameObject;
            dialogueChoiceSelectManager = transform.parent.Find("DialogueChoiceSelectManager").GetComponent<DialogueChoiceSelectManager>();
        }

        void Start()
        {
            playerChoiceButtonComponentList = PlayerChoiceButtonComponentList.ButtonComponentList;
            playerChoiceTextComponentList = PlayerChoiceButtonComponentList.TextComponentList;

            float panelHeight = instantiatePanel.GetComponent<RectTransform>().rect.height;
            instantiatePanel.transform.position -= new Vector3(0, panelHeight, 0);
        }

        #endregion


        public void StartPrint(DialogueItem dialogueItem)
        {
            switch (dialogueItem.itemType)
            {
                case ItemType.NPCText:
                    NPCText.text = "";
                    StartCoroutine(TextPrintAnimation(((NPCText)dialogueItem).Text));
                    break;

                case ItemType.PlayerChoice:
                    poolingCount = ((PlayerChoice)dialogueItem).SignLanguageItem.Count;
                    PoolingChoiceObject(((PlayerChoice)dialogueItem));
                    break;

                case ItemType.PlayerText:
                    break;
            }
        }

        /*
        public void StartPrint(DialogueItem dialogueItem)
        {
            //�ؽ�Ʈ�� �ö���� ���
            if (dialogueItem.itemType != ItemType.PlayerChoice)
            {
                //������Ʈ ������ Instantiate
                GameObject instance = InstantiateDialogueObject(dialogueItem.whoseItem);

                //Text ����
                SetTextContent(instance, dialogueItem);

                //�г��� �о�ø��� �ִϸ��̼�
                float movingHeightOfDialoguePanel = PanelUpperMovingOffset + instance.GetComponent<RectTransform>().rect.height;
                StartCoroutine(DialoguePanelUpperSlidingAnimationCoroutine(movingHeightOfDialoguePanel));
            }

            //�÷��̾� �������� �ö���� ���
            //else if(dialogueItem.itemType == ItemType.PlayerChoice)
            else
            {
                //������ ����Ʈ�� �޾ƿ���
                List<SignLanguageSO> choiceList = new List<SignLanguageSO>(((PlayerChoice)dialogueItem).SignLanguageItem);

                //������Ʈ ������ Instantiate
                float objectIntervalOffset = 25f;
                List<GameObject> choiceObjectList = InstantiateChoiceObject(choiceList.Count, objectIntervalOffset, out float movedPanelHeightOfChoice);
                ChoiceObjectList = new List<GameObject>(choiceObjectList);
                this.movedPanelHeightOfChoice = movedPanelHeightOfChoice;

                //��ư�� ������ ����
                SetChoiceContent(ref choiceObjectList, ref choiceList);

                //�������� ������ŭ �ø�
                StartCoroutine(DialoguePanelUpperSlidingAnimationCoroutine(movedPanelHeightOfChoice));
            }
        } */

        public void AdjustPanelHeightAfterSelectChoice()
        {
            PlayerText playerText = new PlayerText();

            playerText.Text = dialogueChoiceSelectManager.GetSelectedSignLanguageSO().Mean;

            //������ ������Ʈ ����
            for (int i = 0; i < PooledChoiceObjectList.Count; i++)
            {
                Destroy(PooledChoiceObjectList[i]);
            }
            //����Ʈ �ʱ�ȭ
            PooledChoiceObjectList.Clear();

            //�г� �Ʒ��� ������
            dialoguePanel.transform.position -= new Vector3(0, movedPanelHeightOfChoice, 0);
            //reset variable
            movedPanelHeightOfChoice = 0;


            //�ؽ�Ʈ ����
            //������Ʈ ������ Instantiate
            GameObject instance = InstantiateDialogueObject(playerText.whoseItem);

            //Text ����
            SetTextContent(instance, playerText);

            float movingHeightOfDialoguePanel = PanelUpperMovingOffset + instance.GetComponent<RectTransform>().rect.height;
            dialoguePanel.transform.position += new Vector3(0, movingHeightOfDialoguePanel, 0);
        }

        #region DIALOGUEPRINTFUNCTION
        private GameObject InstantiateDialogueObject(WhoseItem whoseItem)
        {
            switch (whoseItem)
            {
                case WhoseItem.NPC:
                    instancePrefab = NPCDialoguePrefab;
                    break;
                case WhoseItem.Player:
                    instancePrefab = PlayerDialoguePrefab;
                    break;
            }

            //������Ʈ ������ Instantiate
            GameObject instance = Instantiate(instancePrefab, instantiatePanel.transform);
            instantiatedPrefab.Add(instance);
            instance.transform.SetParent(dialoguePanel.transform, true);

            return instance;
        }

        private void SetTextContent(GameObject instance, DialogueItem dialogueItem)
        {
            TMP_Text textComponent = instance.transform.GetChild(0).GetComponent<TMP_Text>();

            //������ Ÿ�Կ� ���� �ٿ�ĳ�����Ͽ� �ؽ�Ʈ ����
            if (dialogueItem.itemType == ItemType.PlayerText)
            {
                textComponent.SetText(((PlayerText)dialogueItem).Text);
            }
            else if (dialogueItem.itemType == ItemType.NPCText)
            {
                textComponent.SetText(((NPCText)dialogueItem).Text);
            }

        }

        #endregion

        #region PLAYERCHOICEFUNCTION
        private List<GameObject> InstantiateChoiceObject(int objectCount, float objectIntervalOffset, out float movedPanelHeightOfChoice)
        {
            #region VARIABLEFIELD
            instancePrefab = PlayerChoicePrefab;

            List<GameObject> choiceObjectList = new List<GameObject>();

            float movingHeightOfInstantiatePanel = objectIntervalOffset + PlayerChoicePrefab.GetComponent<RectTransform>().rect.height;

            movedPanelHeightOfChoice = 0;

            Vector3 originalPositionOfinstantiatePanel = instantiatePanel.transform.position;
            #endregion

            for (int i = 0; i < objectCount; i++)
            {
                //������Ʈ ������ Instantiate
                GameObject instance = Instantiate(instancePrefab, instantiatePanel.transform);
                instantiatedPrefab.Add(instance);
                choiceObjectList.Add(instance);
                instance.transform.SetParent(dialoguePanel.transform, true);

                instantiatePanel.transform.localPosition -= new Vector3(0, movingHeightOfInstantiatePanel, 0);
                movedPanelHeightOfChoice += movingHeightOfInstantiatePanel;
            }

            movedPanelHeightOfChoice += PanelUpperMovingOffset;
            instantiatePanel.transform.position = originalPositionOfinstantiatePanel;

            return choiceObjectList;
        }

        private void PoolingChoiceObject(PlayerChoice playerChoiceItem)
        {
            List<GameObject> objectList = new List<GameObject>();
            List<SignLanguageSO> signLanguageList = new List<SignLanguageSO>(playerChoiceItem.SignLanguageItem);

            //Object pooling
            for (int i = 0; i < poolingCount; i++)
            {
                playerChoiceButtonComponentList[i].gameObject.SetActive(true);
                objectList.Add(playerChoiceButtonComponentList[i].gameObject);
            }

            PooledChoiceObjectList = objectList;

            SetChoiceContent(ref PooledChoiceObjectList, ref signLanguageList);

            StartCoroutine(ChoiceObjectFadeAnimation("on"));
        }

        public void ReturnChoiceObject()
        {
            StartCoroutine(ChoiceObjectFadeAnimation("off"));

            float offset = 0.1f;
            Invoke("ObjectSetActiveFalse", ChoiceObjectAnimationTime + offset);
        }

        void ObjectSetActiveFalse()
        {
            for (int i = 0; i < playerChoiceButtonComponentList.Count; i++)
            {
                playerChoiceButtonComponentList[i].gameObject.SetActive(false);
            }
        }

        private void SetChoiceContent(ref List<GameObject> choiceObjectList, ref List<SignLanguageSO> choiceList)
        {
            #region ERRORCHECK
            if (choiceObjectList.Count != choiceList.Count)
            {
                Debug.LogError("��ũ��Ʈ ���� in PrintManager.cs");
            }
            #endregion

            for (int i = 0; i < choiceObjectList.Count; i++)
            {
                //choice ������Ʈ�� SO�Ҵ�
                choiceObjectList[i].GetComponent<ChoiceInformation>().signLanguageSO = choiceList[i];

                //�ؽ�Ʈ �Ҵ�
                TMP_Text textComponent = choiceObjectList[i].transform.GetChild(0).GetComponent<TMP_Text>();

                textComponent.SetText(choiceList[i].Mean);
            }
        }
        #endregion

        #region COROUTINE

        IEnumerator DialoguePanelUpperSlidingAnimationCoroutine(float VerticalMovingHeight)
        {
            //���� ����
            #region VARIABLEFIELD
            //float dialoguePanelHeight = dialoguePanel.GetComponent<RectTransform>().rect.height;
            Vector3 targetPosition = dialoguePanel.transform.position + new Vector3(0, VerticalMovingHeight, 0);

            float dialoguePanelX = dialoguePanel.transform.position.x;
            float dialoguePanelZ = dialoguePanel.transform.position.z;

            float velocityY = 0f;
            float smoothTime = 0.3f;

            float offset = 0.1f;
            #endregion

            //�г��� ���� �ε巴�� �ø���
            while (targetPosition.y - offset > dialoguePanel.transform.position.y)
            {
                float positionY = Mathf.SmoothDamp(dialoguePanel.transform.position.y, targetPosition.y, ref velocityY, smoothTime);
                dialoguePanel.transform.position = new Vector3(dialoguePanelX, positionY, dialoguePanelZ);

                yield return null;
            }

            dialoguePanel.transform.position = targetPosition;

            //��ȭ�� �б� ���� �ణ�� �ð��� ��ٷ��ش�
            float playerReadWaitingTime = 1.0f;
            yield return new WaitForSeconds(playerReadWaitingTime);

            //�ִϸ��̼� �ϷḦ �˸�
            yield return StartCoroutine(AnnouncePrintDone());
        }

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

            while (ChoiceObjectAnimationTime > time)
            {
                switch (type)
                {
                    case "on":
                        alpha = time / ChoiceObjectAnimationTime;
                        break;

                    case "off":
                        alpha = (ChoiceObjectAnimationTime - time) / ChoiceObjectAnimationTime;
                        break;
                }

                for (int i = 0; i < poolingCount; i++)
                {
                    ColorBlock colorBlock = playerChoiceButtonComponentList[i].colors;
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
                        ColorBlock colorBlock = playerChoiceButtonComponentList[i].colors;
                        colorBlock.normalColor = new Color(1f, 1f, 1f, 1f);
                        playerChoiceButtonComponentList[i].colors = colorBlock;
                    }
                    break;

                case "off":
                    for (int i = 0; i < poolingCount; i++)
                    {
                        ColorBlock colorBlock = playerChoiceButtonComponentList[i].colors;
                        colorBlock.normalColor = new Color(1f, 1f, 1f, 0);
                        playerChoiceButtonComponentList[i].colors = colorBlock;
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