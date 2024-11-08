namespace Assets.Scripts
{
    using System.Collections;
    using Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem;
    using UnityEngine;

    public class ElementExecutor : MonoBehaviour
    {
        [SerializeField]
        private ExecutableList _executableList;
        private void Start()
        {
            StartCoroutine(Initialize());
        }
        private IEnumerator Initialize()
        {
            if(_executableList==null)
            {
                yield break;
            }
            ExecutableElement[] _executables=_executableList.Executables;
            for(int i=0;i<_executables.Length;i++)
            {
                yield return _executables[i].Initialize();
            }
        }
        public void StartExecutableList(ExecutableList executableList)
        {
            StartCoroutine(ExecuteExecutableList(executableList));
        }
        private IEnumerator ExecuteExecutableList(ExecutableList executableList)
        {
            ExecutableElement[] executables=executableList.Executables;
            for(int i=0;i<executables.Length;i++)
            {
                yield return executables[i].Initialize();
            }
        }
        public void StartExecutableElement(ExecutableElement executableElement)
        {
            StartCoroutine(ExecuteExecutableElement(executableElement));
        }
        private IEnumerator ExecuteExecutableElement(ExecutableElement executableElement)
        {
            yield return executableElement.Initialize();
        }
    }
}