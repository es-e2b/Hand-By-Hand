using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;
#endif


//��ȭ�� ����ϴ� �Ŵ����Դϴ�.
namespace HandByHand.NightSystem
{
    public class PrintManager : MonoBehaviour
    {
        #region PREFABFIELD
        public GameObject PlayerDialogiePrefab;
        public GameObject NPCDialogiePrefab;

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
            //������Ʈ ������ Instantiate
            GameObject instance = ObjectInstantiate(dialogueItem.whoseItem);
            //Text ����
            SetTextContent(instance, dialogueItem);

            StartCoroutine(DialogueUpperSlidingAnimationCoroutine());
        }

        private void SetTextContent(GameObject instance, DialogueItem dialogueItem)
        {
            TMP_Text textComponent = instance.transform.GetChild(0).GetComponent<TMP_Text>();

            //������ Ÿ�Կ� ���� �ٿ�ĳ�����Ͽ� �ؽ�Ʈ ����
            if(dialogueItem.itemType == ItemType.PlayerText) 
            {
                textComponent.SetText(((PlayerText)dialogueItem).Text);
            }
            else if(dialogueItem.itemType == ItemType.NPCText)
            {
                textComponent.SetText(((NPCText)dialogueItem).Text);
            }

        }

        IEnumerator DialogueUpperSlidingAnimationCoroutine()
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

        /// <summary>
        /// instancePrefab ���� ������Ʈ ���� �� ��ȯ �Լ�
        /// </summary>
        /// <param name="whoseItem">������Ʈ�� ���� �������� ���� ������ �Ǵ� ���ʿ� ��ġ</param>
        /// <returns>InstancePrefab</returns>
        private GameObject ObjectInstantiate(WhoseItem whoseItem)
        {
            switch(whoseItem)
            {
                case WhoseItem.NPC:
                    instancePrefab = NPCDialogiePrefab;
                    break;
                case WhoseItem.Player:
                    instancePrefab = PlayerDialogiePrefab;
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

        IEnumerator AnnouncePrintDone()
        {
            IsPrintEnd = true;
            yield return null;

            IsPrintEnd = false;
            yield return null;
        }
    }
}