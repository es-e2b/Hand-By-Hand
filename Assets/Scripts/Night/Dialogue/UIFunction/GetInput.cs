using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.DialogueSystem
{
    public class GetInput : MonoBehaviour
    {
        public bool IsGetInput { get; private set; } = false;

        private void Start()
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
