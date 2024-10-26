using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//대화를 출력하는 매니저입니다.
namespace HandByHand.NightSystem
{
    public class PrintManager : MonoBehaviour
    {
        [SerializeField]
        DialogueFileSO dialogueFile;

        [HideInInspector]
        public bool IsPrintEnd = false;

        public void StartPrint(DialogueItem dialogueItem)
        {

        }

        IEnumerator StartPrintCoroutine()
        {
            yield return null;
        }
    }
}