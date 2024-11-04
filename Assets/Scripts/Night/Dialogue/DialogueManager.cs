using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using HandByHand.NightSystem.SignLanguageSystem;

//��ȭ �ý����� �����ϴ� �Ŵ����Դϴ�.
namespace HandByHand.NightSystem.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        //���� �ΰ��ӿ��� ���� dialogueSO
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
                //�������� ���� ����ߴٸ� �ݺ��� ����
                if (itemCount >= dialogueFileSO.DialogueItemList.Count) break;

                Debug.Log("ItemPrint");
                //��ȭ ���
                printManager.StartPrint(itemList[itemCount]);

                #region BUGFIXOFFSET
                //������Ʈ ���� Print ���� fix���� (���� �ǰ� �׳� ���λ�)
                float offsetTime = 0.3f;
                yield return new WaitForSeconds(offsetTime);
                #endregion
                
                //��ȭ�� ��µ� ������ ���
                yield return new WaitUntil(() => printManager.IsPrintEnd == true);

                //�������� ��ȭ ������ �ƴ϶�� ����Ͽ� ��ȭ�� ���
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

                    //���� �� ��ȭ �����
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
            }

            yield return null;
        }
    }
}