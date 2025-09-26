using System.Collections;
using TMPro;
using UnityEngine;

namespace _0.Custom.Scripts
{
    public class ToastNotifier : MonoBehaviour
    {
        public static ToastNotifier Instance { get; private set; }

        public CanvasGroup canvasGroup; 
        public TextMeshProUGUI messageText; 

        public float fadeIn = 0.15f;

        public float hold = 1.5f;
        public float fadeOut = 0.35f;

        private Coroutine playCo;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (canvasGroup == null) canvasGroup = GetComponentInChildren<CanvasGroup>(true);
            canvasGroup.alpha = 0f;
            gameObject.SetActive(true);
        }

        private void Show(string msg, float? customHold = null)
        {
            if (messageText != null) messageText.text = msg;
            if (playCo != null) StopCoroutine(playCo);
            playCo = StartCoroutine(PlayToast(customHold ?? hold));
        }

        public void ShowNotEnoughMoney()
        {
            Show("Not Enough Money");
        }

        private IEnumerator PlayToast(float holdTime)
        {
            canvasGroup.blocksRaycasts = false; 
            float t = 0f;
            while (t < fadeIn)
            {
                t += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeIn);
                yield return null;
            }

            canvasGroup.alpha = 1f;

            t = 0f;
            while (t < holdTime)
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }

            t = 0f;
            while (t < fadeOut)
            {
                t += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeOut);
                yield return null;
            }

            canvasGroup.alpha = 0f;

            playCo = null;
        }
    }
}