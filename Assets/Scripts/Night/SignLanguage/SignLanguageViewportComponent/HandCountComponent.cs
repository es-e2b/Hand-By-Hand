using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class HandCountComponent : MonoBehaviour
    {
        private HandCount handCount;

        void Awake()
        {
            handCount.UsingHand = UsingHand.None;
        }


    }
}