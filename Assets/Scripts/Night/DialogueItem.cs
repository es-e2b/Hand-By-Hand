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
        Player
    }

    public enum itemType
    {
        NPCText,
        PlayerText,
        PlayerChoice
    }

    [System.Serializable]
    public class DialogueItem : MonoBehaviour
    {
        public WhoseItem whoseItem;
        public itemType itemType;

        public DialogueItem(WhoseItem whoseItem, itemType itemType)
        {
            this.whoseItem = whoseItem;
            this.itemType = itemType;
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

        public NPCText() : base(WhoseItem.NPC, itemType.NPCText) { }

        public TMP_Text text
        {
            get => this.Text;
        }
    }

    [System.Serializable]
    public class PlayerText : DialogueItem
    {
        public TMP_Text Text;

        public PlayerText() : base(WhoseItem.Player, itemType.PlayerText) { }

        public TMP_Text text
        {
            get => this.Text;
        }
    }

    [System.Serializable]
    public class PlayerChoice : DialogueItem
    {
        public List<SignLanguageSO> SignLanguageItem;

        public PlayerChoice() : base(WhoseItem.Player, itemType.PlayerChoice) 
        { SignLanguageItem = new List<SignLanguageSO>(); }
    }
}