using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using HandByHand.NightSystem.SignLanguageSystem;
using UnityEditor.U2D.Aseprite;
using Unity.VisualScripting;


#if UNITY_EDITOR
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;
#endif


//대화를 출력하는 매니저입니다.
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

        public GameObject ScrollView;
        #endregion

        #region VARIABLE
        [Header("VariableField")]

        public float PanelUpperMovingOffset;

        public bool IsPrintEnd { get; private set; }

        //DialogueChoiceSelectManager에서 선택지 오브젝트를 받아오기 위한 변수
        [HideInInspector]
        public List<GameObject> ChoiceObjectList;

        //선택지를 출력 했을 때 패널을 올린 높이를 담아둘 값
        private float movedPanelHeightOfChoice = 0;

        DialogueChoiceSelectManager dialogueChoiceSelectManager;
        #endregion

        #region INIT

        private void Awake()
        {
            IsPrintEnd = false;
            instantiatePanel = dialogueCanvas.transform.Find("InstantiatePanel").gameObject;
            dialoguePanel = dialogueCanvas.transform.Find("DialoguePanel").gameObject;
            dialogueChoiceSelectManager = transform.parent.Find("DialogueChoiceSelectManager").GetComponent<DialogueChoiceSelectManager>();
        }

        void Start()
        {
            float panelHeight = instantiatePanel.GetComponent<RectTransform>().rect.height;
            instantiatePanel.transform.position -= new Vector3(0, panelHeight, 0);
        }

        #endregion

        public void StartPrint(DialogueItem dialogueItem)
        {
            //텍스트가 올라오는 경우
            if (dialogueItem.itemType != ItemType.PlayerChoice)
            {
                //오브젝트 프리팹 Instantiate
                GameObject instance = InstantiateDialogueObject(dialogueItem.whoseItem);

                //Text 설정
                SetTextContent(instance, dialogueItem);

                //패널을 밀어올리는 애니메이션
                float movingHeightOfDialoguePanel = PanelUpperMovingOffset + instance.GetComponent<RectTransform>().rect.height;
                StartCoroutine(DialoguePanelUpperSlidingAnimationCoroutine(movingHeightOfDialoguePanel));
            }

            //플레이어 선택지가 올라오는 경우
            //else if(dialogueItem.itemType == ItemType.PlayerChoice)
            else
            {
                //선택지 리스트로 받아오기
                List<SignLanguageSO> choiceList = new List<SignLanguageSO>(((PlayerChoice)dialogueItem).SignLanguageItem);

                //오브젝트 프리팹 Instantiate
                float objectIntervalOffset = 25f;
                List<GameObject> choiceObjectList = InstantiateChoiceObject(choiceList.Count, objectIntervalOffset, out float movedPanelHeightOfChoice);
                ChoiceObjectList = new List<GameObject>(choiceObjectList);
                this.movedPanelHeightOfChoice = movedPanelHeightOfChoice;

                //버튼당 컨텐츠 설정
                SetChoiceContent(ref choiceObjectList, ref choiceList);

                //선택지의 개수만큼 올림
                StartCoroutine(DialoguePanelUpperSlidingAnimationCoroutine(movedPanelHeightOfChoice));
            }
        }

        public void AdjustPanelHeightAfterSelectChoice()
        {
            PlayerText playerText = new PlayerText();

            playerText.Text = dialogueChoiceSelectManager.GetSelectedSignLanguageSO().Mean;

            //선택지 오브젝트 삭제
            for (int i = 0; i < ChoiceObjectList.Count; i++)
            {
                Destroy(ChoiceObjectList[i]);
            }
            //리스트 초기화
            ChoiceObjectList.Clear();

            //패널 아래로 내리기
            dialoguePanel.transform.position -= new Vector3(0, movedPanelHeightOfChoice, 0);
            //reset variable
            movedPanelHeightOfChoice = 0;


            //텍스트 생성
            //오브젝트 프리팹 Instantiate
            GameObject instance = InstantiateDialogueObject(playerText.whoseItem);

            //Text 설정
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

            //오브젝트 프리팹 Instantiate
            GameObject instance = Instantiate(instancePrefab, instantiatePanel.transform);
            instantiatedPrefab.Add(instance);
            instance.transform.SetParent(dialoguePanel.transform, true);

            return instance;
        }

        private void SetTextContent(GameObject instance, DialogueItem dialogueItem)
        {
            TMP_Text textComponent = instance.transform.GetChild(0).GetComponent<TMP_Text>();

            //아이템 타입에 따라 다운캐스팅하여 텍스트 설정
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
                //오브젝트 프리팹 Instantiate
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

        private void SetChoiceContent(ref List<GameObject> choiceObjectList, ref List<SignLanguageSO> choiceList)
        {
            #region ERRORCHECK
            if (choiceObjectList.Count != choiceList.Count)
            {
                Debug.LogError("스크립트 오류 in PrintManager.cs");
            }
            #endregion

            for (int i = 0; i < choiceObjectList.Count; i++)
            {
                //choice 오브젝트에 SO할당
                choiceObjectList[i].GetComponent<ChoiceInformation>().signLanguageSO = choiceList[i];

                //텍스트 할당
                TMP_Text textComponent = choiceObjectList[i].transform.GetChild(0).GetComponent<TMP_Text>();

                textComponent.SetText(choiceList[i].Mean);
            }
        }
        #endregion

        #region COROUTINE

        IEnumerator DialoguePanelUpperSlidingAnimationCoroutine(float VerticalMovingHeight)
        {
            //변수 선언
            #region VARIABLEFIELD
            //float dialoguePanelHeight = dialoguePanel.GetComponent<RectTransform>().rect.height;
            Vector3 targetPosition = dialoguePanel.transform.position + new Vector3(0, VerticalMovingHeight, 0);

            float dialoguePanelX = dialoguePanel.transform.position.x;
            float dialoguePanelZ = dialoguePanel.transform.position.z;

            float velocityY = 0f;
            float smoothTime = 0.3f;

            float offset = 0.1f;
            #endregion

            //패널을 위로 부드럽게 올린다
            while (targetPosition.y - offset > dialoguePanel.transform.position.y)
            {
                float positionY = Mathf.SmoothDamp(dialoguePanel.transform.position.y, targetPosition.y, ref velocityY, smoothTime);
                dialoguePanel.transform.position = new Vector3(dialoguePanelX, positionY, dialoguePanelZ);

                yield return null;
            }

            dialoguePanel.transform.position = targetPosition;

            //대화를 읽기 위한 약간의 시간을 기다려준다
            float playerReadWaitingTime = 1.0f;
            yield return new WaitForSeconds(playerReadWaitingTime);

            //애니메이션 완료를 알림
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