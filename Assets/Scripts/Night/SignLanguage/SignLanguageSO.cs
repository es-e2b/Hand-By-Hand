using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "SingLanguageSO", menuName = "Scriptable Object/SignLanguageSO")]
    public class SignLanguageSO : ScriptableObject
    {
        public string Mean;

        public FingerSign FingerSign;

        public Position Position;

        public Shape Shape;

        public Special Special;
    }
}
