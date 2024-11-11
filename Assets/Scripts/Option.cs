using HandByHand.SoundSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;

namespace HandByHand
{
    public class Option : MonoBehaviour
    {
        [SerializeField]
        private GameObject OptionPanel;

        [SerializeField]
        private Slider musicSlider;
        
        [SerializeField]
        private Slider _SESlider;

        [SerializeField]
        private Button ResumeButton;

        [SerializeField]
        private Button MainMenuButton;

        [SerializeField]
        private Image musicImage;

        [SerializeField]
        private Image _SEImage;

        [SerializeField]
        private Sprite musicMuteSprite;

        [SerializeField]
        private Sprite _SEMuteSprite;

        private Sprite musicSprite;
        private Sprite _SESprite;

        private float preMusicVolume;
        private float preSEVolume;


        private void Start()
        {
            musicSprite = musicImage.sprite;
            _SESprite = _SEImage.sprite;
            
            musicSlider.value = SoundManager.Instance.BGMVolume;
            _SESlider.value = SoundManager.Instance.SEVolume;

            preMusicVolume = SoundManager.Instance.BGMVolume;
            preSEVolume = SoundManager.Instance.SEVolume;

        }

        public void SetActiveOptionPanel()
        {
            if(!OptionPanel.activeSelf)
                OptionPanel.SetActive(true);
            else
                OptionPanel.SetActive(false);
        }

        #region MUSIC
        public void MuteMusic()
        {
            if(musicSlider.value != 0)
            {
                musicSlider.value = 0;
            }
            else
            {
                musicSlider.value = preMusicVolume;
            }

            AdjustBGMVolume();
        }

        public void AdjustBGMVolume()
        {
            SoundManager.Instance.AdjustBGMVolume(musicSlider.value);
            CheckBGMMute();
        }

        private void CheckBGMMute()
        {
            if(SoundManager.Instance.BGMVolume != 0)
                preMusicVolume = SoundManager.Instance.BGMVolume;

            if (SoundManager.Instance.BGMVolume == 0)
            {
                musicImage.sprite = musicMuteSprite;
            }
            else
            {
                musicImage.sprite = musicSprite;
            }
        }
        #endregion

        #region SE

        public void MuteSE()
        {
            if(_SESlider.value != 0)
            {
                _SESlider.value = 0;
            }
            else
            {
                _SESlider.value = preSEVolume;
            }
            
            AdjustSEVolume();
        }
        
        public void AdjustSEVolume()
        {
            SoundManager.Instance.AdjustSEVolume(_SESlider.value);
            CheckSEMute();
        }

        private void CheckSEMute()
        {
            if(SoundManager.Instance.SEVolume != 0)
                preSEVolume = SoundManager.Instance.SEVolume;

            if (SoundManager.Instance.SEVolume == 0)
            {
                _SEImage.sprite = _SEMuteSprite;
            }
            else
            {
                _SEImage.sprite = _SESprite;
            }
        }
        #endregion

        public void ResumeTheGame()
        {
            OptionPanel.SetActive(false);
        }

        public void GoToMainMenu()
        {
            GameManager.Instance.CurrentDayCycle=DayCycle.Start;
            SetActiveOptionPanel();
        }
    }
}
