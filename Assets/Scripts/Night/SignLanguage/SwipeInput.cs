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
            //SignLanguageUICanvas가 Active 되지 않았다면 스와이프는 안 됨
            if (!signLanguageUIManager.SignLanguageUIActiveSelf) return;

            // 터치 입력의 개수를 확인합니다.
            if (Input.touchCount > 0)
            {
                // 첫 번째 터치 입력을 가져옵니다.
                Touch touch = Input.GetTouch(0);

                // 터치 상태에 따라 다른 동작을 수행합니다.
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        // 터치가 시작되면 시작 위치를 기록합니다.
                        fingerDownPosition = touch.position;
                        isSwiping = true;
                        break;

                    case TouchPhase.Moved:
                        break;

                    case TouchPhase.Ended:
                        // 터치가 종료되면 종료 위치를 기록하고 스와이프를 확인합니다.
                        fingerUpPosition = touch.position;
                        CheckSwipe();
                        isSwiping = false;
                        break;
                }
            }
        }

        void CheckSwipe()
        {
            // 스와이프 거리를 계산합니다.
            float swipeDistanceX = Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
            float swipeDistanceY = Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);

            // 좌우 스와이프 확인
            if (isSwiping && swipeDistanceX > swipeThreshold && swipeDistanceX > swipeDistanceY)
            {
                if (fingerDownPosition.x - fingerUpPosition.x > 0 && dialogueManager.IsSwipeEnable)
                {
                    // 오른쪽으로 스와이프한 경우
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
                    // 왼쪽으로 스와이프한 경우
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

