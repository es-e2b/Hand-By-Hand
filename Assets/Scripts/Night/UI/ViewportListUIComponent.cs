using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem
{
    public class ViewportListUIComponent : MonoBehaviour
    {
        [HideInInspector]
        public List<GameObject> UIObject = new List<GameObject>();

        private void Awake()
        {
            if(transform.childCount != 4)
            {
                Debug.Log("Viewport 자식 오브젝트 갯수 오류");
            }

            for(int i = 0; i < gameObject.transform.childCount; i++)
            {
                UIObject.Add(transform.GetChild(i).gameObject);
            }
        }
    }
}
