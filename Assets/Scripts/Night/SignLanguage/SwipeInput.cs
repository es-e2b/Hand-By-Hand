using HandByHand.NightSystem.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class SwipeInput : MonoBehaviour
    {
        private Vector2 fingerDownPosition;
        private Vector2 fingerUpPosition;
        private bool isSwiping = false;

        public SignLanguageUIManager signLanguageUIManager;
        public DialogueManager dialogueManager;
        public float swipeThreshold = 50f;

        void Update()
        {
            //SignLanguageUICanvas�� Active ���� �ʾҴٸ� ���������� �� ��
            if (!signLanguageUIManager.SignLanguageUIActiveSelf) return;

            // ��ġ �Է��� ������ Ȯ���մϴ�.
            if (Input.touchCount > 0)
            {
                // ù ��° ��ġ �Է��� �����ɴϴ�.
                Touch touch = Input.GetTouch(0);

                // ��ġ ���¿� ���� �ٸ� ������ �����մϴ�.
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        // ��ġ�� ���۵Ǹ� ���� ��ġ�� ����մϴ�.
                        fingerDownPosition = touch.position;
                        isSwiping = true;
                        break;

                    case TouchPhase.Moved:
                        break;

                    case TouchPhase.Ended:
                        // ��ġ�� ����Ǹ� ���� ��ġ�� ����ϰ� ���������� Ȯ���մϴ�.
                        fingerUpPosition = touch.position;
                        CheckSwipe();
                        isSwiping = false;
                        break;
                }
            }
        }

        void CheckSwipe()
        {
            // �������� �Ÿ��� ����մϴ�.
            float swipeDistanceX = Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
            float swipeDistanceY = Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);

            // �¿� �������� Ȯ��
            if (isSwiping && swipeDistanceX > swipeThreshold && swipeDistanceX > swipeDistanceY)
            {
                if (fingerDownPosition.x - fingerUpPosition.x > 0 && dialogueManager.IsSwipeEnable)
                {
                    // ���������� ���������� ���
                    if ((signLanguageUIManager.presentPanelIndex + 1) < 4)
                    {
                        if (signLanguageUIManager.incorrectAnswerIndexList[0] == -1)
                        {
                            signLanguageUIManager.ChangeUITab(signLanguageUIManager.presentPanelIndex + 1);
                        }
                        else
                        {
                            foreach (int i in signLanguageUIManager.incorrectAnswerIndexList)
                            {
                                if (i == (signLanguageUIManager.presentPanelIndex + 1))
                                {
                                    signLanguageUIManager.ChangeUITab(signLanguageUIManager.presentPanelIndex + 1);
                                }
                            }
                        }
                    }
                }
                else
                {
                    // �������� ���������� ���
                    if ((signLanguageUIManager.presentPanelIndex - 1) > -1 && dialogueManager.IsSwipeEnable)
                    {
                        if (signLanguageUIManager.incorrectAnswerIndexList[0] != -1)
                        {
                            signLanguageUIManager.ChangeUITab(signLanguageUIManager.presentPanelIndex - 1);
                        }
                        else
                        {
                            foreach (int i in signLanguageUIManager.incorrectAnswerIndexList)
                            {
                                if (i == (signLanguageUIManager.presentPanelIndex - 1))
                                {
                                    signLanguageUIManager.ChangeUITab(signLanguageUIManager.presentPanelIndex - 1);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

