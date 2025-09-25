using System;
using _0.Custom.Scripts.Menu;
using FortuneWheel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _0.Custom.Scripts
{
    public class MenuController : MonoBehaviour
    {
        public static MenuController ins;
        public TextMeshProUGUI txtLevel;
        public Text txtCoin;
        public DailySpin dailySpin;
        public GameObject spinObj;
        public SettingPanel settingPanel;
        public BuyCarUI BuyCarUI;

        private void Awake()
        {
            ins = this;
        }

        private void Start()
        {
            txtLevel.text = $"LELVEL {PlayerData.currentLevel+1}";
            spinObj.gameObject.SetActive(PlayerData.ShouldShowDailyReward());
            PlayerData.onChangeCoin += UpdateTxtCoin;
            UpdateTxtCoin();
        }

        private void OnDestroy()
        {
            PlayerData.onChangeCoin += UpdateTxtCoin;
        }

        private void UpdateTxtCoin()
        {
            txtCoin.text = $"{PlayerData.currentGold}";
        }

        public void StartLevel()
        {
            AudioManager.ins.PlayButtonClick();
            string sceneName = $"Level_{PlayerData.currentLevel}";
            SceneManager.LoadScene(sceneName);
        }

        public void ShowSetting()
        {
            AudioManager.ins.PlayButtonClick();
            settingPanel.gameObject.SetActive(true);
        }

        public void ShowShop()
        {
            AudioManager.ins.PlayButtonClick();
            BuyCarUI.gameObject.SetActive(true);
        }
    }
}