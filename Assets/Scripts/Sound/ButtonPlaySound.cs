using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandByHand.SoundSystem
{
    public class ButtonPlaySound : MonoBehaviour
    {
        [SerializeField]
        private SoundName soundName = SoundName.Click;
        private void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(PlaySound);
        }
        private void PlaySound()
        {
            SoundManager.Instance.PlaySE(soundName);
        }
    }
}