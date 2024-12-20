namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using System;
    using System.Collections;
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using System.Text;

    [Serializable]
    public class ExecutableDialogueLegacy : ExecutableElement
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

            yield return base.Begin();
        }

        private void OnButtonClick()
        {
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
                bool onTag=false;
                int tagNumber=0;
                for(;currentIndex<=nextIndex&&currentIndex<_currentString.Length;currentIndex++)
                {
                    stringBuilder.Append(_currentString[currentIndex]);
                    if(_currentString[currentIndex]=='<')
                    {
                        onTag=true;
                    }
                    else if(_currentString[currentIndex]=='>')
                    {
                        onTag=false;
                        elapseTime+=(float)tagNumber/_charactersPerSecond;
                        tagNumber=0;
                    }
                    if(onTag)
                    {
                        nextIndex++;
                        tagNumber++;
                    }
                }
                _text.text=stringBuilder.ToString();
            }
            _text.text = _currentString;
            _buttonClicked = false; // 스킵 후 플래그 초기화
            yield return null;

            _nextDialogueIcon.SetActive(true);
            _buttonClicked = false;

            yield return new WaitUntil(() => _buttonClicked); // 버튼 클릭 대기
            _nextDialogueIcon.SetActive(false);
        }

        public override IEnumerator Complete()
        {
            _textPanel.SetActive(false);
            _button.onClick.RemoveListener(OnButtonClick); // 이벤트 제거
            yield break;
        }
    }
}