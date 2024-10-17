using HandByHand.NightSystem.SignLanguageSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class SignLanguageData : MonoBehaviour
    {
        [HideInInspector]
        public bool IsFingerSignSelected = false;

        [HideInInspector]
        public bool IsHandCountSelected = false;

        [HideInInspector]
        public bool IsPositionSelected = false;

        [HideInInspector]
        public bool IsSpecialSelected = false;

        [SerializeField]
        private FingerSign fingerSign;

        [SerializeField]
        private HandCount handCount;

        [SerializeField]
        private Position position;

        [SerializeField]
        private SpecialSO special;


        public void SaveFingerSignData(FingerSign _fingerSign)
        {
            this.IsFingerSignSelected = true;
            this.fingerSign = _fingerSign;
        }

        public void SaveHandCountData(HandCount _handCount)
        {
            this.IsHandCountSelected = true;
            this.handCount = _handCount;
        }

        public void SavePositionData(Position _position)
        {
            this.IsPositionSelected = true;
            this.position = _position;
        }

        public void SaveSpecialData(SpecialSO _special)
        {
            this.IsSpecialSelected = true;
            this.special = _special;
        }

        public FingerSign GetFingerSignData()
        {
            if (this.IsFingerSignSelected) return fingerSign;
            else return null;
        }

        public HandCount GetHandCountData()
        {
            if (this.IsHandCountSelected) return handCount;
            else return null;
        }

        public Position GetPositionData()
        {
            if (this.IsPositionSelected) return position;
            else return null;
        }

        public SpecialSO GetSpecialData()
        {
            if (this.IsSpecialSelected) return special;
            else return null;
        }
    }
}