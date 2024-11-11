using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using HandByHand.NightSystem.SignLanguageSystem;
using Assets.Scripts.SignLanguage;
using HandByHand.SoundSystem;

namespace HandByHand.NightSystem.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public DialogueFileSO DialogueFileSO;

        //수화 선택창에서 Vocabulary 팝업을 닫았을 시 Swipe기능을 on
        [HideInInspector]
        public bool IsSwipeEnable { get; private set; } = false;

        [Header("UI Components")]
        public GameObject DialogueSpeaker;

        public GameObject PopupSpeaker;

        public GameObject PopupObject;

        public GameObject BlinkIcon;

        private GetInput getInput;

        float waitingTimeOffset = 1.5f;

        public Coroutine DialogueCoroutine;

        [SerializeField]
        private GameObject ConnectionCanvasObject;

        [SerializeField]
        private GameObject EndingCanvasObject;

        #region MANAGERCOMPONENT
        [Header("ManagerComponents")]
        [SerializeField]
        private SignLanguageUIManager signLanguageUIManager;

        [SerializeField]
        private PrintManager printManager;

        [SerializeField]
        private SignLanguageManager signLanguageManager;

        [SerializeField]
        private DialogueChoiceSelectManager dialogueChoiceSelectManager;
        #endregion

        #region INIT
        void Awake()
        {
            if (EndingCanvasObject.activeSelf)
                EndingCanvasObject.SetActive(false);

            if (ConnectionCanvasObject.activeSelf)
                ConnectionCanvasObject.SetActive(false);

            getInput = BlinkIcon.GetComponent<GetInput>();
            LoadDialogue();
        }

        void Start()
        {
            BlinkIcon.SetActive(false);
            SoundManager.Instance.PlayBGM(SoundName.NightBGM);
            DialogueCoroutine = StartCoroutine(StartDialogue());
        }

        #region FILESAVEANDLOAD
        //하루가 지났을 시 해당 함수를 불러주세요.
        void SaveDayIndex()
        {
            PlayerPrefs.SetInt("Day", PlayerPrefs.GetInt("Day") + 1);
        }

        void LoadDialogue()
        {
            //Init PlayerPrefs
            if (!PlayerPrefs.HasKey("Day"))
            {
                PlayerPrefs.SetInt("Day", 1);
            }

            if(DialogueFileSO == null)
            {
                LoadDialogueFileSO();
            }
        }

        void LoadDialogueFileSO()
        {
            int index = PlayerPrefs.GetInt("Day");
            DialogueFileSO = Resources.Load<DialogueFileSO>("DialogueSOFile/DialogueFile/Day" + index + "/Day" + index);
        }

        //키 값을 초기화 하기 위한 함수
        [ContextMenu("InitDayPlayerPrefs")]
        public void DeleteDayPlayerPrefs()
        {
            PlayerPrefs.DeleteKey("Day");
        }
        #endregion
        #endregion

        public IEnumerator StartDialogue()
        {
            int itemCount = 0;
            List<DialogueItem> itemList = new List<DialogueItem>(DialogueFileSO.DialogueItemList);

            while (true)
            {
                if (itemCount >= itemList.Count)
                {
                    break;
                }

                yield return null;

                //Print Item
                printManager.StartPrint(itemList[itemCount]);

                yield return new WaitUntil(() => printManager.IsPrintEnd == true);
                yield return new WaitForEndOfFrame();

                //Act by type of item
                switch (itemList[itemCount].itemType)
                {
                    case ItemType.NPCText:

                        //itemCount + 1에 대한 indexOutOfRange 체크
                        try
                        {
                            if (itemList[itemCount + 1].itemType == ItemType.NPCText)
                            {
                            }
                        }
                        catch
                        {
                            break;
                        }

                        if (itemList[itemCount + 1].itemType == ItemType.NPCText ||
                            itemList[itemCount + 1].itemType == ItemType.MakeSignLanguage)
                        {
                            BlinkIcon.SetActive(true);
                            yield return new WaitUntil(() => getInput.IsGetInput == true);
                            BlinkIcon.SetActive(false);
                            //Play Click SE
                            SoundManager.Instance.PlaySE(SoundName.Select);
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                        }


                        break;

                    case ItemType.PlayerText:
                        BlinkIcon.SetActive(true);

                        yield return new WaitUntil(() => getInput.IsGetInput == true);
                        //Play Click SE
                        SoundManager.Instance.PlaySE(SoundName.Select);
                        BlinkIcon.SetActive(false);

                        printManager.ReturnChoiceObject();

                        yield return new WaitUntil(() => printManager.IsPrintEnd == true);
                        break;

                    case ItemType.PlayerChoice:

                        dialogueChoiceSelectManager.WaitForSelectChoice();
                        yield return new WaitUntil(() => dialogueChoiceSelectManager.IsChoiceSelected == true);
                        //Play Click SE
                        SoundManager.Instance.PlaySE(SoundName.Select);
                        printManager.ReturnChoiceObject();

                        yield return new WaitUntil(() => printManager.IsPrintEnd == true);

                        SignLanguageSO selectedSignLanguageSO = dialogueChoiceSelectManager.GetSelectedSignLanguageSO();

                        int selectedChoiceNumber = dialogueChoiceSelectManager.SelectedChoiceNumber;

                        //SO가 없다면 무시하고 넘어가기
                        //if it doesn't have SO, break.
                        if (selectedSignLanguageSO == null)
                        {
                            yield return null;
                        }
                        else
                        {
                            //Set Active Popup
                            PopupObject.SetActive(true);
                            IsSwipeEnable = false;

                            //Show SignLanguageUICanvas (Make SignLanguage)
                            signLanguageUIManager.ActiveUIObject(selectedSignLanguageSO.Mean);

                            signLanguageManager.MakeSignLanguage(selectedSignLanguageSO);

                            //waiting offset
                            yield return new WaitForSeconds(waitingTimeOffset);

                            //Show SignLanguageVocabulary while it's closing
                            if (((PlayerChoice)itemList[itemCount]).ChoiceContentList[selectedChoiceNumber].AdditionalSetting.ShowVocabularyFirst)
                            {
                                Vocabulary vocabulary = ((PlayerChoice)itemList[itemCount]).ChoiceContentList[selectedChoiceNumber].Vocabulary;

                                while (!IsSwipeEnable)
                                {
                                    yield return new WaitForSeconds(0.5f);

                                    yield return StartCoroutine(SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(PopupSpeaker, vocabulary));
                                }
                            }

                            yield return new WaitUntil(() => signLanguageManager.IsSignLanguageMade == true);
                            IsSwipeEnable = false;

                            //Close SignLanguageUICanvas
                            signLanguageUIManager.InActiveUIObject();

                            yield return new WaitForSeconds(waitingTimeOffset);

                            //Show SignLanguageVocabulary
                            if (((PlayerChoice)itemList[itemCount]).ChoiceContentList[selectedChoiceNumber].AdditionalSetting.ShowVocabularyLast)
                            {
                                Vocabulary vocabulary = ((PlayerChoice)itemList[itemCount]).ChoiceContentList[selectedChoiceNumber].Vocabulary;
                                yield return StartCoroutine(SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(DialogueSpeaker, vocabulary));
                            }
                        }

                        //if it have DialogueFileSO, load to it
                        if (((PlayerChoice)itemList[itemCount]).ChoiceContentList[selectedChoiceNumber].AdditionalSetting.DialogueSO != null)
                        {
                            DialogueFileSO = ((PlayerChoice)itemList[itemCount]).ChoiceContentList[selectedChoiceNumber].AdditionalSetting.DialogueSO;
                            Restart();
                            yield break;
                        }
                        break;

                    case ItemType.MakeSignLanguage:

                        MakeSignLanguage makeSignLanguageItem = ((MakeSignLanguage)itemList[itemCount]);

                        //Show SignLanguageUICanvas (Make SignLanguage)
                        signLanguageUIManager.ActiveUIObject(makeSignLanguageItem.SignLanguageSO.Mean);

                        signLanguageManager.MakeSignLanguage(makeSignLanguageItem.SignLanguageSO);

                        //waiting offset
                        yield return new WaitForSeconds(waitingTimeOffset);

                        PopupObject.SetActive(true);
                        IsSwipeEnable = false;

                        //Show SignLanguageVocabulary
                        if (makeSignLanguageItem.AdditionalSetting.ShowVocabularyFirst)
                        {
                            Vocabulary vocabulary = makeSignLanguageItem.Vocabulary;

                            while(!IsSwipeEnable)
                            {
                                yield return new WaitForSeconds(0.5f);

                                yield return StartCoroutine(SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(PopupSpeaker, vocabulary));
                            }
                        }

                        yield return new WaitUntil(() => signLanguageManager.IsSignLanguageMade == true);

                        IsSwipeEnable = false;

                        //Close SignLanguageUICanvas
                        signLanguageUIManager.InActiveUIObject();

                        yield return new WaitForSeconds(waitingTimeOffset);

                        //Show SignLanguageVocabulary
                        if (makeSignLanguageItem.AdditionalSetting.ShowVocabularyLast)
                        {
                            Vocabulary vocabulary = makeSignLanguageItem.Vocabulary;
                            yield return StartCoroutine(SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(DialogueSpeaker, vocabulary));
                        }

                        //if it have DialogueFileSO, load to it
                        if (makeSignLanguageItem.AdditionalSetting.DialogueSO != null)
                        {
                            DialogueFileSO = makeSignLanguageItem.AdditionalSetting.DialogueSO;
                            Restart();
                            yield break;
                        }
                        break;

                    case ItemType.Tutorial:
                        //Show SignLanguageUICanvas (Make SignLanguage)
                        signLanguageUIManager.ActiveUIObject(((Tutorial)itemList[itemCount]).SignLanguageSO.Mean);

                        //waiting offset
                        yield return new WaitForSeconds(waitingTimeOffset);

                        GameObject tutorialAsset = Instantiate(((Tutorial)itemList[itemCount]).TutorialAsset, null) as GameObject;

                        yield return StartCoroutine(tutorialAsset.transform.GetChild(0).GetComponent<TutorialManager>().StartTutorial(((Tutorial)itemList[itemCount])));

                        Destroy(tutorialAsset);

                        signLanguageUIManager.InActiveUIObject();

                        //waiting offset
                        yield return new WaitForSeconds(waitingTimeOffset);

                        break;
                }

                itemCount++;
            }

            //3일차의 경우 끝나고 엔딩으로 넘어감
            if(PlayerPrefs.GetInt("Day") == 3)
            {
                EndingCanvasObject.SetActive(true);
                yield break;
            }

            //날짜 저장
            SaveDayIndex();
            //씬 전환
            ConnectionCanvasObject.SetActive(true);

            yield return null;
        }

        public void ClosePopup()
        {
            IsSwipeEnable = true;
            //비활성화시 Enqueue 코루틴에서 오류 발생
            PopupObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(2000, 2000);
        }

        private void Restart()
        {
            StopCoroutine(DialogueCoroutine);
            DialogueCoroutine = StartCoroutine(StartDialogue());
        }
    }
}