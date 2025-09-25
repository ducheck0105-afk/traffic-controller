using System.Collections.Generic;
using UnityEngine;

namespace _0.Custom.Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "CarData", menuName = "Game/Car Data")]
    public class CarData : ScriptableObject
    {
        static CarData ins;

        public static CarData Instance
        {
            get
            {
                if (ins == null)
                {
                    Setup();
                }

                return ins;
            }
        }

        static void Setup()
        {
            ins = Resources.Load<CarData>("CarData");
        }

        public List<CarAI> mainCars;
        public List<CarAI> subCars;
        public GameObject carExplode;
        public GameObject scoreParticle;
        public CarAI GetCarPrefab(CarAI.CarFlow flow)
        {
            if (flow == CarAI.CarFlow.Main) return mainCars[PlayerData.PlayerCar];
            return subCars[Random.Range(0, subCars.Count)];
        }
    }
}