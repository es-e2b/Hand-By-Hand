using HandByHand.NightSystem.DialogueSystem;
using HandByHand.SoundSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem
{
    public class GameManager : MonoBehaviour
    {
        public DialogueManager DialogueManager;

        private void Start()
        {
            DialogueManager.DialogueCoroutine = StartCoroutine(DialogueManager.StartDialogue());
            SoundManager.Instance.PlayBGM(SoundName.NightBGM);
        }
    }
}
