using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//수어 관련된 시스템을 관리하는 매니저입니다.
namespace HandByHand.NightSystem.SignLanguageSystem
{
    public class SignLanguageManager : MonoBehaviour
    {
        #region VIEWPORTUICOMPONENT
        public ViewportListUIComponent ViewportListUIComponent;

        private List<GameObject> viewportUIObjectList;

        private HandCountComponent handCountComponent;
        private SymbolAndDirectionComponent symbolAndDirectionComponent;
        private HandPositionComponent handPositionComponent;
        private ParticularComponent particularComponent;
        #endregion

        #region SIGNLANGUAGECOMPONENT
        private SignLanguageSO signLanguageSO;

        private bool isSignLanguageCorrect = false;
        public bool IsSignLanguageMade { get; private set; }
        #endregion

        #region INIT
        void Awake()
        {
            IsSignLanguageMade = false;
        }

        void Start()
        {
            viewportUIObjectList = ViewportListUIComponent.UIObject;

            handCountComponent = viewportUIObjectList[0].GetComponent<HandCountComponent>();
            symbolAndDirectionComponent = viewportUIObjectList[1].GetComponent<SymbolAndDirectionComponent>();
            handPositionComponent = viewportUIObjectList[2].GetComponent<HandPositionComponent>();
            particularComponent = viewportUIObjectList[3].GetComponent<ParticularComponent>();
        }
        #endregion

        public void MakeSignLanguage(SignLanguageSO signLanguageSO)
        {
            //수화 SO 파일 받아오기
            this.signLanguageSO = signLanguageSO;
            
            //각 뷰포트의 component스크립트에 변수 할당
            SetComponent();
        }


        private void SetComponent()
        {
            handCountComponent.SetAnswer(signLanguageSO.HandCount);
            symbolAndDirectionComponent.SetAnswer(signLanguageSO.SymbolAndDirection);
            handPositionComponent.SetAnswer(signLanguageSO.Position);
            particularComponent.SetAnswer(signLanguageSO.Special);
        }

        public void ComparingHandSignSO()
        {
            //검사 후 맞는지 틀린지에 따라 isSignLanguageCorrect = true;
            if(handCountComponent.IsCorrect && symbolAndDirectionComponent.IsCorrect
                &&handPositionComponent.IsCorrect && particularComponent.IsCorrect)
            {
                isSignLanguageCorrect = true;
                StartCoroutine(MadeSignLanguageCoroutine());
            }
            else
                isSignLanguageCorrect= false;
        }

        #region COROUTINE
        IEnumerator MadeSignLanguageCoroutine()
        {
            yield return new WaitUntil(() => isSignLanguageCorrect == true);

            yield return StartCoroutine(AnnounceSignLanguageMade());
        }
        
        IEnumerator AnnounceSignLanguageMade()
        {
            //Reset variable
            IsSignLanguageMade = true;
            yield return null;

            IsSignLanguageMade = false;
            isSignLanguageCorrect = false;
            yield return null;
        }
        #endregion
    }
}