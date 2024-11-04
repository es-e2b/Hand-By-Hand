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

        void Start() {
            buttonComponent = gameObject.GetComponent<Button>();
            childText = transform.GetChild(0).GetComponent<TMP_Text>();
        }

        void Update() {
            childText.color = (buttonComponent.colors).normalColor;
        }
    }
}