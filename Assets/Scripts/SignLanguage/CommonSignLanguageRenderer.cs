namespace Assets.Scripts.SignLanguage
{
    using UnityEngine;

    public class CommonSignLanguageRenderer : MonoBehaviour
    {
        public static CommonSignLanguageRenderer Instance { get; private set; }
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
        private void Start()
        {
            
        }
    }
}