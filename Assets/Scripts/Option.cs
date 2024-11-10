using HandByHand.SoundSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace HandByHand
{
    public class Option : MonoBehaviour
    {
        [SerializeField]
        private GameObject OptionPanel;

        [SerializeField]
        private Slider musicSlider;
        
        [SerializeField]
        private Slider SESlider;

        [SerializeField]
        private Button ResumeButton;

        [SerializeField]
        private Button MainMenuButton;

        private void Start()
        {
            musicSlider.value = SoundManager.Instance.BGMVolume;
            SESlider.value = SoundManager.Instance.SEVolume;
        }

        public void SetActiveOptionPanel()
        {
            if(!OptionPanel.activeSelf)
                OptionPanel.SetActive(true);
            else
                OptionPanel.SetActive(false);
        }

        public void AdjustBGMVolume()
        {
            SoundManager.Instance.AdjustBGMVolume(musicSlider.value);
        }

        public void AdjustSEVolume()
        {
            SoundManager.Instance.AdjustSEVolume(SESlider.value);
        }

        public void ResumeTheGame()
        {
            OptionPanel.SetActive(false);
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("Start Scene", LoadSceneMode.Single);
            SetActiveOptionPanel();
        }
    }
}
