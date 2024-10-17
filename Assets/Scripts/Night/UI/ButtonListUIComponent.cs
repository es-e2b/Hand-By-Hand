using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class ButtonListUIComponent : MonoBehaviour
    {
        public List<TMP_Text> buttonText = new List<TMP_Text>();

        private void Awake()
        {
            //button 오브젝트 아래에는 4개의 자식 오브젝트만 있어야함
            if (gameObject.transform.childCount != 4)
                Debug.Log("button 자식 오브젝트 오류");

            //리스트 오브젝트 initialize
            for (int i = 0; i < 4; i++)
            {
                TMP_Text textComponent = gameObject.transform.GetChild(i).gameObject.transform.GetChild(0).GetComponent<TMP_Text>();

                //버튼 오브젝트 할당
                buttonText.Add(textComponent);
            }
        }
    }
}