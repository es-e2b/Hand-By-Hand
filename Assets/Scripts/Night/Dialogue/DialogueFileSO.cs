using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.DialogueSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "DialogueFile", menuName = "Scriptable Object/DialogueFile")]
    public class DialogueFileSO : ScriptableObject
    {
        [SerializeReference]
        #if UNITY_EDITOR
        [ArrayElementTitle("itemType")]
        #endif
        public List<DialogueItem> DialogueItemList = new List<DialogueItem>();

        [ContextMenu("AddNPCText")]
        public void AddNPCText()
        {
            DialogueItemList.Add(new NPCText());
        }

        [ContextMenu("AddPlayerText")]
        public void AddPlayerText()
        {
            DialogueItemList.Add(new PlayerText());
        }

        [ContextMenu("AddPlayerChoice")]
        public void AddPlayerChoice()
        {
            DialogueItemList.Add(new PlayerChoice());
        }
    }
}
