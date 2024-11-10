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
        /// SoundClip 배정
        /// </summary>
        /// <param name="bgmList"></param>
        /// <param name="seList"></param>
        public void Init(List<SoundAndName> bgmList, List<SoundAndName> seList)
        {
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
            audioSource.PlayOneShot(seDict[clipName], 0.7f);
        }

        //오프셋 실행을 위한 오버로드
        public void PlaySE(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// 현재 재생중인 BGM을 정지
        /// </summary>
        /// <param name="fadeTime">정지시 페이드 인 시간</param>
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
    }
}
