using HandByHand.NightSystem.SignLanguageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.DialogueSystem
{
    public class ChoiceInformation : MonoBehaviour
    {
        public SignLanguageSO signLanguageSO;

        public bool IsSelected { get; private set; }

        private static bool HadSelected = false;

        void Awake()
        {
            IsSelected = false;
        }

        public void Select()
        {
            //중복 선택 방지
            if(HadSelected)
            {
                return;
            }

            IsSelected = true;
            HadSelected = true;
        }

        public SignLanguageSO GetSignLanguageSO()
        {
            ResetVariable();
            return signLanguageSO;
        }

        private void ResetVariable()
        {
            HadSelected = false;
        }
    }
}