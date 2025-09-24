using UnityEngine;

namespace _0.Custom.Scripts
{
    public class TriggerScorePoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Car" && other.GetComponent<CarAI>().waypoints.Contains(this.transform))
            {
                GameController.instance.levelController.AddScore();
            }
        }
    }
}