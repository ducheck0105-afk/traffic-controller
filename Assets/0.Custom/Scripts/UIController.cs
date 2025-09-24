using UnityEngine;

namespace _0.Custom.Scripts
{
    public class UIController : MonoBehaviour
    {
        public GameOver gameOver;

        public void ShowGameOver(bool isWin)
        {
            gameOver.SetUp(isWin);
            gameOver.gameObject.SetActive(true);
        }
    }
}