using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HandByHand.SoundSystem
{
    public class SoundManager : MonoBehaviour
    {
        #region FIELD
        [HideInInspector]
        static public SoundManager Instance { get; private set; }

        [SerializeField]
        private AudioSource _BGMAudioSource;
        [SerializeField]
        private AudioSource _SEAudioSource;

        public Dictionary<SoundName, AudioClip> bgmDict = new Dictionary<SoundName, AudioClip>();

        public Dictionary<SoundName, AudioClip> seDict = new Dictionary<SoundName, AudioClip>();

        public float BGMVolume = 0.5f;

        public float SEVolume = 0.5f;
        #endregion

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

        /// <summary>
        /// SoundClip ����
        /// </summary>
        /// <param name="bgmList"></param>
        /// <param name="seList"></param>
        public void Init(List<SoundAndName> bgmList, List<SoundAndName> seList)
        {
            _BGMAudioSource.volume = BGMVolume;

            for (int i = 0; i < bgmList.Count; i++)
            {
                bgmDict.Add(bgmList[i].Name, bgmList[i].AudioClip);
            }

            for (int i = 0; i < seList.Count; i++)
            {
                seDict.Add(seList[i].Name, seList[i].AudioClip);
            }
        }

        public void PlayBGM(SoundName clipName)
        {
            StopBGM();
            _BGMAudioSource.clip = bgmDict[clipName];
            _BGMAudioSource.Play();
        }
        public void StopBGM()
        {
            _BGMAudioSource.Stop();
        }

        public void PlaySE(SoundName clipName)
        {
            _SEAudioSource.PlayOneShot(seDict[clipName], SEVolume);
        }
        public void StopSE()
        {
            _SEAudioSource.Stop();
        }

        /// <summary>
        /// Ŭ���� �´� EnumŸ���� ���� ��� ����ϱ� ���� �����ε� �Լ�
        /// </summary>
        /// <param name="clip"></param>
        public void PlaySE(AudioClip clip)
        {
            StopSE();
            _SEAudioSource.PlayOneShot(clip, SEVolume);
        }

        /// <summary>
        /// ���� ������� BGM�� ����
        /// </summary>
        /// <param name="fadeTime">������ ���̵� �� �ð�</param>
        public void StopBGM(float fadeTime = 0)
        {
            if(fadeTime == 0)
            {
                _BGMAudioSource.Stop();
                return;
            }
            else
            {
                StartCoroutine(BGMFadeIn(fadeTime));
            }
        }

        /// <summary>
        /// ������ fadeTime���� õõ�� �ٿ�����.
        /// </summary>
        /// <param name="fadeTime"></param>
        /// <returns></returns>
        IEnumerator BGMFadeIn(float fadeTime)
        {
            float time = 0;

            float volume;

            while(time < fadeTime)
            {
                volume = (fadeTime - time) / fadeTime;
                _BGMAudioSource.volume = volume;

                time += Time.deltaTime;
                yield return null;
            }

            _BGMAudioSource.volume = 0;

            _BGMAudioSource.Stop();
        }

        public void AdjustBGMVolume(float volume)
        {
            _BGMAudioSource.volume = volume;
            BGMVolume = volume;
        }

        public void AdjustSEVolume(float volume)
        {
            SEVolume = volume;
        }
    }
}
