namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;

    [Serializable]
    public class ExecutableDialogue : ExecutableElement
    {
        [SerializeField]
        private GameObject _textPanel;
        [SerializeField]
        private TMP_Text _text;
        [SerializeField]
        private TMP_Text _wholeText;
        [SerializeField]
        private GameObject _nextDialgueIcon;
        [SerializeField]
        private string[] _dialogue;
        private int _index;
        private string _currentString;
        [SerializeField]
        private int _framesToWait;
        public override IEnumerator Begin()
        {
            Debug.Log("Called Begin");
            _textPanel.SetActive(true);
            _index=0;
            yield return Next();
        }
        public override IEnumerator Next()
        {
            while(_index<_dialogue.Length)
            {
                _text.text="";
                _currentString=_dialogue[_index++];
                _wholeText.text=_currentString;
                LayoutRebuilder.ForceRebuildLayoutImmediate(_wholeText.GetComponent<RectTransform>());
                yield return Execute();
            }
            yield return Complete();
        }
        public override IEnumerator Execute()
        {
            bool skipTriggered = false; // 스킵 트리거 플래그

            // 문자 출력 루프
            for (int i = 0; i < _currentString.Length; i++)
            {
                _text.text += _currentString[i];

                // 입력 체크
                if (Input.GetMouseButtonUp(0))
                {
                    skipTriggered = true; // 스킵 트리거
                    break; // 문자 루프를 종료
                }

                // 프레임 대기 루프
                for (int j = 0; j < _framesToWait; j++)
                {
                    yield return null; // 프레임 대기

                    // 입력 체크 (프레임 대기 중에)
                    if (Input.GetMouseButtonUp(0))
                    {
                        skipTriggered = true; // 스킵 트리거
                        break; // 프레임 대기 루프를 종료
                    }
                }

                // 스킵 트리거가 설정된 경우
                if (skipTriggered)
                {
                    break; // 문자 루프를 종료
                }
            }

            // 스킵이 발생한 경우
            if (skipTriggered)
            {
                yield return Skip(); // 스킵 메서드 호출
            }

            yield return Pause(); // 다음 다이얼로그를 기다리는 메서드 호출
        }
        public override IEnumerator Pause()
        {
            _nextDialgueIcon.SetActive(true);
            yield return new WaitUntil(()=>Input.GetMouseButtonUp(0));
            yield return null;
            _nextDialgueIcon.SetActive(false);
        }
        public override IEnumerator Skip()
        {
            _text.text=_currentString;
            yield return null;
        }
        public override IEnumerator Complete()
        {
            _textPanel.SetActive(false);
            yield break;
        }
    }
}