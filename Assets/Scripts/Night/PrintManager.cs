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


//대화를 출력하는 매니저입니다.
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
            //오브젝트 프리팹 Instantiate
            GameObject instance = ObjectInstantiate(dialogueItem.whoseItem);
            //Text 설정
            SetTextContent(instance, dialogueItem);

            StartCoroutine(DialogueUpperSlidingAnimationCoroutine());
        }

        private void SetTextContent(GameObject instance, DialogueItem dialogueItem)
        {
            TMP_Text textComponent = instance.transform.GetChild(0).GetComponent<TMP_Text>();

            //아이템 타입에 따라 다운캐스팅하여 텍스트 설정
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
            //변수 선언
            #region VARIABLEFIELD
            float dialoguePanelHeight = dialoguePanel.GetComponent<RectTransform>().rect.height;
            Vector3 targetPosition = dialoguePanel.transform.position + new Vector3(0, dialoguePanelHeight, 0);

            float dialoguePanelX = dialoguePanel.transform.position.x;
            float dialoguePanelZ = dialoguePanel.transform.position.z;

            float velocityY = 0f;
            float smoothTime = 0.3f;

            float offset = 0.1f;
            #endregion

            //대화창 패널을 패널의 세로 길이만큼 위로 이동(대화 로그가 전부 위로 이동)
            while (targetPosition.y - offset > dialoguePanel.transform.position.y)
            {
                float positionY = Mathf.SmoothDamp(dialoguePanel.transform.position.y, targetPosition.y, ref velocityY, smoothTime);
                dialoguePanel.transform.position = new Vector3(dialoguePanelX, positionY, dialoguePanelZ);

                yield return null;
            }

            //대화를 읽기 위한 약간의 시간을 기다려줌
            float playerReadWaitingTime = 1.0f;
            yield return new WaitForSeconds(playerReadWaitingTime);

            dialoguePanel.transform.position = targetPosition;

            yield return StartCoroutine(AnnouncePrintDone());
        }

        /// <summary>
        /// instancePrefab 게임 오브젝트 생성 후 반환 함수
        /// </summary>
        /// <param name="whoseItem">오브젝트가 누구 것인지에 따라 오른쪽 또는 왼쪽에 배치</param>
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

            //오브젝트 프리팹 Instantiate
            GameObject instance = Instantiate(instancePrefab, instantiatePanel.transform);
            instantiatedPrefab.Add(instance);
            instance.transform.SetParent(dialoguePanel.transform, true);

            ///
            /// 오브젝트 위치 조정
            ///
            float halfWidth = (instance.GetComponent<RectTransform>().rect.width) * 0.5f;
            //NPC 오브젝트는 앵커가 왼쪽이므로 오른쪽으로 반만큼 밀어주기
            if (whoseItem == WhoseItem.NPC)
            {
                instance.transform.position += new Vector3(halfWidth, 0, 0);
            }
            //플레이어 오브젝트는 앵커가 오른쪽이므로 왼쪽으로 반만큼 밀어주기
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