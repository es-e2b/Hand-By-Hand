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

        private AudioSource audioSource;

        private Dictionary<SoundName, AudioClip> bgmDict = new Dictionary<SoundName, AudioClip>();

        private Dictionary<SoundName, AudioClip> seDict = new Dictionary<SoundName, AudioClip>();

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

            audioSource = gameObject.GetComponent<AudioSource>();
        }

        /// <summary>
        /// SoundClip ����
        /// </summary>
        /// <param name="bgmList"></param>
        /// <param name="seList"></param>
        public void Init(List<SoundAndName> bgmList, List<SoundAndName> seList)
        {
            audioSource.volume = BGMVolume;

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
            audioSource.clip = bgmDict[clipName];
            audioSource.Play();
        }

        public void PlaySE(SoundName clipName)
        {
            audioSource.PlayOneShot(seDict[clipName], SEVolume);
        }

        /// <summary>
        /// Ŭ���� �´� EnumŸ���� ���� ��� ����ϱ� ���� �����ε� �Լ�
        /// </summary>
        /// <param name="clip"></param>
        public void PlaySE(AudioClip clip)
        {
            audioSource.PlayOneShot(clip, SEVolume);
        }

        /// <summary>
        /// ���� ������� BGM�� ����
        /// </summary>
        /// <param name="fadeTime">������ ���̵� �� �ð�</param>
        public void StopBGM(float fadeTime = 0)
        {
            if(fadeTime == 0)
            {
                audioSource.Stop();
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
                audioSource.volume = volume;

                time += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = 0;

            audioSource.Stop();
        }

        public void AdjustBGMVolume(float volume)
        {
            audioSource.volume = volume;
            BGMVolume = volume;
        }

        public void AdjustSEVolume(float volume)
        {
            SEVolume = volume;
        }
    }
}
