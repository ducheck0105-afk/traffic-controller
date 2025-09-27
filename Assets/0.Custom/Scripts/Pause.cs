using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _0.Custom.Scripts
{
    public class Pause : MonoBehaviour
    {
        public Transform content;

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
       

        public void Close()
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            AudioManager.ins.PlayButtonClick();
        }

        public void Setting()
        {
            AudioManager.ins.PlayButtonClick();
            gameObject.SetActive(false);
            GameController.instance.uiController.ShowSetting();
        }
    }
}