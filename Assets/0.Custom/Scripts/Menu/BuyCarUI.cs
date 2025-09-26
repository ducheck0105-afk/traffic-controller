using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _0.Custom.Scripts.Menu
{
    public class BuyCarUI : MonoBehaviour
    {
        public Color gray;
        public Color green;
        public List<CarUI> carUI;
        public TextMeshProUGUI price;
        public Button buttonBuy;
        public Button buttonEquip;

        private CarUI currentCarUI;

        private void Start()
        {
            foreach (var a in carUI)
            {
                a.button.onClick.AddListener(() => Select(a));
            }

            currentCarUI = carUI[PlayerData.PlayerCar];
            SetEquip();
        }

        private void OnEnable()
        {
            var currentUI = carUI[PlayerData.PlayerCar];
            Select(currentUI);
        }

        private void Select(CarUI ui)
        {
            price.text = ui.price.ToString();
            var index = carUI.IndexOf(ui);
            var unlock = PlayerData.ListCar.Contains(index);
            buttonBuy.interactable = !unlock;
            buttonEquip.interactable = unlock && PlayerData.PlayerCar != index;
            currentCarUI = ui;
            foreach (var a in carUI)
            {
                var cc = ui == a ? green : gray;
                a.Select(cc);
            }
        }

        public void Buy()
        {
            AudioManager.ins.PlayButtonClick();
            if (PlayerData.currentGold < currentCarUI.price)
            {
                ToastNotifier.Instance.ShowNotEnoughMoney();
                return;
            }
            PlayerData.currentGold -= currentCarUI.price;
            var litCar = PlayerData.ListCar;
            var index = carUI.IndexOf(currentCarUI);
            litCar.Add(index);
            PlayerData.ListCar = litCar;

            Select(currentCarUI);
        }

        public void Close()
        {
            AudioManager.ins.PlayButtonClick();
            gameObject.SetActive(false);
        }

        public void Equip()
        {
            AudioManager.ins.PlayButtonClick();
            PlayerData.PlayerCar = carUI.IndexOf(currentCarUI);
            SetEquip();
        }

        private void SetEquip()
        {
            foreach (var a in carUI)
            {
                var cc = currentCarUI == a;
                a.Equip(cc);
            }
        }
    }
}