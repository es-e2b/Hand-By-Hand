using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using HandByHand.NightSystem.SignLanguageSystem;
using Assets.Scripts.SignLanguage;
using HandByHand.SoundSystem;
using Assets.Scripts;

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

        private delegate GameObject returnObject(GameObject gameObj);
        #endregion

        #region INIT
        void Awake()
        {
            //Set False
            if (EndingCanvasObject.activeSelf)
                EndingCanvasObject.SetActive(false);

            if (ConnectionCanvasObject.activeSelf)
                ConnectionCanvasObject.SetActive(false);

            getInput = BlinkIcon.GetComponent<GetInput>();

            LoadDialogue();
        }

        void Start()
        {
            //노래 재생
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

            if (DialogueFileSO == null)
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
                yield return StartCoroutine(printManager.StartPrint(itemList[itemCount]));

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
                            yield return new WaitUntil(() => !BlinkIcon.activeSelf);
                            //yield return new WaitUntil(() => getInput.IsGetInput == true);
                            //BlinkIcon.SetActive(false);
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
                        yield return new WaitUntil(() => !BlinkIcon.activeSelf);

                        // yield return new WaitUntil(() => getInput.IsGetInput == true);
                        //Play Click SE
                        SoundManager.Instance.PlaySE(SoundName.Select);
                        //BlinkIcon.SetActive(false);

                        yield return StartCoroutine(printManager.ReturnChoiceObject());
                        break;

                    case ItemType.PlayerChoice:

                        yield return StartCoroutine(dialogueChoiceSelectManager.DetectingSelectChoice());
                        //Play Click SE
                        SoundManager.Instance.PlaySE(SoundName.Select);
                        yield return StartCoroutine(printManager.ReturnChoiceObject());

                        SignLanguageSO selectedSignLanguageSO = dialogueChoiceSelectManager.GetSelectedSignLanguageSO();

                        int selectedChoiceNumber = dialogueChoiceSelectManager.SelectedChoiceNumber;

                        ChoiceContent playerChoiceItem = ((PlayerChoice)itemList[itemCount]).ChoiceContentList[selectedChoiceNumber];

                        //SO가 없다면 무시하고 넘어가기
                        //if it doesn't have SO, break.
                        //SO가 있을시 선택 프로토콜 실행
                        if (selectedSignLanguageSO != null)
                        {
                            yield return StartCoroutine(InitAndActiveUI(selectedSignLanguageSO));

                            //Show SignLanguageVocabulary while it's closing
                            if (playerChoiceItem.AdditionalSetting.ShowVocabularyFirst)
                            {
                                yield return StartCoroutine(ShowVocabulary(playerChoiceItem.Vocabulary, PopupSpeaker));
                            }

                            IsSwipeEnable = true;
                            yield return new WaitUntil(() => signLanguageManager.IsSignLanguageMade == true);
                            IsSwipeEnable = false;

                            //Close SignLanguageUICanvas
                            yield return StartCoroutine(signLanguageUIManager.InActiveUIObject());

                            //Show SignLanguageVocabulary while it's closing
                            if (playerChoiceItem.AdditionalSetting.ShowVocabularyLast)
                            {
                                yield return StartCoroutine(SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(DialogueSpeaker, playerChoiceItem.Vocabulary));
                            }
                        }

                        //if it have DialogueFileSO, load to it
                        if (playerChoiceItem.AdditionalSetting.DialogueSO != null)
                        {
                            DialogueFileSO = playerChoiceItem.AdditionalSetting.DialogueSO;
                            Restart();
                            yield break;
                        }
                        break;

                    case ItemType.MakeSignLanguage:

                        MakeSignLanguage makeSignLanguageItem = ((MakeSignLanguage)itemList[itemCount]);

                        SignLanguageSO signLanguageSO = makeSignLanguageItem.SignLanguageSO;

                        yield return StartCoroutine(InitAndActiveUI(signLanguageSO));


                        //Show SignLanguageVocabulary while it's closing
                        if (makeSignLanguageItem.AdditionalSetting.ShowVocabularyFirst)
                        {
                            yield return StartCoroutine(ShowVocabulary(makeSignLanguageItem.Vocabulary, PopupSpeaker));
                        }

                        IsSwipeEnable = true;
                        yield return new WaitUntil(() => signLanguageManager.IsSignLanguageMade == true);
                        IsSwipeEnable = false;

                        //Close SignLanguageUICanvas
                        yield return StartCoroutine(signLanguageUIManager.InActiveUIObject());

                        //Show SignLanguageVocabulary while it's closing
                        if (makeSignLanguageItem.AdditionalSetting.ShowVocabularyLast)
                        {
                            yield return StartCoroutine(SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(DialogueSpeaker, makeSignLanguageItem.Vocabulary));
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
                        yield return StartCoroutine(signLanguageUIManager.ActiveUIObject(((Tutorial)itemList[itemCount]).SignLanguageSO.Mean));

                        GameObject tutorialAsset = Instantiate(((Tutorial)itemList[itemCount]).TutorialAsset);

                        yield return new WaitUntil (() => tutorialAsset != null);

                        yield return StartCoroutine(tutorialAsset.transform.GetChild(0).GetComponent<TutorialManager>().StartTutorial(((Tutorial)itemList[itemCount])));

                        Destroy(tutorialAsset);

                        yield return StartCoroutine(signLanguageUIManager.InActiveUIObject());

                        break;
                }

                itemCount++;
            }

            //3일차의 경우 끝나고 엔딩으로 넘어감
            if (PlayerPrefs.GetInt("Day") == 3)
            {
                EndingCanvasObject.SetActive(true);
                yield break;
            }

            //날짜 저장
            SaveDayIndex();
            //씬 전환
            ConnectionCanvasObject.SetActive(true);
        }

        IEnumerator InitAndActiveUI(SignLanguageSO signLanguageSO)
        {
            //Set Active Popup
            PopupObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1120);
            IsSwipeEnable = false;

            yield return StartCoroutine(signLanguageManager.MakeSignLanguage(signLanguageSO));

            //Show SignLanguageUICanvas (Make SignLanguage)
            yield return StartCoroutine(signLanguageUIManager.ActiveUIObject(signLanguageSO.Mean));
        }

        IEnumerator ShowVocabulary(Vocabulary vocabulary, GameObject speaker)
        {
                while (!IsSwipeEnable)
                {
                    yield return new WaitForSeconds(0.5f);

                    yield return StartCoroutine(SignAnimationRenderer.Instance.StopAndEnqueueVocabulary(speaker, vocabulary));
                }
        }

        //CloseButton Event
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