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
                Debug.Log("Viewport �ڽ� ������Ʈ ���� ����");
            }

            for(int i = 0; i < gameObject.transform.childCount; i++)
            {
                UIObject.Add(transform.GetChild(i).gameObject);
            }
        }
    }
}
