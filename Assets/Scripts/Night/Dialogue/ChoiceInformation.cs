using HandByHand.NightSystem.SignLanguageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandByHand.NightSystem.DialogueSystem
{
    public class ChoiceInformation : MonoBehaviour
    {
        [HideInInspector]
        public SignLanguageSO signLanguageSO;

        public bool IsSelected { get; set; }

        //중복 클릭 방지
        public static bool alreadySelected { get; set; }

        void OnEnable()
        {
            IsSelected = false;
            alreadySelected = false;
        }

        public void Select()
        {
            if (alreadySelected)
                return;

            IsSelected = true;
            alreadySelected = true;
        }

        public SignLanguageSO GetSignLanguageSO()
        {
            return signLanguageSO;
        }
    }
}