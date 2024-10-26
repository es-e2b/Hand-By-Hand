using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//��ȭ �ý����� �����ϴ� �Ŵ����Դϴ�.
namespace HandByHand.NightSystem.SignLanguageSystem

{
    public class DialogueManager : MonoBehaviour
    {
        //���� �ΰ��ӿ��� ���� dialogueSO
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
                //�������� ���� ����ߴٸ� �ݺ��� ����
                if (itemCount >= dialogueFileSO.DialogueItemList.Count) break;

                //��ȭ ���
                printManager.StartPrint(itemList[itemCount]);
                yield return new WaitUntil(() => printManager.IsPrintEnd == true);

                //�������� ��ȭ ������ �ƴ϶�� ����Ͽ� ��ȭ�� ���
                if (itemList[itemCount].itemType != itemType.PlayerChoice)
                {
                    itemCount++;
                    continue; 
                }
                else
                {
                    //�������� ��ȭ��� ��ȭ ��� ���� �� ��ȭ ���� ����
                    signLanguageUIManager.ActiveUIObject();
                    yield return new WaitUntil(() => signLanguageManager.IsSignLanguageMade == true);

                    itemCount++;
                }
            }

            yield return null;
        }
    }
}