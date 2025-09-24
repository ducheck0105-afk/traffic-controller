using DG.Tweening;
using UnityEngine;

namespace _0.Custom.Scripts
{
    public static class Common
    {
        public static bool RandomBool()
        {
            return UnityEngine.Random.value < 0.5f;
        }
        public static bool RandomByPercent(float percent)
        {
            return UnityEngine.Random.Range(0f, 100f) < percent;
        }
        public static void ShowPopup(this Transform obj)
        {
            obj.transform.localScale = Vector3.one * 0.5f;
            obj.DOScale(1, 0.3f).SetEase(Ease.OutBack).SetUpdate(true);
        }
    }
}