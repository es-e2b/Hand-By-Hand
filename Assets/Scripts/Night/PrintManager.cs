using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//��ȭ�� ����ϴ� �Ŵ����Դϴ�.
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