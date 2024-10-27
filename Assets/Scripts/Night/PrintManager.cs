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
        [Header("InstantiatePrefab")]
        public GameObject PlayerDialoguePrefab;
        public GameObject NPCDialoguePrefab;
        public GameObject PlayerChoicePrefab;

        private GameObject instancePrefab;

        private List<GameObject> instantiatedPrefab = new List<GameObject>();
        #endregion

        #region UICOMPONENTS
        [SerializeField]
        private GameObject dialogueCanvas;

        private GameObject instantiatePanel;
        private GameObject dialoguePanel;
        #endregion

        [HideInInspector]
        public bool IsPrintEnd = false;

        private void Awake()
        {
            instantiatePanel = dialogueCanvas.transform.Find("InstantiatePanel").gameObject;
            dialoguePanel = dialogueCanvas.transform.Find("DialoguePanel").gameObject;
        }

        void Start()
        {
            float panelHeight = instantiatePanel.GetComponent<RectTransform>().rect.height;
            instantiatePanel.transform.position -= new Vector3(0, panelHeight, 0);
        }

        public void StartPrint(DialogueItem dialogueItem)
        {
            if (dialogueItem.ItemType != ItemType.PlayerChoice)
            {
                //������Ʈ ������ Instantiate
                GameObject instance = DialogueObjectInstantiate(dialogueItem.whoseItem);
                //Text ����
                SetTextContent(instance, dialogueItem);
                //�г��� �о�ø��� �ִϸ��̼�
                StartCoroutine(DialogueObjectUpperSlidingAnimationCoroutine());
            }
            else if(dialogueItem.ItemType == ItemType.PlayerChoice)
            {
                List<SignLanguageSO> choiceList = new List<SignLanguageSO>( ( (PlayerChoice)dialogueItem ).SignLanguageItem);
                //������Ʈ ������ Instantiate

                //��ư�� ������ ����

                //�������� ������ŭ �ø�
                
            }
        }

        #region DIALOGUEPRINTFUNCTION
        private GameObject DialogueObjectInstantiate(WhoseItem whoseItem)
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

            ///
            /// ������Ʈ ��ġ ����
            ///
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

        IEnumerator DialogueObjectUpperSlidingAnimationCoroutine()
        {
            //���� ����
            #region VARIABLEFIELD
            float dialoguePanelHeight = dialoguePanel.GetComponent<RectTransform>().rect.height;
            Vector3 targetPosition = dialoguePanel.transform.position + new Vector3(0, dialoguePanelHeight, 0);

            float dialoguePanelX = dialoguePanel.transform.position.x;
            float dialoguePanelZ = dialoguePanel.transform.position.z;

            float velocityY = 0f;
            float smoothTime = 0.3f;

            float offset = 0.1f;
            #endregion

            //��ȭâ �г��� �г��� ���� ���̸�ŭ ���� �̵�(��ȭ �αװ� ���� ���� �̵�)
            while (targetPosition.y - offset > dialoguePanel.transform.position.y)
            {
                float positionY = Mathf.SmoothDamp(dialoguePanel.transform.position.y, targetPosition.y, ref velocityY, smoothTime);
                dialoguePanel.transform.position = new Vector3(dialoguePanelX, positionY, dialoguePanelZ);

                yield return null;
            }

            //��ȭ�� �б� ���� �ణ�� �ð��� ��ٷ���
            float playerReadWaitingTime = 1.0f;
            yield return new WaitForSeconds(playerReadWaitingTime);

            dialoguePanel.transform.position = targetPosition;

            yield return StartCoroutine(AnnouncePrintDone());
        }
        #endregion

        #region CHOICEPRINTFUNCTION

        private List<GameObject> ChoiceObjectInstantiate()
        {
            instancePrefab = PlayerChoicePrefab;

            //������Ʈ ������ Instantiate
            GameObject instance = Instantiate(instancePrefab, instantiatePanel.transform);
            instantiatedPrefab.Add(instance);
            instance.transform.SetParent(dialoguePanel.transform, true);

            ///
            /// ������Ʈ ��ġ ����
            ///
            float halfWidth = (instance.GetComponent<RectTransform>().rect.width) * 0.5f;
            instance.transform.position -= new Vector3(halfWidth, 0, 0);


        }

        private void SetChoiceContent()
        {

        }

        IEnumerator ChoiceObjectUpperSlidingAnimationCoroutine()
        {
            yield return null;
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