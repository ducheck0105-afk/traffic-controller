using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _0.Custom.Scripts
{
    public class GameOver : MonoBehaviour
    {
        public Transform content;
        public TextMeshProUGUI title;
        public Text reward;
        public Button buttonNext;

        public void SetUp(bool isWin)
        {
            string t = isWin ? "MISSION COMPLETE" : "MISSION FAILED";
            title.text = t;
            buttonNext.interactable = isWin;
            var rw = isWin ? PlayerData.currentLevel * 100 : 0;
            reward.text = $"{rw}";
        }

        private void OnEnable()
        {
            content.ShowPopup();
        }

        public void Home()
        {
            AudioManager.ins.PlayButtonClick();
            Time.timeScale = 1;
            string sceneName = $"MainMenu";
            SceneManager.LoadScene(sceneName);
        }

        public void Restart()
        {
            AudioManager.ins.PlayButtonClick();
            Time.timeScale = 1;

            string sceneName = $"Level_{PlayerData.currentLevel}";
            SceneManager.LoadScene(sceneName);
        }

        public void Next()
        {
            AudioManager.ins.PlayButtonClick();
            Time.timeScale = 1;
            PlayerData.currentLevel += 1;
            string sceneName = $"Level_{PlayerData.currentLevel}";
            SceneManager.LoadScene(sceneName);
        }
    }
}