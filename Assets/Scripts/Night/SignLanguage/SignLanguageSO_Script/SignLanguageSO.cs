using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SingLanguageSO", menuName = "Scriptable Object/SignLanguageSO")]
    public class SignLanguageSO : ScriptableObject
    {
        //수어의 의미
        public string Mean;

        //수위
        public Position Position;

        //개수
        public HandCount HandCount;

        //특수
        public Particular Special;

        //수형+수향
        public SymbolAndDirection SymbolAndDirection;
    }
}
