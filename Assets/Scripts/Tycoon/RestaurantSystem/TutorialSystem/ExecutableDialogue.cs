namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using System.Text;

    [Serializable]
    public class ExecutableDialogue : ExecutableElement
    {
        // [SerializeField] 
        private GameObject _textPanel;
        [SerializeField]
        private TMP_Text _text;
        [SerializeField]
        private TMP_Text _wholeText;
        [SerializeField]
        private GameObject _nextDialogueIcon;
        [SerializeField]
        private string[] _dialogue;
        private int _index;
        private string _currentString;
        [SerializeField]
        private int _charactersPerSecond;
        private bool _buttonClicked = false;
        [SerializeField]
        private Button _button;

        public override IEnumerator Begin()
        {
            Debug.Log("Called Begin");
            _textPanel=transform.GetChild(0).gameObject;
            _textPanel.SetActive(true);
            _index = 0;

            _button.onClick.AddListener(OnButtonClick);

            yield return Next();
        }

        private void OnButtonClick()
        {
            print("Called On Button Click Method");
            _buttonClicked = true; // 버튼 클릭 이벤트 발생 시 플래그를 설정
        }

        public override IEnumerator Next()
        {
            while (_index < _dialogue.Length)
            {
                _text.text = "";
                _currentString = _dialogue[_index++];
                _wholeText.text = _currentString;
                LayoutRebuilder.ForceRebuildLayoutImmediate(_wholeText.GetComponent<RectTransform>());
                yield return Execute();
            }
            yield return Complete();
        }

        public override IEnumerator Execute()
        {
            StringBuilder stringBuilder=new();
            float elapseTime=0f;
            int currentIndex=0;
            _buttonClicked = false;

            while(!_buttonClicked&&currentIndex<_currentString.Length)
            {
                yield return null;
                elapseTime+=Time.deltaTime;
                int nextIndex=(int)(elapseTime*_charactersPerSecond);
                for(;currentIndex<=nextIndex&&currentIndex<_currentString.Length;currentIndex++)
                {
                    stringBuilder.Append(_currentString[currentIndex]);
                }
                _text.text=stringBuilder.ToString();
            }
            if(_buttonClicked)
            {
                yield return Skip();
            }

            yield return Pause();
        }

        public override IEnumerator Pause()
        {
            _nextDialogueIcon.SetActive(true);
            _buttonClicked = false;

            print("Called Dialogue Element Pause Method");

            yield return new WaitUntil(() => _buttonClicked); // 버튼 클릭 대기
            _nextDialogueIcon.SetActive(false);
        }

        public override IEnumerator Skip()
        {
            _text.text = _currentString;
            _buttonClicked = false; // 스킵 후 플래그 초기화
            yield return null;
        }

        public override IEnumerator Complete()
        {
            _textPanel.SetActive(false);
            _button.onClick.RemoveListener(OnButtonClick); // 이벤트 제거
            yield break;
        }
    }
}