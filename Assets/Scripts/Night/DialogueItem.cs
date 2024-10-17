using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HandByHand.NightSystem.SignLanguageSystem;

namespace HandByHand.NightSystem
{
    public enum WhoseItem
    {
        NPC,
        Protagonist
    }

    [System.Serializable]
    public class DialogueItem : MonoBehaviour
    {
        public WhoseItem itemType;

        public DialogueItem(WhoseItem whoseItem)
        {
            this.itemType = whoseItem;
        }

        public WhoseItem WhoseItem
        {
            get => WhoseItem;
        }
    }

    [System.Serializable]
    public class NPCText : DialogueItem
    {
        public TMP_Text Text;

        public NPCText() : base(WhoseItem.NPC) { }

        public TMP_Text text
        {
            get => this.Text;
        }
    }

    [System.Serializable]
    public class PlayerChoice : DialogueItem
    {
        public List<SignLanguageSO> SignLanguageItem;

        public PlayerChoice() : base(WhoseItem.Protagonist) { SignLanguageItem = new List<SignLanguageSO>(); }
    }
}