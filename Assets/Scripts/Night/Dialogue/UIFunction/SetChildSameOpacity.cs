using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HandByHand.NightSystem.DialogueSystem
{
    public class SetTextSameOpacity : MonoBehaviour
    {
        private Button buttonComponent;
        private TMP_Text childText;
        private float R;
        private float G;
        private float B;

        void Start() {
            buttonComponent = gameObject.GetComponent<Button>();
            childText = transform.GetChild(0).GetComponent<TMP_Text>();

            R = childText.color.r;
            G = childText.color.g;
            B = childText.color.b;
        }

        void Update() {
            childText.color = new Color(R, G, B, (buttonComponent.colors).normalColor.a);
        }
    }
}