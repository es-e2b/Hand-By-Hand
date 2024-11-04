namespace Assets.Scripts.SignLanguage
{
    using UnityEngine;

    public class CommonSignLanguageDictionary : MonoBehaviour
    {
        public static CommonSignLanguageDictionary Instance { get; private set; }
        [SerializeField]
        private SignLanguageDictionary _commonSignLanguageDictionary;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        public Vocabulary this[CommonSignLanguage commonSignLanguage]
        {
            get=> _commonSignLanguageDictionary[commonSignLanguage.ToString()];
        }
        public enum CommonSignLanguage
        {
            Thank
        }
    }
}