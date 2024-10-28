using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HandByHand.NightSystem.SignLanguageSystem;
using UnityEditor.U2D.Aseprite;

#if UNITY_EDITOR
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;
#endif


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
        private GameObject dialogueCanvas;

        private GameObject instantiatePanel;
        private GameObject dialoguePanel;
        #endregion

        #region VARIABLE
        [Header("UIVariableField")]

        public float PanelUpperMovingOffset;

        public bool IsPrintEnd {  get; private set; }

        #endregion

        #region INIT

        private void Awake()
        {
            IsPrintEnd = false;
            instantiatePanel = dialogueCanvas.transform.Find("InstantiatePanel").gameObject;
            dialoguePanel = dialogueCanvas.transform.Find("DialoguePanel").gameObject;
        }

        void Start()
        {
            float panelHeight = instantiatePanel.GetComponent<RectTransform>().rect.height;
            instantiatePanel.transform.position -= new Vector3(0, panelHeight, 0);
        }

        #endregion

        public void StartPrint(DialogueItem dialogueItem)
        {
            //�÷��̾� �ؽ�Ʈ�� �ö���� ���
            if (dialogueItem.ItemType != ItemType.PlayerChoice)
            {
                //������Ʈ ������ Instantiate
                GameObject instance = InstantiateDialogueObject(dialogueItem.whoseItem);

                //������Ʈ ���� ��ġ ����
                MoveDialogueObjectHorizontalPosition(instance, dialogueItem.whoseItem);

                //Text ����
                SetTextContent(instance, dialogueItem);

                //�г��� �о�ø��� �ִϸ��̼�
                float movingHeightOfDialoguePanel = PanelUpperMovingOffset + instance.GetComponent<RectTransform>().rect.height;
                StartCoroutine(DialoguePanelUpperSlidingAnimationCoroutine(movingHeightOfDialoguePanel));
            }

            //�÷��̾� �������� �ö���� ���
            //else if(dialogueItem.ItemType == ItemType.PlayerChoice)
            else
            {
                //������ ����Ʈ�� �޾ƿ���
                List<SignLanguageSO> choiceList = new List<SignLanguageSO>( ( (PlayerChoice)dialogueItem ).SignLanguageItem);

                //������Ʈ ������ Instantiate
                float objectIntervalOffset = 20f;
                List<GameObject> choiceObjectList = InstantiateChoiceObject(choiceList.Count, objectIntervalOffset, out float dialoguePanelMovingHeight);

                //������Ʈ ���� ��ġ ����
                MoveChoiceObjectHorizontalPosition(ref choiceObjectList);

                //��ư�� ������ ����
                SetChoiceContent(ref choiceObjectList, ref choiceList);

                //�������� ������ŭ �ø�
                StartCoroutine(DialoguePanelUpperSlidingAnimationCoroutine(dialoguePanelMovingHeight));
            }
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

        private void MoveDialogueObjectHorizontalPosition(GameObject instance, WhoseItem whoseItem)
        {
            float halfWidth = (instance.GetComponent<RectTransform>().rect.width) * 0.5f;
            //NPC ������Ʈ�� ��Ŀ�� �����̹Ƿ� ���������� �ݸ�ŭ �о��ֱ�
            if (whoseItem == WhoseItem.NPC)
            {
                instance.transform.position += new Vector3(halfWidth, 0, 0);
            }
            //�÷��̾� ������Ʈ�� ��Ŀ�� �������̹Ƿ� �������� �ݸ�ŭ �о��ֱ�
            else
            {
                instance.transform.position -= new Vector3(halfWidth, 0, 0);
            }
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

        #region CHOICEPRINTFUNCTION
        private List<GameObject> InstantiateChoiceObject(int objectCount, float objectIntervalOffset, out float dialoguePanelMovingHeight)
        {
            #region VARIABLEFIELD
            instancePrefab = PlayerChoicePrefab;

            List<GameObject> choiceObjectList = new List<GameObject>();

            float movingHeightOfInstantiatePanel = objectIntervalOffset + PlayerChoicePrefab.GetComponent<RectTransform>().rect.height;

            dialoguePanelMovingHeight = 0;

            Vector3 originalPositionOfinstantiatePanel = instancePrefab.transform.position;
            #endregion

            for(int i = 0; i < objectCount; i++)
            {
                //������Ʈ ������ Instantiate
                GameObject instance = Instantiate(instancePrefab, instantiatePanel.transform);
                instantiatedPrefab.Add(instance);
                choiceObjectList.Add(instance);
                instance.transform.SetParent(dialoguePanel.transform, true);

                instantiatePanel.transform.position -= new Vector3(0, movingHeightOfInstantiatePanel, 0);
                dialoguePanelMovingHeight += movingHeightOfInstantiatePanel;
            }

            instantiatePanel.transform.position = originalPositionOfinstantiatePanel;

            return choiceObjectList;
        }

        private void MoveChoiceObjectHorizontalPosition(ref List<GameObject> choiceObjectList)
        {
            float halfWidth = (choiceObjectList[0].GetComponent<RectTransform>().rect.width) * 0.5f;
            //�÷��̾� ������Ʈ�� ��Ŀ�� �������̹Ƿ� �������� �ݸ�ŭ �о��ֱ�

            for(int i = 0; i < choiceObjectList.Count; i++)
            {
                choiceObjectList[i].transform.position -= new Vector3(halfWidth, 0, 0);
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

        #region PANELANIMATION

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

        #endregion

        IEnumerator AnnouncePrintDone()
        {
            IsPrintEnd = true;
            yield return null;

            IsPrintEnd = false;
            yield return null;
        }
    }
}