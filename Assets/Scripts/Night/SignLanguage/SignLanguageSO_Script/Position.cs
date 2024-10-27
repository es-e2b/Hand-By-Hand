using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public enum HandPosition
    {
        OverHead,
        Face,
        Jaw,
        Chest,
        Belly
    }

    [System.Serializable]
    public class Position
    {
        public HandPosition HandPosition;
    }
}
