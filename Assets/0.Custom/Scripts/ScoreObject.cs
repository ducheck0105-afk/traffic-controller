using UnityEngine;

namespace _0.Custom.Scripts
{
    public class ScoreObject : MonoBehaviour
    {
        public GameObject completeObj;

        public void Complete(bool complete)
        {
            completeObj.SetActive(complete);
        }
    }
}