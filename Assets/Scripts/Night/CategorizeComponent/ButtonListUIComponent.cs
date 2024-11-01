using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HandByHand.NightSystem
{
    public class ButtonListUIComponent : MonoBehaviour
    {
        [HideInInspector]
        public List<TMP_Text> buttonText = new List<TMP_Text>();

        [HideInInspector]
        public List<GameObject> buttonGameObject = new List<GameObject>();

        private void Awake()
        {
            //button 오브젝트 아래에는 4개의 자식 오브젝트만 있어야함
            if (gameObject.transform.childCount != 4)
                Debug.Log("button 자식 오브젝트 오류");

            //리스트 오브젝트 initialize
            for (int i = 0; i < 4; i++)
            {
                TMP_Text textComponent = gameObject.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<TMP_Text>();
                GameObject button = gameObject.transform.GetChild(i).gameObject;

                //TMP Component 리스트에 추가
                buttonText.Add(textComponent);
                //버튼 게임 오브젝트 리스트에 추가
                buttonGameObject.Add(button);
            }
        }
    }
}