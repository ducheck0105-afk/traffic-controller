using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _0.Custom.Scripts
{
    public class LevelController : MonoBehaviour
    {
        public int targetCount;

        public int currentScore;

        public ScoreObject scorePrefab;
        public Transform scoreParent;
        [HideInInspector] public List<ScoreObject> scoreObjects;
        public int reward;

        private void Start()
        {
            for (int i = 0; i < targetCount; i++)
            {
                var obj = Instantiate(scorePrefab, scoreParent);
                scoreObjects.Add(obj);
            }
        }

        public void AddScore()
        {
            if (GameController.instance.stopAll) return;
            currentScore++;
            for (int i = 0; i < scoreObjects.Count; i++)
            {
                var obj = scoreObjects[i];
                obj.Complete(currentScore > i);
            }

            if (currentScore >= targetCount)
            {
                GameController.instance.GameOver(true);
            }
        }
    }
}