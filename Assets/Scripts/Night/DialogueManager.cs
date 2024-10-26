using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//대화 시스템을 관장하는 매니저입니다.
namespace HandByHand.NightSystem.SignLanguageSystem

{
    public class DialogueManager : MonoBehaviour
    {
        //현재 인게임에서 사용될 dialogueSO
        public DialogueFileSO dialogueFileSO;

        [SerializeField]
        private PrintManager printManager;

        [SerializeField]
        private SignLanguageManager signLanguageManager;

        [SerializeField]
        private SignLanguageUIManager signLanguageUIManager;

        void Start()
        {

        }

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
                yield return new WaitUntil(() => printManager.IsPrintEnd == true);

                //아이템이 수화 선택이 아니라면 계속하여 대화를 출력
                if (itemList[itemCount].itemType != itemType.PlayerChoice)
                {
                    itemCount++;
                    continue; 
                }
                else
                {
                    //아이템이 수화라면 대화 출력 중지 후 수화 선택 시작
                    signLanguageUIManager.ActiveUIObject();
                    yield return new WaitUntil(() => signLanguageManager.IsSignLanguageMade == true);

                    itemCount++;
                }
            }

            yield return null;
        }
    }
}