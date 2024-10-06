using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class FingerSignUIManager : MonoBehaviour
    {
        private FingerSign fingerSign = new FingerSign();

        [SerializeField]
        private Image[] fingerSignImageComp = new Image[5];

        [SerializeField]
        private Sprite[] foldFingerImage = new Sprite[5];

        [SerializeField]
        private Sprite[] unfoldFingerImage = new Sprite[5];

        private void Start()
        {
            fingerSign.Thumb = FingerState.Unfold;
            fingerSign.IndexFinger = FingerState.Unfold;
            fingerSign.MiddleFinger = FingerState.Unfold;
            fingerSign.RingFinger = FingerState.Unfold;
            fingerSign.Pinky = FingerState.Unfold;

            for (int i = 0; i < fingerSignImageComp.Length; i++)
            {
                fingerSignImageComp[i].sprite = unfoldFingerImage[i];
            }
        }

        public void ThumbAction()
        {
            if (fingerSign.Thumb == FingerState.Unfold)
            {
                fingerSignImageComp[4].sprite = foldFingerImage[4];
                fingerSign.Thumb = FingerState.Fold;
            }
            else
            {
                fingerSignImageComp[4].sprite = unfoldFingerImage[4];
                fingerSign.Thumb = FingerState.Unfold;
            }
        }

        public void IndexFingerAction()
        {
            if (fingerSign.IndexFinger == FingerState.Unfold)
            {
                fingerSignImageComp[0].sprite = foldFingerImage[0];
                fingerSign.IndexFinger = FingerState.Fold;
            }
            else
            {
                fingerSignImageComp[0].sprite = unfoldFingerImage[0];
                fingerSign.IndexFinger = FingerState.Unfold;
            }
        }

        public void MiddleFingerAction()
        {
            if (fingerSign.MiddleFinger == FingerState.Unfold)
            {
                fingerSignImageComp[1].sprite = foldFingerImage[1];
                fingerSign.MiddleFinger = FingerState.Fold;
            }
            else
            {
                fingerSignImageComp[1].sprite = unfoldFingerImage[1];
                fingerSign.MiddleFinger = FingerState.Unfold;
            }
        }

        public void RingFingerAction()
        {
            if (fingerSign.RingFinger == FingerState.Unfold)
            {
                fingerSignImageComp[2].sprite = foldFingerImage[2];
                fingerSign.RingFinger = FingerState.Fold;
            }
            else
            {
                fingerSignImageComp[2].sprite = unfoldFingerImage[2];
                fingerSign.RingFinger = FingerState.Unfold;
            }
        }

        public void PinkyAction()
        {
            if (fingerSign.Pinky == FingerState.Unfold)
            {
                fingerSignImageComp[3].sprite = foldFingerImage[3];
                fingerSign.Pinky = FingerState.Fold;
            }
            else
            {
                fingerSignImageComp[3].sprite = unfoldFingerImage[3];
                fingerSign.Pinky = FingerState.Unfold;
            }
        }
    }
}
