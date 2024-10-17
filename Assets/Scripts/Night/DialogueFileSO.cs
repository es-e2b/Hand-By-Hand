using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "DialogueFile", menuName = "Scriptable Object/DialogueFile")]
    public class DialogueFileSO : ScriptableObject
    {
        public List<DialogueItem> DialogueItemList = new List<DialogueItem>();

        [ContextMenu("AddNPCText")]
        public void AddNPCText()
        {
            DialogueItemList.Add(new NPCText());
        }

        [ContextMenu("AddPlayerChoice")]
        public void AddPlayerChoice()
        {
            DialogueItemList.Add(new PlayerChoice());
        }
    }
}
