using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class SignLanguageUIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject fingerSignObject;
        [SerializeField]
        private GameObject handCountObject;
        [SerializeField]
        private GameObject positionObject;
        [SerializeField]
        private GameObject specialObject;

        private GameObject nowUsingObject;

        private void Start()
        {
            if(!fingerSignObject.activeSelf)
            {
                fingerSignObject.SetActive(true);
            }
            handCountObject.SetActive(false);
            positionObject.SetActive(false);
            specialObject.SetActive(false);

            nowUsingObject = fingerSignObject;
        }

        public void ShowFingerSignUI()
        {
            fingerSignObject.SetActive(true);
            handCountObject.SetActive(false);
            positionObject.SetActive(false);
            specialObject.SetActive(false);
        }

        public void ShowHandCountUI()
        {
            fingerSignObject.SetActive(false);
            handCountObject.SetActive(true);
            positionObject.SetActive(false);
            specialObject.SetActive(false);
        }

        public void ShowPositionUI()
        {
            fingerSignObject.SetActive(false);
            handCountObject.SetActive(false);
            positionObject.SetActive(true);
            specialObject.SetActive(false);
        }

        public void ShowSpecialUI()
        {
            fingerSignObject.SetActive(false);
            handCountObject.SetActive(false);
            positionObject.SetActive(false);
            specialObject.SetActive(true);
        }

        public void CloseFingerSignUI()
        {

        }
    }
}
