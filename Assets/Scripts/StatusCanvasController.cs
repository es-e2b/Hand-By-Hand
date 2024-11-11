namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class StatusCanvasController : MonoBehaviour
    {
        public static StatusCanvasController Instance { get; private set; }
        [SerializeField]
        private GameObject _statusPanel;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded+=(_, _)=>GetComponent<Canvas>().worldCamera=Camera.main;
        }
        private void Start()
        {
            _statusPanel.SetActive(true);
        }
    }
}