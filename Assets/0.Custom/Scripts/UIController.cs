using System;
using UnityEngine;
using UnityEngine.UI;

namespace _0.Custom.Scripts
{
    public class UIController : MonoBehaviour
    {
        public GameOver gameOver;
        public Text txtCoin;
        public Pause pause;
        public SettingPanel settingPanel;
        private void Start()
        {
            PlayerData.onChangeCoin += UpdateTxtCoin;
            UpdateTxtCoin();
        }
        private void OnDestroy()
        {
            PlayerData.onChangeCoin -= UpdateTxtCoin;
        }

        private void UpdateTxtCoin()
        {
            txtCoin.text = $"{PlayerData.currentGold}";
        }
        public void ShowGameOver(bool isWin)
        {
            gameOver.SetUp(isWin);
            gameOver.gameObject.SetActive(true);
        }
        
        public void ShowSetting()
        {
            settingPanel.gameObject.SetActive(true);
        }

        public void ShowPause()
        {
            Time.timeScale = 0;
            pause.gameObject.SetActive(true);
        }

    }
}