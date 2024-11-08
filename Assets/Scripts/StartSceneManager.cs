namespace Assets.Scripts
{
    using System.Collections;
    using Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem;
    using UnityEngine;

    public class StartSceneManager : MonoBehaviour
    {
        [SerializeField]
        private ExecutableList _executableList;
        private void Start()
        {
            StartCoroutine(Initialize());
        }
        private IEnumerator Initialize()
        {
            yield return new WaitUntil(()=>GameManager.Instance!=null);
            ExecutableElement[] _executables=_executableList.Executables;
            for(int i=0;i<_executables.Length;i++)
            {
                yield return _executables[i].Initialize();
            }
        }
    }
}