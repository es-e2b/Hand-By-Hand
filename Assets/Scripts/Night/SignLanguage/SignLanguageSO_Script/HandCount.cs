using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public enum UsingHand
    {
        BothHand,
        OneHand,
        None
    }

    [System.Serializable]
    public class HandCount
    {
        public UsingHand UsingHand;
    }
}