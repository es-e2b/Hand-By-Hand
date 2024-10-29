using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� ���õ� �ý����� �����ϴ� �Ŵ����Դϴ�.
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