using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.SoundSystem
{
    public enum SoundName
    {
        GameStartBGM,
        AfternoonBGM,
        NightBGM,
        GameEndingBGM,
        BusStart,
        WalkAndListenNatureSound,
        Falling,
        ClothesRustling,
        Surprise,
        ClockTickingAndTimerEnd,
        CalculatingMachineType,
        Select,
        MenuSelect,
        Success,
        Wrong,
        SceneChangeBGM, //연결 화면 BGM
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
