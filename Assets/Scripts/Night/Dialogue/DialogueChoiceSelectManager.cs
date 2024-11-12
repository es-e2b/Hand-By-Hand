using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HandByHand.NightSystem.SignLanguageSystem;
using Assets.Scripts.SignLanguage;

namespace HandByHand.NightSystem.DialogueSystem
{
    public class DialogueChoiceSelectManager : MonoBehaviour
    {
        public bool IsChoiceSelected { get; private set; }

        public int SelectedChoiceNumber { get; private set; }

        public Vocabulary selectedChoiceVocabulary;

        private PrintManager printManager;

        private SignLanguageSO SelectedSignLanguageSO;

        void Start()
        {
            IsChoiceSelected = false;
            printManager = gameObject.transform.parent.Find("PrintManager").GetComponent<PrintManager>();
        }

        public void WaitForSelectChoice()
        {
            StartCoroutine(DetectingSelectChoice());
        }

        IEnumerator DetectingSelectChoice()
        {
            yield return new WaitUntil(() => printManager.isSignLanguageSOInit);

            List<GameObject> choiceObjectList = new List<GameObject>(printManager.PooledChoiceObjectList);

            bool isChoiceSelected = false;

            while (!isChoiceSelected)
            {
                for (int i = 0; i < choiceObjectList.Count; i++)
                {
                    if (choiceObjectList[i].GetComponent<ChoiceInformation>().IsSelected == true)
                    {
                        isChoiceSelected = true;
                        SelectedSignLanguageSO = choiceObjectList[i].GetComponent<ChoiceInformation>().GetSignLanguageSO();
                        SelectedChoiceNumber = i;
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
