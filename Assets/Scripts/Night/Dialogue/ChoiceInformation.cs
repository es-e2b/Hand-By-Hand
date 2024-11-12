using HandByHand.NightSystem.SignLanguageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.DialogueSystem
{
    public class ChoiceInformation : MonoBehaviour
    {
        [HideInInspector]
        public SignLanguageSO signLanguageSO;

        public bool IsSelected { get; private set; }

        private static bool HadSelected = false;

        void OnEnabled()
        {
            IsSelected = false;
            HadSelected = false;
        }

        public void Select()
        {
            //�ߺ� ���� ����
            if(HadSelected)
            {
                return;
            }

            IsSelected = true;
            HadSelected = true;
        }

        public SignLanguageSO GetSignLanguageSO()
        {
            return signLanguageSO;
        }
    }
}