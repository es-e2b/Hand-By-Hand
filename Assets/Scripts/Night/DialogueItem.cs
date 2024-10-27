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

    public enum ItemType
    {
        NPCText,
        PlayerText,
        PlayerChoice
    }

    [System.Serializable]
    public class DialogueItem
    {
        [HideInInspector]
        public WhoseItem whoseItem;
        [HideInInspector]
        public ItemType itemType;

        public DialogueItem(WhoseItem whoseItem, ItemType itemType)
        {
            this.whoseItem = whoseItem;
            this.itemType = itemType;
        }

        public WhoseItem WhoseItem
        {
            get => WhoseItem;
        }
        public ItemType ItemType
        {
            get => ItemType;
        }
    }

    [System.Serializable]
    public class NPCText : DialogueItem
    {
        [TextArea(3, 8)]
        public string Text;

        public NPCText() : base(WhoseItem.NPC, ItemType.NPCText) { }

        public string text
        {
            get => Text;
        }
    }

    [System.Serializable]
    public class PlayerText : DialogueItem
    {
        [TextArea(3, 8)]
        public string Text;

        public PlayerText() : base(WhoseItem.Player, ItemType.PlayerText) { }

        public string text
        {
            get => Text;
        }
    }

    [System.Serializable]
    public class PlayerChoice : DialogueItem
    {
        public List<SignLanguageSO> SignLanguageItem;

        public PlayerChoice() : base(WhoseItem.Player, ItemType.PlayerChoice) 
        { SignLanguageItem = new List<SignLanguageSO>(); }
    }
}