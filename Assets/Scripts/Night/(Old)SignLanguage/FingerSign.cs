using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public enum FingerState
    {
        Fold,
        Unfold
    }

    [System.Serializable]
    public class FingerSign
    {
        public FingerState Thumb = FingerState.Unfold;

        public FingerState IndexFinger = FingerState.Unfold;

        public FingerState MiddleFinger = FingerState.Unfold;

        public FingerState RingFinger = FingerState.Unfold;

        public FingerState Pinky = FingerState.Unfold;
    }
}