namespace Assets.Scripts.SignLanguage
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class SignLanguageDictionary
    {
        [SerializeField]
        private Vocabulary[] _vocabulary;
        private Dictionary<string, Vocabulary> _vocabularyDictionary;
        public Vocabulary this[string key]
        {
            get
            {
                EnsureInitialized();
                return _vocabularyDictionary.TryGetValue(key, out var vocabulary) ? vocabulary : null;
            }
        }
        private void EnsureInitialized()
        {
            if (_vocabularyDictionary == null)
            {
                _vocabularyDictionary = new Dictionary<string, Vocabulary>();
                foreach (var vocabulary in _vocabulary)
                {
                    if (vocabulary != null && !_vocabularyDictionary.ContainsKey(vocabulary.Name))
                    {
                        _vocabularyDictionary.Add(vocabulary.Name, vocabulary);
                    }
                }
            }
        }
    }
}