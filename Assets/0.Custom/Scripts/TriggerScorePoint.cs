using _0.Custom.Scripts.Gameplay;
using UnityEngine;

namespace _0.Custom.Scripts
{
    public class TriggerScorePoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Car" && other.GetComponent<CarAI>().waypoints.Contains(this.transform))
            {
                var par = Instantiate(CarData.Instance.scoreParticle);
                par.transform.position = other.transform.position;
                GameController.instance.levelController.AddScore();
            }
        }
    }
}