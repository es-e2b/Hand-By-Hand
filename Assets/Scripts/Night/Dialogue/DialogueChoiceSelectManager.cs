using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandByHand.NightSystem.SignLanguageSystem;

namespace HandByHand.NightSystem.DialogueSystem
{
    public class DialogueChoiceSelectManager : MonoBehaviour
    {
        public bool IsChoiceSelected { get; private set; }

        private SignLanguageSO SelectedSignLanguageSO;

        void Start()
        {
            IsChoiceSelected = false;
        }

        public void WaitForSelectChoice()
        {
            PrintManager printManager = gameObject.transform.parent.Find("PrintManager").GetComponent<PrintManager>();
            List<GameObject> choiceObjectList = new List<GameObject>(printManager.PooledChoiceObjectList);

            StartCoroutine(DetectingSelectChoice(choiceObjectList));
        }

        IEnumerator DetectingSelectChoice(List<GameObject> choiceObjectList)
        {
            bool isChoiceSelected = false;

            while (!isChoiceSelected)
            {
                for (int i = 0; i < choiceObjectList.Count; i++)
                {
                    if (choiceObjectList[i].GetComponent<ChoiceInformation>().IsSelected == true)
                    {
                        isChoiceSelected = true;
                        SelectedSignLanguageSO = choiceObjectList[i].GetComponent<ChoiceInformation>().GetSignLanguageSO();
                        break;
                    }
                }
                yield return null;
            }

            yield return StartCoroutine(AnnounceChoiceSelected());
        }

        IEnumerator AnnounceChoiceSelected()
        {
            IsChoiceSelected = true;
            yield return null;

            IsChoiceSelected = false;
            yield return null; 
        }

        public SignLanguageSO GetSelectedSignLanguageSO()
        {
            return SelectedSignLanguageSO;
        }
    }
}
