using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� ���õ� �ý����� �����ϴ� �Ŵ����Դϴ�.
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