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
        public List<CarAI> carPrefabs;
      
        public Transform carParent;
        public JunctionController juncion;
        public LevelController levelController;
        public CarSpawner carSpawner;
        public UIController uiController;
        public Material lightFloor;
        public List<GameObject> moveStatus;

        private MoveState state = MoveState.Stop;

        private void Awake()
        {
            instance = this;
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
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(1.5f);
            uiController.ShowGameOver(isWin);
        }
    }
}