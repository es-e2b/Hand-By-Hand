namespace Assets.Scripts
{
    using UnityEngine;
    using CharacterData;

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public Character[] CharacterPool;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}