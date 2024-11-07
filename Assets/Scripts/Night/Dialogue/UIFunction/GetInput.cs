using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace HandByHand.NightSystem.DialogueSystem
{
    public class GetInput : MonoBehaviour
    {
        public bool IsGetInput { get; private set; } = false;

        void OnEnable()
        {
            IsGetInput = false;
        }

        private void Update()
        {
            if(Input.touchCount > 0)
            {
                IsGetInput = true;
            }
        }
    }
}
