using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//수어 관련된 시스템을 관리하는 매니저입니다.
namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class SignLanguageManager : MonoBehaviour
    {
        public SignLanguageSO ComparingSignLanguage;

        [HideInInspector]
        public bool IsSignLanguageMade = false;

        public void ComparingHandSignSO(SignLanguageSO singLanguageSO)
        {

        }
    }
}