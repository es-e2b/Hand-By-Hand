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
        public DialogueFileSO dialogueFileSO;

        public GameObject BlinkIcon;
        private GetInput getInput;

        #region MANAGERCOMPONENT
        [SerializeField]
        private SignLanguageUIManager signLanguageUIManager;

        private PrintManager printManager;

        private SignLanguageManager signLanguageManager;

        private DialogueChoiceSelectManager dialogueChoiceSelectManager;

        private SignAnimationRendererManager signAnimationRendererManager;
        #endregion

        #region INIT
        void Awake()
        {
            printManager = gameObject.transform.Find("PrintManager").GetComponent<PrintManager>();
            signLanguageManager = gameObject.transform.Find("SignLanguageManager").GetComponent<SignLanguageManager>();
            dialogueChoiceSelectManager = gameObject.transform.Find("DialogueChoiceSelectManager").GetComponent<DialogueChoiceSelectManager>();
            signAnimationRendererManager = gameObject.transform.Find("SignAnimationRendererManager").GetComponent<SignAnimationRendererManager>();
            getInput = BlinkIcon.GetComponent<GetInput>();
        }

        void Start()
        {
            StartCoroutine(StartDialogue());
            BlinkIcon.SetActive(false);
        }
        #endregion

        IEnumerator StartDialogue()
        {
            int itemCount = 0;
            List<DialogueItem> itemList = new List<DialogueItem>(dialogueFileSO.DialogueItemList);

            while (true)
            {
                if (itemCount >= dialogueFileSO.DialogueItemList.Count)
                {
                    break;
                }

                //Print Item
                printManager.StartPrint(itemList[itemCount]);

                yield return new WaitUntil(() => printManager.IsPrintEnd == true);
                yield return new WaitForEndOfFrame();

                //Act by type of item
                switch (itemList[itemCount].itemType)
                {
                    case ItemType.NPCText:
                        yield return new WaitForSeconds(0.5f);
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

                        SignLanguageSO selectedSignLanguageSO = dialogueChoiceSelectManager.GetSelectedSignLanguageSO();

                        int selectedChoiceNumber = dialogueChoiceSelectManager.SelectedChoiceNumber;

                        //SO가 없다면 무시하고 넘어가기
                        if (selectedSignLanguageSO == null)
                        {
                            break;
                        }
                        else
                        {
                            //Show SignLanguageUICanvas
                            signLanguageUIManager.ActiveUIObject(selectedSignLanguageSO.Mean);

                            signLanguageManager.MakeSignLanguage(selectedSignLanguageSO);

                            float waitingTimeOffset = 1.5f;

                            //ĵ������ �ö���� �ð��� offset���� ��ٷ���
                            yield return new WaitForSeconds(waitingTimeOffset);

                            yield return new WaitUntil(() => signLanguageManager.IsSignLanguageMade == true);

                            signLanguageUIManager.InActiveUIObject();

                            yield return new WaitForSeconds(waitingTimeOffset);

                            signAnimationRendererManager.StartVocabulary(((PlayerChoice)itemList[itemCount]).ChoiceContentList[selectedChoiceNumber].Vocabulary);

                            yield return new WaitUntil(() => signAnimationRendererManager.IsVocabularyEnd == true);
                        }

                        break;
                }

                itemCount++;

                /*
                if (itemList[itemCount].itemType != ItemType.PlayerChoice)
                {
                    itemCount++;
                    continue; 
                }
                else
                {
                    //�������� playerChoice��� �÷��̾��� ������ ���
                    dialogueChoiceSelectManager.WaitForSelectChoice();
                    yield return new WaitUntil(() => dialogueChoiceSelectManager.IsChoiceSelected == true);

                    SignLanguageSO selectedSignLanguageSO = dialogueChoiceSelectManager.GetSelectedSignLanguageSO();

                    //Show SignLanguageUICanvas
                    signLanguageUIManager.ActiveUIObject(selectedSignLanguageSO.Mean);

                    signLanguageManager.MakeSignLanguage(selectedSignLanguageSO);

                    float waitingTimeOffset = 1.5f;
                    //ĵ������ �ö���� �ð��� offset���� ��ٷ���
                    yield return new WaitForSeconds(waitingTimeOffset);

                    yield return new WaitUntil(() => signLanguageManager.IsSignLanguageMade == true);

                    signLanguageUIManager.InActiveUIObject();

                    yield return new WaitForSeconds(waitingTimeOffset);

                    itemCount++;
                }
                */
            }

            yield return null;
        }
    }
}