using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.SoundSystem
{
    public enum SoundName
    {
        BGM,
        BusStart,
        WalkAndListenNatureSound,
        Falling,
        ClothesRustling,
        Surprise,
        ClockTicking,
        TimerEnd,
        CalculatingMachineType,
        Click,
        Success,
        Wrong,
        SceneChange,
        SceneChange2,
    }

    [System.Serializable]
    public class SoundAndName
    {
        public SoundName Name;
        public AudioClip AudioClip;

        public SoundName name
        {
            get => Name;
        }

        public AudioClip audioClip
        {
            get => AudioClip;
        }
    }

    public class SoundClips : MonoBehaviour
    {
        public List<SoundAndName> BGMClips = new List<SoundAndName>();
        public List<SoundAndName> SEClips = new List<SoundAndName>();

        private void Start()
        {
            SoundManager.Instance.Init(BGMClips, SEClips);
        }
    }
}
