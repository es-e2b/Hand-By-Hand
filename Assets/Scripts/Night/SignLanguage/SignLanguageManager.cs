using HandByHand.SoundSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� ���õ� �ý����� �����ϴ� �Ŵ����Դϴ�.
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
        public SignLanguageUIManager signLanguageUIManager;

        private bool isSignLanguageCorrect = false;
        public bool IsSignLanguageMade { get; private set; }

        public bool isCheckEnd { get; private set; } = false;
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

        public IEnumerator MakeSignLanguage(SignLanguageSO signLanguageSO)
        {
            //��ȭ SO ���� �޾ƿ���
            this.signLanguageSO = signLanguageSO;

            //�� ����Ʈ�� component��ũ��Ʈ�� ���� �Ҵ�
            yield return SetComponent();
        }


        private IEnumerator SetComponent()
        {
            yield return StartCoroutine(handCountComponent.SetAnswer(signLanguageSO.HandCount));
            yield return StartCoroutine(symbolAndDirectionComponent.SetAnswer(signLanguageSO.SymbolAndDirection));
            yield return StartCoroutine(handPositionComponent.SetAnswer(signLanguageSO.Position));
            yield return StartCoroutine(particularComponent.SetAnswer(signLanguageSO.Special));
        }

        public void ComparingHandSignSO()
        {
            if (isCheckEnd)
                return;

            isCheckEnd = true;

            //�˻� �� �´��� Ʋ������ ���� isSignLanguageCorrect = true;
            if (handCountComponent.IsCorrect && symbolAndDirectionComponent.IsCorrect
                && handPositionComponent.IsCorrect && particularComponent.IsCorrect)
            {
                isSignLanguageCorrect = true;
                SoundManager.Instance.PlaySE(SoundName.Success);
                isCheckEnd = false;
                StartCoroutine(MadeSignLanguageCoroutine());
            }
            else
            {
                SoundManager.Instance.PlaySE(SoundName.Wrong);
                CheckWrongAnswerAndDisableCorrectAnswer();
            }
        }

        private void CheckWrongAnswerAndDisableCorrectAnswer()
        {
            //��� ��ư�� ��ȣ�ۿ� ���� ����
            signLanguageUIManager.SetButtonInteractable(false);

            //Ʋ�� ���丸 ��ư ��ȣ�ۿ��� Ų��
            if (!handCountComponent.IsCorrect)
                signLanguageUIManager.CheckWrongAnswerButton(0);
            if (!symbolAndDirectionComponent.IsCorrect)
                signLanguageUIManager.CheckWrongAnswerButton(1);
            if (!handPositionComponent.IsCorrect)
                signLanguageUIManager.CheckWrongAnswerButton(2);
            if (!particularComponent.IsCorrect)
                signLanguageUIManager.CheckWrongAnswerButton(3);

            signLanguageUIManager.ChangeToWrongAnswerTab();
            isSignLanguageCorrect = false;
            isCheckEnd = false;
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
            InitBooleanOfEachComponent();
            yield return null;
        }

        private void InitBooleanOfEachComponent()
        {
            handCountComponent.InitBoolean();
            symbolAndDirectionComponent.InitBoolean();
            handPositionComponent.InitBoolean();
            particularComponent.InitBoolean();
        }
        #endregion
    }
}