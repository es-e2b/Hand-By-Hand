using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public enum ParticularEnum
    {
        None
    }

    [System.Serializable]
    public class Particular
    {
        public Sprite Sprite;

        public ParticularEnum _Particular;
    }
}