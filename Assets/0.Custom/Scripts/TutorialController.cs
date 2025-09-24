using System.Collections;
using UnityEngine;

namespace _0.Custom.Scripts
{
    public class TutorialController : MonoBehaviour
    {
        public GameObject canvas;
        private float delayTime = 7f;
        private void Start()
        {
            StartCoroutine(StartGame());
        }

        IEnumerator StartGame()
        {
            while (delayTime > 0)
            {
                delayTime -= Time.deltaTime;
                yield return null;
            }

            canvas.SetActive(true);
        }
    }
}