using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace HandByHand.NightSystem.DialogueSystem
{
    public class ImageBlink : MonoBehaviour
    {
        private Image image;

        public float BlinkTime = 1f;

        private void Awake()
        {
            image = transform.GetComponent<Image>();
        }

        void OnEnable()
        {
            image.color = new Color(1, 1, 1, 1);
            StartCoroutine(BlinkAnimation());
        }

        IEnumerator BlinkAnimation()
        {
            while (true)
            {
                yield return new WaitForSeconds(BlinkTime);

                image.color = new Color(1, 1, 1, 0);

                yield return new WaitForSeconds(BlinkTime);

                image.color = new Color(1, 1, 1, 1);
            }
        }
    }
}