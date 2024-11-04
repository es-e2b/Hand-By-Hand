using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HandByHand.NightSystem.DialogueSystem
{
    public class PlayerChoiceButtonComponentList : MonoBehaviour
    {
        public int ignoreLayoutIndex = 0;

        [HideInInspector]
        public List<Button> ButtonComponentList = new List<Button>();
        
        [HideInInspector]
        public List<TMP_Text> TextComponentList = new List<TMP_Text>();

        void Awake()
        {

            for(int i = ignoreLayoutIndex; i < transform.childCount; i++) 
            {
                if(!transform.GetChild(i).gameObject.activeSelf)
                    transform.GetChild(i).gameObject.SetActive(true);

                ButtonComponentList.Add(transform.GetChild(i).GetComponent<Button>());
                TextComponentList.Add(transform.GetChild(i).GetChild(0).GetComponent<TMP_Text>());
            }
        }

        void Start() {
            for(int i = 0; i < ButtonComponentList.Count; i++) {
                ButtonComponentList[i].gameObject.SetActive(false);
            }
        }
    }
}
