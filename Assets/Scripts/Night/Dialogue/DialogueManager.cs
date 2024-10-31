using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using HandByHand.NightSystem.SignLanguageSystem;

//대화 시스템을 관장하는 매니저입니다.
namespace HandByHand.NightSystem.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        //현재 인게임에서 사용될 dialogueSO
        public DialogueFileSO dialogueFileSO;

        #region MANAGERCOMPONENT
        [SerializeField]
        private SignLanguageUIManager signLanguageUIManager;

        private PrintManager printManager;

        private SignLanguageManager signLanguageManager;

        private DialogueChoiceSelectManager dialogueChoiceSelectManager;
        #endregion

        #region INIT
        void Awake()
        {
            printManager = gameObject.transform.Find("PrintManager").GetComponent<PrintManager>();
            signLanguageManager = gameObject.transform.Find("SignLanguageManager").GetComponent<SignLanguageManager>();
            dialogueChoiceSelectManager = gameObject.transform.Find("DialogueChoiceSelectManager").GetComponent<DialogueChoiceSelectManager>();
        }

        void Start()
        {
            StartCoroutine(StartDialogue());
        }
        #endregion

        IEnumerator StartDialogue()
        {
            int itemCount = 0;
            List<DialogueItem> itemList = new List<DialogueItem>(dialogueFileSO.DialogueItemList);

            while (true)
            {
                //아이템을 전부 출력했다면 반복문 중지
                if (itemCount >= dialogueFileSO.DialogueItemList.Count) break;

                //대화 출력
                printManager.StartPrint(itemList[itemCount]);

                #region BUGFIXOFFSET
                //오브젝트 연속 Print 버그 fix구문 (몰라도 되고 그냥 냅두삼)
                float offsetTime = 0.3f;
                yield return new WaitForSeconds(offsetTime);
                #endregion
                
                //대화가 출력될 때까지 대기
                yield return new WaitUntil(() => printManager.IsPrintEnd == true);

                //아이템이 수화 선택이 아니라면 계속하여 대화를 출력
                if (itemList[itemCount].itemType != ItemType.PlayerChoice)
                {
                    itemCount++;
                    continue; 
                }
                else
                {
                    //아이템이 playerChoice라면 플레이어의 선택을 대기
                    dialogueChoiceSelectManager.WaitForSelectChoice();
                    yield return new WaitUntil(() => dialogueChoiceSelectManager.IsChoiceSelected == true);

                    SignLanguageSO selectedSignLanguageSO = dialogueChoiceSelectManager.GetSelectedSignLanguageSO();

                    //선택 후 수화 만들기
                    //Show SignLanguageUICanvas
                    signLanguageUIManager.ActiveUIObject(selectedSignLanguageSO.Mean);

                    signLanguageManager.MakeSignLanguage(selectedSignLanguageSO);

                    //캔버스가 올라오는 시간을 offset으로 기다려줌
                    yield return new WaitForSeconds(1.5f);
                    //선택한 선택지를 텍스트로 바꾸는 함수 실행
                    printManager.AdjustPanelHeightAfterSelectChoice();

                    //수화를 만들때까지 대기
                    yield return new WaitUntil(() => signLanguageManager.IsSignLanguageMade == true);

                    signLanguageUIManager.InActiveUIObject();

                    itemCount++;
                }
            }

            yield return null;
        }
    }
}