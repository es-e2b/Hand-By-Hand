using Assets.Scripts.SignLanguage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.NightSystem.DialogueSystem
{
    public class SignAnimationRendererManager : MonoBehaviour
    {
        public GameObject Speaker;

        public Vocabulary Vocabulary;

        public bool IsVocabularyEnd { get; private set; } = false; 

        Coroutine enqueueVocabulary;

        public void StartVocabulary(Vocabulary vocabulary)
        {
            InitVocabulary(vocabulary);
            StartCoroutine(StartVocabularyCoroutine());
        }

        public void InitVocabulary(Vocabulary vocabulary)
        {
            Vocabulary = vocabulary;
        }

        IEnumerator StartVocabularyCoroutine()
        {
            yield return StartCoroutine(SignAnimationRenderer.Instance.EnqueueVocabulary(Speaker, Vocabulary));

            yield return StartCoroutine(AnnounceVocabularyEnd());
        }

        IEnumerator AnnounceVocabularyEnd()
        {
            IsVocabularyEnd = true;
            yield return null;

            IsVocabularyEnd = false;
        }
    }
}
