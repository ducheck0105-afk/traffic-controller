using System;
using UnityEngine;
using UnityEngine.UI;

namespace _0.Custom.Scripts
{
    public class SettingPanel : MonoBehaviour
    {
        public Transform content;
        public Slider musicSlider;
        public Slider sfxSlider;


        private void Start()
        {
            musicSlider.value = PlayerData.MusicVolume;
            sfxSlider.value = PlayerData.SfxVolume;
        }

        public void ChangeSfx(float value)
        {
            PlayerData.SfxVolume = value;
        }

        public void ChangeMusic(float value)
        {
            PlayerData.MusicVolume = value;
        }

        private void OnEnable()
        {
            content.ShowPopup();
        }

        public void Close()
        {
            AudioManager.ins.PlayButtonClick();
            gameObject.SetActive(false);
        }
    }
}