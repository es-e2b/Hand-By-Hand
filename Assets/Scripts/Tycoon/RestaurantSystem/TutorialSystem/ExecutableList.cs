namespace Assets.Scripts.Tycoon.RestaurantSystem.TutorialSystem
{
    using UnityEngine;

    public class ExecutableList : MonoBehaviour
    {
        private ExecutableElement[] _executables;
        public ExecutableElement[] Executables
        {
            get=>_executables;
        }
        private void Awake()
        {
            _executables=GetComponentsInChildren<ExecutableElement>(false);
        }
    }
}