using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _0.Custom.Scripts
{
    public class GameController : MonoBehaviour
    {
        public static GameController instance;

        public enum MoveState
        {
            Stop = 0,
            Move = 1,
        }

        public float booster = 1;
        public Transform carParent;
        public JunctionController juncion;
        public LevelController levelController;
        public UIController uiController;
        public Material lightFloor;
        public List<GameObject> moveStatus;

        private MoveState state = MoveState.Stop;
        public bool stopAll;

        private void Awake()
        {
            instance = this;
            booster = 1;
        }

        private void Start()
        {
            ConfirmState();
        }

        public void OnClickLight()
        {
            if (state == MoveState.Stop) state = MoveState.Move;
            else state = MoveState.Stop;
            ConfirmState();
            AudioManager.ins.PlayButtonClick();
        }

        public void ConfirmState()
        {
            juncion.ChangeState(state);
            for (int i = 0; i < moveStatus.Count; i++)
            {
                moveStatus[i].SetActive(i == (int)state);
            }

            lightFloor.color = state == MoveState.Move ? Color.green : Color.red;
        }

        public void GameOver(bool isWin)
        {
            StartCoroutine(GameOverIE(isWin));
        }

        private IEnumerator GameOverIE(bool isWin)
        {
            stopAll = true;
            yield return new WaitForSeconds(0.6f);
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(2.5f);
            uiController.ShowGameOver(isWin);
        }

        public void Boost()
        {
            AudioManager.ins.PlayButtonClick();
            if (isBoost) return;
            if (PlayerData.currentGold >= 200)
            {
                PlayerData.currentGold -= 200;
                StartCoroutine(BoostIE());
            }
            else ToastNotifier.Instance.ShowNotEnoughMoney();
        }

        private bool isBoost;

        private IEnumerator BoostIE()
        {
            isBoost = true;
            booster = 1.6f;
            yield return new WaitForSeconds(10f);
            booster = 1;
            isBoost = false;
        }

        public void RemoveCar()
        {
            AudioManager.ins.PlayButtonClick();
            if (PlayerData.currentGold >= 400)
            {
                PlayerData.currentGold -= 400;
                WaypointsHolder.clearCar?.Invoke();
            }
            else
            {
                ToastNotifier.Instance.ShowNotEnoughMoney();
            }
        }
    }
}