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

            InitName();
        }

        private void InitName()
        {
            for (int i = DialogueItemList.Count - 2; i >= 0; i--)
            {
                if (DialogueItemList[i].itemType == ItemType.NPCText)
                {
                    ((NPCText)DialogueItemList[DialogueItemList.Count - 1]).Name = ((NPCText)DialogueItemList[i]).Name;
                }
            }
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

        [ContextMenu("AddTutorial")]
        public void AddTutorial()
        {
            DialogueItemList.Add(new Tutorial());
        }
    }
}
