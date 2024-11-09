using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using HandByHand.NightSystem.SignLanguageSystem;
using Assets.Scripts.SignLanguage;

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

        Coroutine DialogueCoroutine;

        #region MANAGERCOMPONENT
        [SerializeField]
        private SignLanguageUIManager signLanguageUIManager;

        private PrintManager printManager;

        private SignLanguageManager signLanguageManager;

        private DialogueChoiceSelectManager dialogueChoiceSelectManager;
        #endregion

        #region INIT
        //하루가 지났을 시 해당 함수를 불러주세요.
        private void SaveDayIndex()
        {
            PlayerPrefs.SetInt("Day", PlayerPrefs.GetInt("Day") + 1);
        }

        void Awake()
        {
            printManager = gameObject.transform.Find("PrintManager").GetComponent<PrintManager>();
            signLanguageManager = gameObject.transform.Find("SignLanguageManager").GetComponent<SignLanguageManager>();
            dialogueChoiceSelectManager = gameObject.transform.Find("DialogueChoiceSelectManager").GetComponent<DialogueChoiceSelectManager>();
            getInput = BlinkIcon.GetComponent<GetInput>();
        }

        void Start()
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
            DialogueCoroutine = StartCoroutine(StartDialogue());
            BlinkIcon.SetActive(false);
        }

        //키 값을 초기화 하기 위한 함수
        [ContextMenu("InitDayPlayerPrefs")]
        public void DeleteDayPlayerPrefs()
        {
            PlayerPrefs.DeleteKey("Day");
        }
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
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.5f);
                        }


                        break;

                    case ItemType.PlayerText:
                        BlinkIcon.SetActive(true);

                        yield return new WaitUntil(() => getInput.IsGetInput == true);
                        BlinkIcon.SetActive(false);

                        printManager.ReturnChoiceObject();

                        yield return new WaitUntil(() => printManager.IsPrintEnd == true);
                        break;

                    case ItemType.PlayerChoice:

                        dialogueChoiceSelectManager.WaitForSelectChoice();
                        yield return new WaitUntil(() => dialogueChoiceSelectManager.IsChoiceSelected == true);

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

                        yield return new WaitForSeconds(0.3f);

                        signLanguageUIManager.InActiveUIObject();

                        //waiting offset
                        yield return new WaitForSeconds(waitingTimeOffset);

                        break;
                }

                itemCount++;
            }

            yield return null;
        }

        public void ClosePopup()
        {
            IsSwipeEnable = true;
            PopupObject.SetActive(false);
        }

        private void Restart()
        {
            StopCoroutine(DialogueCoroutine);
            DialogueCoroutine = StartCoroutine(StartDialogue());
        }

        private void LoadDialogueFileSO()
        {
            int index = PlayerPrefs.GetInt("Day");
            DialogueFileSO = Resources.Load<DialogueFileSO>("DialogueSOFile/DialogueFile/Day" + index + "/Day" + index);
        }
    }
}