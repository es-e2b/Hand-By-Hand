using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HandByHand.NightSystem.SignLanguageSystem;
using Assets.Scripts.SignLanguage;
using static HandByHand.NightSystem.DialogueSystem.PlayerChoice;

namespace HandByHand.NightSystem.DialogueSystem
{
    public enum WhoseItem
    {
        NPC,
        Player,
        None
    }

    public enum ItemType
    {
        NPCText,
        PlayerText,
        PlayerChoice,
        Tutorial,
        MakeSignLanguage
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
        public string Name;

        [TextArea(3, 6)]
        public string Text;

        public NPCText() : base(WhoseItem.NPC, ItemType.NPCText)
        { 
        }

        public string text
        {
            get => Text;
        }
    }

    [System.Serializable]
    public class PlayerText : DialogueItem
    {
        [TextArea(3, 8)]
        public List<string> Text;

        public PlayerText() : base(WhoseItem.Player, ItemType.PlayerText)
        {
            Text = new List<string>();
        }
    }

    [System.Serializable]
    public class AdditionalSetting
    {
        public DialogueFileSO DialogueSO = null;
        public bool ShowVocabularyFirst = true;
        public bool ShowVocabularyLast = true;
    }

    [System.Serializable]
    public class PlayerChoice : DialogueItem
    {
        public List<ChoiceContent> ChoiceContentList;

        public PlayerChoice() : base(WhoseItem.Player, ItemType.PlayerChoice)
        {
            ChoiceContentList = new List<ChoiceContent>();
        }
    }

    [System.Serializable]
    public class ChoiceContent
    {
        public SignLanguageSO SignLanguageItem;
        public string ChoiceText;
        public Vocabulary Vocabulary;
        public AdditionalSetting AdditionalSetting;

        public ChoiceContent()
        {
            AdditionalSetting = new AdditionalSetting();
        }
    }

    [System.Serializable]
    public class MakeSignLanguage : DialogueItem
    {
        public SignLanguageSO SignLanguageSO;
        public Vocabulary Vocabulary;
        public AdditionalSetting AdditionalSetting;

        public MakeSignLanguage() : base(WhoseItem.Player, ItemType.MakeSignLanguage)
        { 
            AdditionalSetting = new AdditionalSetting();
        }
    }

    [System.Serializable]
    public class Tutorial : DialogueItem
    {
        public GameObject TutorialAsset;

        public SignLanguageSO SignLanguageSO;

        public Vocabulary Vocabulary;

        public Tutorial() : base(WhoseItem.None, ItemType.Tutorial)
        {

        }
    }
}