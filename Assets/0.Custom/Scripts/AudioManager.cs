using System;
using UnityEngine;

namespace _0.Custom.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager ins;

        [Header("Music")]
        public AudioClip musicClip;

        [Header("Button SFX")]
        public AudioClip clickClip;
        public AudioClip backClip;
        public AudioClip openClip;

        private AudioSource _musicSrc;
        private AudioSource _sfxSrc;

        private void Awake()
        {
            if (ins != null && ins != this) { Destroy(gameObject); return; }
            ins = this;
            DontDestroyOnLoad(gameObject);

            _musicSrc = gameObject.AddComponent<AudioSource>();
            _musicSrc.loop = true;
            
            _sfxSrc = gameObject.AddComponent<AudioSource>();
            _sfxSrc.loop = false;
            
            SetSfxVolume(PlayerData.SfxVolume);
            SetMusicVolume(PlayerData.MusicVolume);
        }

        private void Start()
        {
            if (musicClip != null) PlayMusic(musicClip, true);
        }

        // ===== Music =====
        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (clip == null) return;
            _musicSrc.clip = clip;
            _musicSrc.loop = loop;
            _musicSrc.volume = PlayerData.MusicVolume;
            _musicSrc.Play();
        }

        public void StopMusic() => _musicSrc.Stop();
        public void SetMusicVolume(float v)
        {
            _musicSrc.volume = PlayerData.MusicVolume;
        }

        // ===== SFX =====
        public void PlaySfx(AudioClip clip)
        {
            if (clip == null) return;
            _sfxSrc.PlayOneShot(clip, PlayerData.SfxVolume);
        }

        public void SetSfxVolume(float v) =>  PlayerData.SfxVolume = Mathf.Clamp01(v);

        // Nút phổ biến
        public void PlayButtonClick() => PlaySfx(clickClip);
        public void PlayButtonBack()  => PlaySfx(backClip);
        public void PlayButtonOpen()  => PlaySfx(openClip);
    }
}