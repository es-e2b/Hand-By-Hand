using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SingLanguageSO", menuName = "Scriptable Object/SignLanguageSO")]
    public class SignLanguageSO : ScriptableObject
    {
        //������ �ǹ�
        public string Mean;

        //����
        public Position Position;

        //����
        public HandCount HandCount;

        //Ư��
        public Particular Special;

        //����+����
        public SymbolAndDirection SymbolAndDirection;
    }
}
