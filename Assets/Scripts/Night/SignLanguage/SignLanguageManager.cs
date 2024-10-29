using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//수어 관련된 시스템을 관리하는 매니저입니다.
namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class SignLanguageManager : MonoBehaviour
    {
        [HideInInspector]
        public SignLanguageSO SignLanguageSO;

        public bool IsSignLanguageMade { get; private set; }

        void Start()
        {
            IsSignLanguageMade = false;
        }

        public void SetSelectedSignLanguageSO(SignLanguageSO signLanguageSO)
        {
            SignLanguageSO = signLanguageSO;
        }

        public void MakeSignLanguage()
        {
            
        }

        public void ComparingHandSignSO(SignLanguageSO singLanguageSO)
        {

        }
    }
}